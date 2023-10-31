using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record DeconstructionNode(ImmutableArray<IAstNode> ItemNodes) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
    public IAstNode Replace(IAstNode from, IAstNode to) => new LiteralListNode(ItemNodes.SafeReplace(from, to));
}