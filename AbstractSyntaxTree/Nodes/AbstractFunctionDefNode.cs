namespace AbstractSyntaxTree.Nodes;

public record AbstractFunctionDefNode(IAstNode Signature) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractFunctionDefNode(to);
}