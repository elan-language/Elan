namespace AbstractSyntaxTree.Nodes;

public record BracketNode(IAstNode BracketedNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { BracketedNode };
}