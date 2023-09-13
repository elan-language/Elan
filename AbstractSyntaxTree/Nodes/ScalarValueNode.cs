namespace AbstractSyntaxTree.Nodes;

public record ScalarValueNode(string Id) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}