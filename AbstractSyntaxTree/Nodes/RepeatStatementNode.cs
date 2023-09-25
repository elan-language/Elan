namespace AbstractSyntaxTree.Nodes;

public record RepeatStatementNode(IAstNode Expression, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, StatementBlock };
}