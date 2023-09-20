namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode : IAstNode {
    public IdentifierNode(string Id) {
        this.Id = Id;
    }
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public string Id { get; init; }
    public void Deconstruct(out string Id) {
        Id = this.Id;
    }
}