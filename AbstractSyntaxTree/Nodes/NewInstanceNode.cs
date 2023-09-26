namespace AbstractSyntaxTree.Nodes;

public record NewInstanceNode(IAstNode Type) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Type };
}