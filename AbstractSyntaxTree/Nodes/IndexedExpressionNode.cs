namespace AbstractSyntaxTree.Nodes;

public record IndexedExpressionNode(IAstNode Expression, IAstNode Range) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression, Range };
}