using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record SwitchStatementNode(IAstNode Expression, ImmutableArray<IAstNode> Cases, IAstNode DefaultCase, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => Cases.Prepend(Expression).Append(DefaultCase);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Expression => this with { Expression = to },
            _ when from == DefaultCase => this with { DefaultCase = to },
            _ => this with { Cases = Cases.SafeReplace(from, to) }
        };
    }
}