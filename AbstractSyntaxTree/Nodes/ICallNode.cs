using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public interface ICallNode : INamed {
    public IAstNode Id { get; }

    public IAstNode? Qualifier { get; }

    public ImmutableArray<IAstNode> Parameters { get; }
}