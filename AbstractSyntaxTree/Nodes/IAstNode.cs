namespace AbstractSyntaxTree.Nodes;

public interface IAstNode {
    public IEnumerable<IAstNode> Children { get; }

    public IAstNode Replace(IAstNode from, IAstNode to);
}