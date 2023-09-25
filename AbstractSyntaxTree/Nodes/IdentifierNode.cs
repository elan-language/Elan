namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode : IAstNode {
    public IdentifierNode(string Id) => this.Id = Id;

    public string Id { get; init; }
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();

    public void Deconstruct(out string Id) {
        Id = this.Id;
    }
}