namespace AbstractSyntaxTree.Nodes;

public record TwoDIndexExpressionNode(IAstNode Expression1, IAstNode Expression2, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression1, Expression2 };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Expression1 => this with { Expression1 = to },
            _ when from == Expression2 => this with { Expression2 = to },

            _ => throw new NotImplementedException()
        };
    }
}