using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record LiteralDictionaryNode(ImmutableArray<IAstNode> ItemNodes, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
    public IAstNode Replace(IAstNode from, IAstNode to) => new LiteralDictionaryNode(ItemNodes.SafeReplace(from, to), Line, Column);
}