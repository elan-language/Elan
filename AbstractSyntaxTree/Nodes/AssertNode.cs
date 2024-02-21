using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AssertNode(IAstNode Actual, IAstNode Expected, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => new[] { Actual, Expected };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Actual => this with { Actual = to },
            _ when from == Expected => this with { Expected = to },
            _ => throw new NotImplementedException()
        };
    }
}