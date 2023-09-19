namespace AbstractSyntaxTree.Nodes;

public record BinaryNode(IAstNode Operator, IAstNode Operand1, IAstNode Operand2) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand1, Operand2 };
}