namespace AbstractSyntaxTree.Nodes;

public record OperatorNode(Operator Value) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this;
}