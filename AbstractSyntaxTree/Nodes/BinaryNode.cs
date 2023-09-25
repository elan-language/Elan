namespace AbstractSyntaxTree.Nodes;

public record BinaryNode : IAstNode {
    public BinaryNode(IAstNode Operator, IAstNode Operand1, IAstNode Operand2) {
        this.Operator = Operator;
        this.Operand1 = Operand1;
        this.Operand2 = Operand2;
    }

    public IAstNode Operator { get; init; }
    public IAstNode Operand1 { get; init; }
    public IAstNode Operand2 { get; init; }
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand1, Operand2 };

    public void Deconstruct(out IAstNode Operator, out IAstNode Operand1, out IAstNode Operand2) {
        Operator = this.Operator;
        Operand1 = this.Operand1;
        Operand2 = this.Operand2;
    }
}