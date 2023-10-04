using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record StatementBlockNode(ImmutableArray<IAstNode> Statements) : IAstNode {
    public IEnumerable<IAstNode> Children => Statements;
    public IAstNode Replace(IAstNode from, IAstNode to) => new StatementBlockNode(Statements.SafeReplace(from, to));
}