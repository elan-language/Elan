namespace AbstractSyntaxTree.Nodes;

public record OperatorNode : IAstNode {
    public OperatorNode(Operator Value) {
        this.Value = Value;
    }
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public Operator Value { get; init; }
    public void Deconstruct(out Operator Value) {
        Value = this.Value;
    }
}