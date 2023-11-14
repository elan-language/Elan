using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ForInStatementNode(IAstNode Id, IAstNode Expression, IAstNode StatementBlock) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => new[] { Id, Expression, StatementBlock };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Expression => this with { Expression = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => throw new NotImplementedException()
        };
    }
}