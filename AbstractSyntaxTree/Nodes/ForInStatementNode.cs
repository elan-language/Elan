namespace AbstractSyntaxTree.Nodes;

public record ForInStatementNode(IAstNode Id, IAstNode Expression, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression, StatementBlock };
}