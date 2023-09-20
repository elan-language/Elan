using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record AggregateNode<T> : IAstNode where T : IAstNode {
    public AggregateNode(ImmutableArray<T> AggregatedNodes) {
        this.AggregatedNodes = AggregatedNodes;
    }
    public IEnumerable<IAstNode> Children => AggregatedNodes.Cast<IAstNode>();
    public ImmutableArray<T> AggregatedNodes { get; init; }
    public void Deconstruct(out ImmutableArray<T> AggregatedNodes) {
        AggregatedNodes = this.AggregatedNodes;
    }
}