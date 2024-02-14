using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record IfStatementNode(ImmutableArray<IAstNode> Expressions, ImmutableArray<IAstNode> StatementBlocks, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => Expressions.Concat(StatementBlocks);

    public IAstNode Replace(IAstNode from, IAstNode to) => new IfStatementNode(Expressions.SafeReplace(from, to), StatementBlocks.SafeReplace(from, to), Line, Column);
}