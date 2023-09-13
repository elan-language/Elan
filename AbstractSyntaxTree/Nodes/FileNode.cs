namespace AbstractSyntaxTree.Nodes;

public record FileNode(MainNode MainNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { MainNode };
}