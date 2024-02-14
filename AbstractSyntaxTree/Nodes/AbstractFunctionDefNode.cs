using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AbstractFunctionDefNode(IAstNode Signature, int Line, int Column) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractFunctionDefNode(to, 0, 0);

    public string Name => ((INamedAstNode)Signature).Name;
}