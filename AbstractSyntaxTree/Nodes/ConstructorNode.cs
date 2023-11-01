using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record ConstructorNode(ImmutableArray<IAstNode> Parameters, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => Parameters.Append(StatementBlock);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}