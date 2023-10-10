using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record LiteralDictionaryNode(ImmutableArray<IAstNode> ItemNodes) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
    public IAstNode Replace(IAstNode from, IAstNode to) => new LiteralDictionaryNode(ItemNodes.SafeReplace(from, to));
}