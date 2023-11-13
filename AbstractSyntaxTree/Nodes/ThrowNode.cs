namespace AbstractSyntaxTree.Nodes;

public record ThrowNode(IAstNode Thrown) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Thrown };

    public IAstNode Replace(IAstNode from, IAstNode to) => new ThrowNode(to);
}