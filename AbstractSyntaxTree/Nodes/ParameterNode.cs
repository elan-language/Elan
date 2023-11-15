namespace AbstractSyntaxTree.Nodes;

public record ParameterNode(IAstNode Id, IAstNode TypeNode, bool IsRef = false) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, TypeNode };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == TypeNode => this with { TypeNode = to },
            _ => throw new NotImplementedException()
        };
    }
}