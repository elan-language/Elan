namespace AbstractSyntaxTree.Nodes;

public record CaseNode(IAstNode StatementBlock, IAstNode? Value = null) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { StatementBlock }.SafePrepend(Value);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Value => this with { Value = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => throw new NotImplementedException()
        };
    }
}