using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record IfExpressionNode(ImmutableArray<IAstNode> Expressions, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => Expressions;

    public IAstNode Replace(IAstNode from, IAstNode to) => new IfExpressionNode(Expressions.SafeReplace(from, to), Line, Column);
}