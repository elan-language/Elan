using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MainNode(ImmutableArray<IAstNode> StatementNodes) : IAstNode {
    public IEnumerable<IAstNode> Children => StatementNodes;
}