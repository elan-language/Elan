namespace AbstractSyntaxTree.Nodes;

public record TypeNode(IAstNode TypeName) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { TypeName };
    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == TypeName => this with { TypeName = to },
            _ => throw new NotImplementedException()
        };
    }
}