namespace AbstractSyntaxTree.Nodes;

public interface IAstNode {
    public IEnumerable<IAstNode> Children { get; }
}