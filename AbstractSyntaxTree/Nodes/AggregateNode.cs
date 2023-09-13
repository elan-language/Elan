using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record AggregateNode<T>(ImmutableArray<T> AggregatedNodes) : IAstNode where T : IAstNode {
    public IEnumerable<IAstNode> Children => AggregatedNodes.Cast<IAstNode>();
}