using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record DeconstructionNode(ImmutableArray<IAstNode> ItemNodes, ImmutableArray<bool> IsNew, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => ItemNodes;
    public IAstNode Replace(IAstNode from, IAstNode to) => new LiteralListNode(ItemNodes.SafeReplace(from, to), Line, Column);
}