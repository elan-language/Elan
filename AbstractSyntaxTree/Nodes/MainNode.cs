namespace AbstractSyntaxTree.Nodes;

public record MainNode : IAstNode {
    public MainNode(IAstNode StatementBlock) {
        this.StatementBlock = StatementBlock;
    }
    public IEnumerable<IAstNode> Children => new[] { StatementBlock };
    public IAstNode StatementBlock { get; init; }
    public void Deconstruct(out IAstNode StatementBlock) {
        StatementBlock = this.StatementBlock;
    }
}