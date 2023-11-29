namespace AbstractSyntaxTree.Nodes;

public record AbstractProcedureDefNode(IAstNode Signature) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractProcedureDefNode(to);
}