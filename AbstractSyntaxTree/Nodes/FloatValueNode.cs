namespace AbstractSyntaxTree.Nodes;

public record FloatValueNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}