using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record TypeNode(IAstNode TypeName) : IAstNode, INamed {
    public IEnumerable<IAstNode> Children => new[] { TypeName };

    public IAstNode Replace(IAstNode from, IAstNode to) => new TypeNode(to);

    public string Name => ((IdentifierNode)TypeName).Id;
}