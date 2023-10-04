namespace AbstractSyntaxTree.Nodes;

public record AssignmentNode(IAstNode Id, IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Expression => this with { Expression = to },
            _ => throw new NotImplementedException()
        };
    }
}