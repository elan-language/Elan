namespace AbstractSyntaxTree.Nodes;

public record WhileStatementNode(IAstNode Expression, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, StatementBlock };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ when from == Expression => this with { Expression = to },
            _ => throw new NotImplementedException()
        };
    }
}