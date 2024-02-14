using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ForStatementNode(IAstNode Id, ImmutableArray<IAstNode> Expressions, IAstNode? Step, bool Neg, IAstNode StatementBlock, int Line, int Column) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => Expressions.Prepend(Id).Append(StatementBlock);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Step => this with { Step = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => this with { Expressions = Expressions.SafeReplace(from, to) }
        };
    }
}