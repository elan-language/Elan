namespace AbstractSyntaxTree.Nodes;

public record ProcedureDefNode(IAstNode Signature, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Signature, StatementBlock };
}