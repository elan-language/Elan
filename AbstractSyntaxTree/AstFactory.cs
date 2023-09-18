using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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
            ElanParser.VarDefContext c => visitor.Build(c),
            ElanParser.ConstantDefContext c => visitor.Build(c),
            ElanParser.AssignableValueContext c => visitor.Build(c),
            ElanParser.AssignmentContext c => visitor.Build(c),

            _ => throw new NotImplementedException(context?.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new IdentifierNode(node.Symbol.Text);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.FileContext context) {
        var mainNode = context.main().Select(visitor.Visit<MainNode>).Single();

        var globalNodes = context.constantDef().Select(visitor.Visit).ToImmutableArray();

        return new FileNode(globalNodes, mainNode);
    }

    private static AggregateNode<IAstNode> Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.StatementBlockContext context) {
        var statements = context.children.Select(visitor.Visit).ToImmutableArray();
        return new AggregateNode<IAstNode>(statements);
    }

    private static MainNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.MainContext context) {
        var statements = visitor.Visit<AggregateNode<IAstNode>>(context.statementBlock());

        return new MainNode(statements.AggregatedNodes);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.CallStatementContext context) => visitor.Visit(context.expression());

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ExpressionContext context) {
        if (context.methodCall() is { } mc) {
            return visitor.Visit(mc);
        }

        if (context.value() is { } v) {
            return visitor.Visit(v);
        }

        throw new NotImplementedException();
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.MethodCallContext context) {
        var id = visitor.Visit(context.IDENTIFIER());
        var pps = (context.argumentList()?.expression().Select(visitor.Visit) ?? Array.Empty<IAstNode>()).ToImmutableArray();

        return new MethodCallNode(id, pps);
    }

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ValueContext context) {
        if (context.literalValue() is { } lv) {
            return visitor.Visit(context.literalValue());
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
        var expr = visitor.Visit(context.expression());

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
        if (context.LITERAL_STRING() is { } s) {
            return new StringValueNode(s.Symbol.Text);
        }

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
}