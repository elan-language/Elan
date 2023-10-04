namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode(string Id) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this;
}