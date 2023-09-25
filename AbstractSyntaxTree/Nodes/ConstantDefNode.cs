namespace AbstractSyntaxTree.Nodes;

public record ConstantDefNode(IAstNode Id, IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };
}