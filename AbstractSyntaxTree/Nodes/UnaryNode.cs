namespace AbstractSyntaxTree.Nodes;

public record UnaryNode(IAstNode Operator, IAstNode Operand, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Operator, Operand };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Operator => this with { Operator = to },
            _ when from == Operand => this with { Operand = to },

            _ => throw new NotImplementedException()
        };
    }
}