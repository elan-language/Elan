namespace AbstractSyntaxTree.Nodes;

public record TryCatchNode(IAstNode TriedCode, IAstNode Id, IAstNode CaughtCode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { TriedCode, Id, CaughtCode };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == TriedCode => this with { TriedCode = to },
            _ when from == CaughtCode => this with { CaughtCode = to },
            _ => throw new NotImplementedException()
        };
    }
}