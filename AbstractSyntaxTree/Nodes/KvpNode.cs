namespace AbstractSyntaxTree.Nodes;

public record KvpNode(IAstNode Key, IAstNode Value) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] {  Key, Value };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
           
            _ when from == Key => this with { Key = to },
            _ when from == Value => this with { Value = to },
            _ => throw new NotImplementedException()
        };
    }
}