﻿using System.Collections.Immutable;
using System.Linq.Expressions;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static ElanParser;

namespace AbstractSyntaxTree;

public static class AstFactory {
    private static T Visit<T>(this ElanBaseVisitor<IAstNode> visitor, IParseTree pt) where T : IAstNode => (T)visitor.Visit(pt);

    public static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ParserRuleContext context) =>
        context switch {
            ElanParser.FileContext c => visitor.Build(c),
            ElanParser.MainContext c => visitor.Build(c),
            ElanParser.StatementBlockContext c => visitor.Build(c),
            ElanParser.CallStatementContext c => visitor.Build(c),
            ElanParser.ExpressionContext c => visitor.Build(c),
            ElanParser.MethodCallContext c => visitor.Build(c),
            ElanParser.ValueContext c => visitor.Build(c),
            ElanParser.LiteralValueContext c => visitor.Build(c),
            ElanParser.LiteralDataStructureContext c => visitor.Build(c),
            ElanParser.VarDefContext c => visitor.Build(c),
            ElanParser.ConstantDefContext c => visitor.Build(c),
            ElanParser.AssignableValueContext c => visitor.Build(c),
            ElanParser.AssignmentContext c => visitor.Build(c),
            ElanParser.LiteralContext c => visitor.Build(c),
            ElanParser.BinaryOpContext c => visitor.Build(c),
            ElanParser.ArithmeticOpContext c => visitor.Build(c),
            ElanParser.LogicalOpContext c => visitor.Build(c),
            ElanParser.ProceduralControlFlowContext c => visitor.Build(c),
            ElanParser.IfContext c => visitor.Build(c),
            ElanParser.WhileContext c => visitor.Build(c),
            ElanParser.RepeatContext c => visitor.Build(c),
            ElanParser.ConditionalOpContext c => visitor.Build(c),
            ElanParser.UnaryOpContext c => visitor.Build(c),
            ElanParser.LiteralListContext c => visitor.Build(c),
            ElanParser.IndexContext c => visitor.Build(c),
            ElanParser.RangeContext c => visitor.Build(c),

            _ => throw new NotImplementedException(context?.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new IdentifierNode(node.Symbol.Text);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.FileContext context) {
        var mainNode = context.main().Select(visitor.Visit<MainNode>).Single();

        var globalNodes = context.constantDef().Select(visitor.Visit).ToImmutableArray();

        return new FileNode(globalNodes, mainNode);
    }

    private static StatementBlockNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.StatementBlockContext context) {
        var statements = context.children.Select(visitor.Visit).ToImmutableArray();
        return new StatementBlockNode(statements);
    }

    private static MainNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.MainContext context) {
        var block = visitor.Visit<StatementBlockNode>(context.statementBlock());

        return new MainNode(block);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.CallStatementContext context) =>  
        new CallStatementNode(visitor.Visit(context.expression()));

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ExpressionContext context) {
        if (context.DOT() is not null) {
            if (context.methodCall() is { } dmc) {
                var ms = visitor.Visit<MethodCallNode>(dmc);
                var exp = visitor.Visit(context.expression().First());
                return new MethodCallNode(ms, exp);
            }
        }
        
        if (context.methodCall() is { } mc) {
            return visitor.Visit(mc);
        }

        if (context.binaryOp() is { } bop) {
            return new BinaryNode(visitor.Visit(bop), visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()));
        }

        if (context.POWER() is { } p) {
            var op = new OperatorNode(Helpers.MapOperator(p.Symbol.Type));
            return new BinaryNode(op, visitor.Visit(context.expression().First()), visitor.Visit(context.expression().Last()));
        }

        if (context.value() is { } v) {
            return visitor.Visit(v);
        }

        if (context.bracketedExpression() is { } b) {
            return new BracketNode(visitor.Visit(b.expression()));
        }

        if (context.unaryOp() is { } uop) {
            return new UnaryNode(visitor.Visit(uop), visitor.Visit(context.expression().Single()));
        }

        if (context.index() is { } idx) {
            var expr = visitor.Visit(context.expression().Single());
            var range = visitor.Visit(idx);

            return new IndexedExpressionNode(expr, range);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.MethodCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.expression().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();

        return new MethodCallNode(id, pps);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ValueContext context) {
        if (context.literal() is { } lv) {
            return visitor.Visit(context.literal());
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.VarDefContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new VarDefNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ConstantDefContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expr = visitor.Visit(context.literal());

        return new ConstantDefNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.AssignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new AssignmentNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.AssignableValueContext context) {
        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LiteralValueContext context) {
        if (context.LITERAL_INTEGER() is { } i) {
            return new IntegerValueNode(i.Symbol.Text);
        }

        if (context.LITERAL_FLOAT() is { } f) {
            return new FloatValueNode(f.Symbol.Text);
        }

        if (context.LITERAL_CHAR() is { } c) {
            return new CharValueNode(c.Symbol.Text);
        }

        if (context.BOOL_VALUE() is { } b) {
            return new BoolValueNode(b.Symbol.Text);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LiteralDataStructureContext context) {
        if (context.LITERAL_STRING() is { } ls) {
            return new StringValueNode(ls.Symbol.Text);
        }

        if (context.literalList() is { } ll) {
            return visitor.Visit(ll);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LiteralContext context) {
        if (context.literalValue() is { } lv) {
            return visitor.Visit(lv);
        }

        if (context.literalDataStructure() is { } lds) {
            return visitor.Visit(lds);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.BinaryOpContext context) {
        if (context.arithmeticOp() is { } ao) {
            return visitor.Visit(ao);
        }

        if (context.conditionalOp() is { } co) {
            return visitor.Visit(co);
        }

        if (context.logicalOp() is { } lo) {
            return visitor.Visit(lo);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.UnaryOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ArithmeticOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ConditionalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LogicalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ProceduralControlFlowContext context) {
        if (context.@if() is { } ic) {
            return visitor.Visit(ic);
        }

        if (context.@while() is { } w) {
            return visitor.Visit(w);
        }

        if (context.repeat() is { } r) {
            return visitor.Visit(r);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.IfContext context) {
        var expressions = context.expression().Select(visitor.Visit);
        var statementBlocks = context.statementBlock().Select(visitor.Visit);

        return new IfStatementNode(expressions.ToImmutableArray(), statementBlocks.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.WhileContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock =visitor.Visit(context.statementBlock());

        return new WhileStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.RepeatContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock =visitor.Visit(context.statementBlock());

        return new RepeatStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LiteralListContext context) {
        var items = context.literal().Select(visitor.Visit);

        return new LiteralListNode(items.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IndexContext context) {
        if (context.range() is { } rangeContext) {
            return visitor.Visit(rangeContext);
        }

        return visitor.Visit(context.expression().Single());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RangeContext context) {
        var prefix = context.children.First() is ITerminalNode;
        var expr1 = visitor.Visit(context.children[prefix ? 1 : 0]);
        var expr2 = context.ChildCount == 3 ? visitor.Visit(context.children[2]) : null;

        return new RangeExpressionNode(prefix, expr1, expr2);
    }
}