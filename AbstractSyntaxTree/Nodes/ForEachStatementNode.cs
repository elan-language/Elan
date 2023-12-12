using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ForEachStatementNode(IAstNode Parameter, IAstNode StatementBlock) : IAstNode, ICanWrapExpression, IHasScope {

    public string Name { get; } = Helpers.UniqueScopeName;

    public IEnumerable<IAstNode> Children => new[] { Parameter, StatementBlock };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Parameter => this with { Parameter = to },
            _ when from == StatementBlock => this with { StatementBlock = to },
            _ => throw new NotImplementedException()
        };
    }
}