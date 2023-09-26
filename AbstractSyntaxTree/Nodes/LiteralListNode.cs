using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record LiteralListNode(ImmutableArray<IAstNode> ItemNodes) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
}