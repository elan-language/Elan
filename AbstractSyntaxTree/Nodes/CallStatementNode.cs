namespace AbstractSyntaxTree.Nodes;

public record CallStatementNode(IAstNode CallNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { CallNode };
}