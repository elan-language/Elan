using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record IfStatementNode : IAstNode {
    public IfStatementNode(ImmutableArray<IAstNode> Expressions, ImmutableArray<IAstNode> StatementBlocks) {
        this.Expressions = Expressions;
        this.StatementBlocks = StatementBlocks;
    }

    public ImmutableArray<IAstNode> Expressions { get; init; }
    public ImmutableArray<IAstNode> StatementBlocks { get; init; }
    public IEnumerable<IAstNode> Children => Expressions.Union(StatementBlocks);

    public void Deconstruct(out ImmutableArray<IAstNode> Expressions, out ImmutableArray<IAstNode> StatementBlocks) {
        Expressions = this.Expressions;
        StatementBlocks = this.StatementBlocks;
    }
}