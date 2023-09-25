namespace AbstractSyntaxTree.Nodes;

public record WhileStatementNode(IAstNode Expression, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, StatementBlock };
}