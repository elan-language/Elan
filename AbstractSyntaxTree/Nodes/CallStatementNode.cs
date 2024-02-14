namespace AbstractSyntaxTree.Nodes;

public record CallStatementNode(IAstNode CallNode, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { CallNode };

    public IAstNode Replace(IAstNode from, IAstNode to) => new CallStatementNode(to, 0, 0);
}