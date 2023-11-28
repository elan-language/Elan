using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record LambdaDefNode(ImmutableArray<IAstNode> Arguments, IAstNode Expression) : IAstNode {
    public IEnumerable<IAstNode> Children => Arguments.Append(Expression);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Expression => this with { Expression = to },
            _ => this with { Arguments = Arguments.SafeReplace(from, to) }
        };
    }
}