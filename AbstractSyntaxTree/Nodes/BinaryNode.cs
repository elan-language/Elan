namespace AbstractSyntaxTree.Nodes;

public record BinaryNode(IAstNode Operator, IAstNode Operand1, IAstNode Operand2) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand1, Operand2 };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Operator => this with { Operator = to },
            _ when from == Operand1 => this with { Operand1 = to },
            _ when from == Operand2 => this with { Operand2 = to },
            _ => throw new NotImplementedException()
        };
    }
}