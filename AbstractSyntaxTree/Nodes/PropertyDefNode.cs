namespace AbstractSyntaxTree.Nodes;

public record PropertyDefNode(IAstNode Id, IAstNode Type, bool IsPrivate) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Id, Type };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Type => this with { Type = to },
            _ => throw new NotImplementedException()
        };
    }
}