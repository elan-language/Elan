using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record WithNode(IAstNode Expression, ImmutableArray<IAstNode> AssignmentNodes, int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => AssignmentNodes.Prepend(Expression);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Expression => this with { Expression = to },
            _ => this with { AssignmentNodes = AssignmentNodes.SafeReplace(from, to) }
        };
    }
}