using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record ForStatementNode(IAstNode Id, ImmutableArray<IAstNode> Expressions, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => Expressions.Prepend(Id).Append(StatementBlock);
}