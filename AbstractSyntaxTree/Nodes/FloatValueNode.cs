namespace AbstractSyntaxTree.Nodes;

public record FloatValueNode : IScalarValueNode {
    public FloatValueNode(string Value) {
        this.Value = Value;
    }
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public string Value { get; init; }
    public void Deconstruct(out string Value) {
        Value = this.Value;
    }
}