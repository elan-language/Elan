namespace AbstractSyntaxTree.Nodes;

public record ParameterCallNode(string Id, bool IsRef) : IAstNode {
    public virtual IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public virtual IAstNode Replace(IAstNode from, IAstNode to) => this;
}