namespace AbstractSyntaxTree.Nodes;

public record BracketNode : IAstNode {
    public BracketNode(IAstNode BracketedNode) {
        this.BracketedNode = BracketedNode;
    }
    public IEnumerable<IAstNode> Children => new[] { BracketedNode };
    public IAstNode BracketedNode { get; init; }
    public void Deconstruct(out IAstNode BracketedNode) {
        BracketedNode = this.BracketedNode;
    }
}