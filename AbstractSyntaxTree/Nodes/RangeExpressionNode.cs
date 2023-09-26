namespace AbstractSyntaxTree.Nodes;

public record RangeExpressionNode(bool Prefix, IAstNode Expression1, IAstNode? Expression2) : IAstNode {
    public IEnumerable<IAstNode> Children => Expression2 is null ? new[] { Expression1 } : new[] { Expression1, Expression2 };
}