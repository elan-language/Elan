using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record StatementBlockNode : IAstNode  {
    public StatementBlockNode(ImmutableArray<IAstNode> Statements) {
        this.Statements = Statements;
    }
    public IEnumerable<IAstNode> Children => Statements;
    public ImmutableArray<IAstNode> Statements { get; init; }
    public void Deconstruct(out ImmutableArray<IAstNode> Statements) {
        Statements = this.Statements;
    }
}