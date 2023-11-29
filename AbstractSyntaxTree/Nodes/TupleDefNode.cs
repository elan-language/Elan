using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record TupleDefNode(ImmutableArray<IAstNode> Expressions) : IAstNode {
    public IEnumerable<IAstNode> Children => Expressions;

    public IAstNode Replace(IAstNode from, IAstNode to) => new TupleDefNode(Expressions.SafeReplace(from, to));
}