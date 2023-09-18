namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode(string Id) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}