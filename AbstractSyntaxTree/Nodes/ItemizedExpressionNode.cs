namespace AbstractSyntaxTree.Nodes;

public record ItemizedExpressionNode(IAstNode Expression, IAstNode Range) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, Range };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Range => this with { Range = to },
            _ when from == Expression => this with { Expression = to },
            _ => throw new NotImplementedException()
        };
    }
}