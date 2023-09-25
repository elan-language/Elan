namespace AbstractSyntaxTree.Nodes;

public record UnaryNode : IAstNode {
    public UnaryNode(IAstNode Operator, IAstNode Operand) {
        this.Operator = Operator;
        this.Operand = Operand;
    }

    public IAstNode Operator { get; init; }
    public IAstNode Operand { get; init; }
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand };

    public void Deconstruct(out IAstNode Operator, out IAstNode Operand) {
        Operator = this.Operator;
        Operand = this.Operand;
    }
}