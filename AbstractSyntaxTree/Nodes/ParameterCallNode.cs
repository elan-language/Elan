namespace AbstractSyntaxTree.Nodes;

public record ParameterCallNode(IAstNode Expression, bool IsRef) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Expression };
    public IAstNode Replace(IAstNode from, IAstNode to) => this with { Expression = to };
}