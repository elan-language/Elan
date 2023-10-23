namespace AbstractSyntaxTree.Nodes;

public record ReturnExpressionNode(IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression };

    public IAstNode Replace(IAstNode from, IAstNode to) => new ReturnExpressionNode(to);
}