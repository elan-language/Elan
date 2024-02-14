namespace AbstractSyntaxTree.Nodes;

public record ThrowNode(IAstNode Thrown, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Thrown };

    public IAstNode Replace(IAstNode from, IAstNode to) => new ThrowNode(to, 0, 0);
}