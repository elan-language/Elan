namespace AbstractSyntaxTree.Nodes;

public record BracketNode(IAstNode BracketedNode, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { BracketedNode };

    public IAstNode Replace(IAstNode from, IAstNode to) => new BracketNode(to, 0, 0);
}