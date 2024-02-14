namespace AbstractSyntaxTree.Nodes;

public record EnumValueNode(string Value, IAstNode TypeNode, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this with { TypeNode = to };
}