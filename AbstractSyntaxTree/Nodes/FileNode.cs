using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record FileNode(ImmutableArray<IAstNode> GlobalNodes, MainNode MainNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { MainNode };
}