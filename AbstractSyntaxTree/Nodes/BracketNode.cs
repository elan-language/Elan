namespace AbstractSyntaxTree.Nodes;

public record BracketNode(IAstNode BracketedNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { BracketedNode };

    public IAstNode Replace(IAstNode from, IAstNode to) => new BracketNode(to);
}