using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record IfStatementNode(ImmutableArray<IAstNode> Expressions, ImmutableArray<IAstNode> StatementBlocks) : IAstNode {
    public IEnumerable<IAstNode> Children => Expressions.Concat(StatementBlocks);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return new IfStatementNode(Expressions.SafeReplace(from, to), StatementBlocks.SafeReplace(from, to));
    }
}