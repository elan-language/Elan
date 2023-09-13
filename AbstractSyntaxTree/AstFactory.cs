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

            _ => throw new NotImplementedException(context?.GetType().FullName ?? null)
        };

    public static IAstNode BuildTerminal(this ElanBaseVisitor<IAstNode> visitor, ITerminalNode node) => new ScalarValueNode(node.Symbol.Text);

    private static FileNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.FileContext context) {
        var mainNode = context.main().Select(visitor.Visit<MainNode>).Single();
        return new FileNode(mainNode);
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

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.ValueContext context) => visitor.Visit(context.literalValue());

    private static IAstNode Build(this ElanBaseVisitor<IAstNode> visitor, ElanParser.LiteralValueContext context) => visitor.Visit(context.LITERAL_STRING());
}