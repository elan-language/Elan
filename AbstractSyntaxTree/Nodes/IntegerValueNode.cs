namespace AbstractSyntaxTree.Nodes;

public record IntegerValueNode(string Value) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}