namespace AbstractSyntaxTree.Nodes;

public record ValueTypeNode(ValueType Type) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}