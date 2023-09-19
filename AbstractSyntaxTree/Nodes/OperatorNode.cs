namespace AbstractSyntaxTree.Nodes;

public record OperatorNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}