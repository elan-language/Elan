using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public interface ICallNode : IHasScope {
    public IAstNode Id { get; }

    public IAstNode? Qualifier { get; }

    public IAstNode? CalledOn { get; }

    public string MethodName { get; }

    public ImmutableArray<IAstNode> Parameters { get; }
}