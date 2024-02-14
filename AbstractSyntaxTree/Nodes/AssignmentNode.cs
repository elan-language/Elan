using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AssignmentNode(IAstNode Id, IAstNode Rhs, bool Inline, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => new[] { Id, Rhs };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Rhs => this with { Rhs = to },
            _ => throw new NotImplementedException()
        };
    }
}