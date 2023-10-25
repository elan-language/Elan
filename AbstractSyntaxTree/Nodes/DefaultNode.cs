namespace AbstractSyntaxTree.Nodes;

public record DefaultNode(IAstNode Type) : IAstNode {
    public TypeType TypeType => Type switch {
        ValueNode => TypeType.Value,
        IdentifierNode => TypeType.Class,
        TypeNode => TypeType.Class,
        DataStructureTypeNode => TypeType.Class,
        ValueTypeNode => TypeType.Value,
        _ => throw new NotImplementedException(Type.GetType().ToString())
    };

    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => new DefaultNode(to);
}