using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodCallNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) : IAstNode {
    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);
}