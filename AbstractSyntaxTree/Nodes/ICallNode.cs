using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public interface ICallNode {
    public IAstNode Id { get; }

    public IAstNode? Qualifier { get; }

    public ImmutableArray<IAstNode> Parameters { get; }

    public string Name { get; }
}