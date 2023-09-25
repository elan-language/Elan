using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record StatementBlockNode : IAstNode {
    public StatementBlockNode(ImmutableArray<IAstNode> Statements) => this.Statements = Statements;

    public ImmutableArray<IAstNode> Statements { get; init; }
    public IEnumerable<IAstNode> Children => Statements;

    public void Deconstruct(out ImmutableArray<IAstNode> Statements) {
        Statements = this.Statements;
    }
}