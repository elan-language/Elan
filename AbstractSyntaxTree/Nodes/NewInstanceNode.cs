using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record NewInstanceNode(IAstNode Type, ImmutableArray<IAstNode> Arguments, ImmutableArray<IAstNode> Init) : IAstNode {
    public IEnumerable<IAstNode> Children => Arguments.Prepend(Type).Concat(Init);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ => this with { Arguments = Arguments.SafeReplace(from, to), Init = Init.SafeReplace(from, to) }
        };
    }
}