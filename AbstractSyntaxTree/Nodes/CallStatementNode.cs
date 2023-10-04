namespace AbstractSyntaxTree.Nodes;

public record CallStatementNode(IAstNode CallNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { CallNode };

    public IAstNode Replace(IAstNode from, IAstNode to) => new CallStatementNode(to);
}