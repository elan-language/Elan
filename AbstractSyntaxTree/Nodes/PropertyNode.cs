namespace AbstractSyntaxTree.Nodes;

public record PropertyNode(IAstNode Expression, IAstNode Property) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, Property };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Property => this with { Property = to },
            _ when from == Expression => this with { Expression = to },
            _ => throw new NotImplementedException()
        };
    }
}