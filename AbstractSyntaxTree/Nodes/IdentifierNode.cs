namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode(string Id) : IAstNode {
    public virtual IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public virtual IAstNode Replace(IAstNode from, IAstNode to) => this;
}