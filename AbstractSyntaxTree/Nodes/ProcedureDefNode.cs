namespace AbstractSyntaxTree.Nodes;

public record ProcedureDefNode(IAstNode Signature, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Signature, StatementBlock };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Signature => this with { Signature = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => throw new NotImplementedException()
        };
    }
}