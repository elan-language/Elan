namespace AbstractSyntaxTree.Nodes;

public record ValueNode(string Value, ValueTypeNode TypeNode) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}