using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record NewInstanceNode(IAstNode Type, ImmutableArray<IAstNode> Arguments, ImmutableArray<IAstNode> Init) : IAstNode {
    public IEnumerable<IAstNode> Children => Arguments.Prepend(Type).Concat(Init);
}