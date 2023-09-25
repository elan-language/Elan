namespace AbstractSyntaxTree.Nodes;

public record AssignmentNode(IAstNode Id, IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };
}