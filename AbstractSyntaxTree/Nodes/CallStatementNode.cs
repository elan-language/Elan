namespace AbstractSyntaxTree.Nodes;

public record CallStatementNode : IAstNode {
    public CallStatementNode(IAstNode CallNode) {
        this.CallNode = CallNode;
    }
    public IEnumerable<IAstNode> Children => new[] { CallNode };
    public IAstNode CallNode { get; init; }
    public void Deconstruct(out IAstNode CallNode) {
        CallNode = this.CallNode;
    }
}