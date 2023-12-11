using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AbstractProcedureDefNode(IAstNode Signature) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractProcedureDefNode(to);

    public string Name => ((INamedAstNode)Signature).Name;
}