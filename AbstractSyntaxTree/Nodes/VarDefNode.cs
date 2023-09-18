namespace AbstractSyntaxTree.Nodes;

public record VarDefNode(IAstNode Id, IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };
}