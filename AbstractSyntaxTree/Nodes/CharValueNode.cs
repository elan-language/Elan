namespace AbstractSyntaxTree.Nodes;

public record CharValueNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}