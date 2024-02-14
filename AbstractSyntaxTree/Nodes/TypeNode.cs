using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record TypeNode(IAstNode TypeName, int Line, int Column) : IAstNode, INamedAstNode {
    public IEnumerable<IAstNode> Children => new[] { TypeName };

    public IAstNode Replace(IAstNode from, IAstNode to) => new TypeNode(to, 0, 0);

    public string Name => ((IdentifierNode)TypeName).Id;
}