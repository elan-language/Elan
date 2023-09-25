namespace AbstractSyntaxTree.Nodes;

public record BoolValueNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}