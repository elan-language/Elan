namespace AbstractSyntaxTree.Nodes;

public interface IAstNode {
    public IEnumerable<IAstNode> Children { get; }

    public int Line { get; }

    public int Column { get; }

    public IAstNode Replace(IAstNode from, IAstNode to);
}