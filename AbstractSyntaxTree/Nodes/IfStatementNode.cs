using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record IfStatementNode(ImmutableArray<IAstNode> Expressions, ImmutableArray<IAstNode> StatementBlocks) : IAstNode {
    public IEnumerable<IAstNode> Children => Expressions.Union(StatementBlocks);
}