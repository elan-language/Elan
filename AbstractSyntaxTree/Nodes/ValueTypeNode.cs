namespace AbstractSyntaxTree.Nodes;

public record ValueTypeNode(ValueType Type) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this;
}