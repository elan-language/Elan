namespace AbstractSyntaxTree.Nodes;

public record IdentifierNode(string Id, int Line, int Column) : IAstNode {
    public virtual IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public virtual IAstNode Replace(IAstNode from, IAstNode to) => this;
}