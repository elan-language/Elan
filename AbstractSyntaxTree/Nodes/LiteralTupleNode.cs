using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record LiteralTupleNode(ImmutableArray<IAstNode> ItemNodes, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
    public IAstNode Replace(IAstNode from, IAstNode to) => new LiteralTupleNode(ItemNodes.SafeReplace(from, to), Line, Column);
}