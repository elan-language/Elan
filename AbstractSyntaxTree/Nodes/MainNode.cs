namespace AbstractSyntaxTree.Nodes;

public record MainNode(IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { StatementBlock };
    public IAstNode Replace(IAstNode from, IAstNode to) => new MainNode(to);
}