namespace AbstractSyntaxTree.Nodes;

public record StringValueNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}