namespace AbstractSyntaxTree.Nodes;

public record UnaryNode(IAstNode Operator, IAstNode Operand) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand };
}