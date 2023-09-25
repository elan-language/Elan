namespace AbstractSyntaxTree.Nodes;

public record ValueNode(string Value, ISymbolType SymbolType) : IScalarValueNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
}