namespace AbstractSyntaxTree.Nodes;

public record ValueNode(string Value, IAstNode TypeNode, int Line, int Column) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this with { TypeNode = to };
}