namespace AbstractSyntaxTree.Nodes;

public record TwoDIndexExpressionNode(IAstNode Expression1, IAstNode Expression2) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression1, Expression2 };
}