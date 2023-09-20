namespace AbstractSyntaxTree.Nodes;

public record CharValueNode : IScalarValueNode {
    public CharValueNode(string Value) {
        this.Value = Value;
    }
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public string Value { get; init; }
    public void Deconstruct(out string Value) {
        Value = this.Value;
    }
}