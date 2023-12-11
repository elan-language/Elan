using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AbstractFunctionDefNode(IAstNode Signature) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractFunctionDefNode(to);

    public string Name => ((INamedAstNode)Signature).Name;
}