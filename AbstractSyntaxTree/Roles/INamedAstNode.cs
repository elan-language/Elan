using AbstractSyntaxTree.Nodes;

namespace AbstractSyntaxTree.Roles;

public interface INamedAstNode : IAstNode {
    public string Name { get; }
}