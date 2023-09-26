using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static ElanParser;

namespace AbstractSyntaxTree;

public static class AstFactory {
    private static T Visit<T>(this ElanBaseVisitor<IAstNode> visitor, IParseTree pt) where T : IAstNode => (T)visitor.Visit(pt);

    public static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ParserRuleContext context) =>
        context switch {
            FileContext c => visitor.Build(c),
            MainContext c => visitor.Build(c),
            StatementBlockContext c => visitor.Build(c),
            CallStatementContext c => visitor.Build(c),
            ExpressionContext c => visitor.Build(c),
            MethodCallContext c => visitor.Build(c),
            ValueContext c => visitor.Build(c),
            LiteralValueContext c => visitor.Build(c),
            LiteralDataStructureContext c => visitor.Build(c),
            VarDefContext c => visitor.Build(c),
            ConstantDefContext c => visitor.Build(c),
            AssignableValueContext c => visitor.Build(c),
            AssignmentContext c => visitor.Build(c),
            LiteralContext c => visitor.Build(c),
            BinaryOpContext c => visitor.Build(c),
            ArithmeticOpContext c => visitor.Build(c),
            LogicalOpContext c => visitor.Build(c),
            ProceduralControlFlowContext c => visitor.Build(c),
            IfContext c => visitor.Build(c),
            WhileContext c => visitor.Build(c),
            RepeatContext c => visitor.Build(c),
            ConditionalOpContext c => visitor.Build(c),
            UnaryOpContext c => visitor.Build(c),
            LiteralListContext c => visitor.Build(c),
            IndexContext c => visitor.Build(c),
            RangeContext c => visitor.Build(c),
            NewInstanceContext c => visitor.Build(c),
            TypeContext c => visitor.Build(c),
            DataStructureTypeContext c => visitor.Build(c),

            _ => throw new NotImplementedException(context?.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new IdentifierNode(node.Symbol.Text);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, FileContext context) {
        var mainNode = context.main().Select(visitor.Visit<MainNode>).Single();

        var globalNodes = context.constantDef().Select(visitor.Visit).ToImmutableArray();

        return new FileNode(globalNodes, mainNode);
    }

    private static StatementBlockNode Build(this ElanBaseVisitor<IAstNode> visitor, StatementBlockContext context) {
        var statements = context.children.Select(visitor.Visit).ToImmutableArray();
        return new StatementBlockNode(statements);
    }

    private static MainNode Build(this ElanBaseVisitor<IAstNode> visitor, MainContext context) {
        var block = visitor.Visit<StatementBlockNode>(context.statementBlock());

        return new MainNode(block);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, CallStatementContext context) =>
        new CallStatementNode(visitor.Visit(context.expression()));

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ExpressionContext context) {
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

        if (context.newInstance() is { } ni) {
            return visitor.Visit(ni);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, MethodCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.expression().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();

        return new MethodCallNode(id, pps);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ValueContext context) {
        if (context.literal() is { } lv) {
            return visitor.Visit(context.literal());
        }

        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, VarDefContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new VarDefNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConstantDefContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var expr = visitor.Visit(context.literal());

        return new ConstantDefNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignmentContext context) {
        var id = visitor.Visit(context.assignableValue());
        var expr = visitor.Visit(context.expression());

        return new AssignmentNode(id, expr);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, AssignableValueContext context) {
        if (context.IDENTIFIER() is { } id) {
            return visitor.Visit(id);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralValueContext context) {
        if (context.LITERAL_INTEGER() is { } i) {
            return new ValueNode(i.Symbol.Text, new ValueTypeNode(ValueType.Int));
        }

        if (context.LITERAL_FLOAT() is { } f) {
            return new ValueNode(f.Symbol.Text, new ValueTypeNode(ValueType.Float));
        }

        if (context.LITERAL_CHAR() is { } c) {
            return new ValueNode(c.Symbol.Text, new ValueTypeNode(ValueType.Char));
        }

        if (context.BOOL_VALUE() is { } b) {
            return new ValueNode(b.Symbol.Text, new ValueTypeNode(ValueType.Bool));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralDataStructureContext context) {
        if (context.LITERAL_STRING() is { } ls) {
            return new ValueNode(ls.Symbol.Text, new ValueTypeNode(ValueType.String));
        }

        if (context.literalList() is { } ll) {
            return visitor.Visit(ll);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralContext context) {
        if (context.literalValue() is { } lv) {
            return visitor.Visit(lv);
        }

        if (context.literalDataStructure() is { } lds) {
            return visitor.Visit(lds);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, BinaryOpContext context) {
        if (context.arithmeticOp() is { } ao) {
            return visitor.Visit(ao);
        }

        if (context.conditionalOp() is { } co) {
            return visitor.Visit(co);
        }

        if (context.logicalOp() is { } lo) {
            return visitor.Visit(lo);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, UnaryOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ArithmeticOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ConditionalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LogicalOpContext context) {
        if (context.children.First() is ITerminalNode tn) {
            return new OperatorNode(Helpers.MapOperator(tn.Symbol.Type));
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ProceduralControlFlowContext context) {
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

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, IfContext context) {
        var expressions = context.expression().Select(visitor.Visit);
        var statementBlocks = context.statementBlock().Select(visitor.Visit);

        return new IfStatementNode(expressions.ToImmutableArray(), statementBlocks.ToImmutableArray());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, WhileContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new WhileStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, RepeatContext context) {
        var expression = visitor.Visit(context.expression());
        var statementBlock = visitor.Visit(context.statementBlock());

        return new RepeatStatementNode(expression, statementBlock);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, LiteralListContext context) {
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

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, NewInstanceContext context) {
        var type = visitor.Visit(context.type());

        return new NewInstanceNode(type);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, TypeContext context) {
        if (context.dataStructureType() is { } dst) {
            return visitor.Visit(dst);
        }

        if (context.VALUE_TYPE() is { } vt) {
            return new ValueTypeNode(ValueType.Int);
        }

        throw new NotImplementedException(context.children.First().GetText());
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, DataStructureTypeContext context) {
        var genericTypes = context.genericSpecifier().type().Select(visitor.Visit);

        if (context.LIST() is not null) {
            return new DataStructureTypeNode(DataStructure.List, genericTypes.ToImmutableArray());
        }

        throw new NotImplementedException(context.children.First().GetText());
    }
}