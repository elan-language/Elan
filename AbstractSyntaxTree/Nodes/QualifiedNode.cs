namespace AbstractSyntaxTree.Nodes;

public record QualifiedNode(IAstNode Qualifier, IAstNode Qualified) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Qualifier, Qualified };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Qualifier => this with { Qualifier = to },
            _ when from == Qualified => this with { Qualified = to },
            _ => throw new NotImplementedException()
        };
    }
}