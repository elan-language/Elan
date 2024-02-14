using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record TupleTypeNode(ImmutableArray<IAstNode> Types, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => Types;

    public IAstNode Replace(IAstNode from, IAstNode to) => new TupleTypeNode(Types.SafeReplace(from, to), Line, Column);
}