using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record FileNode(ImmutableArray<IAstNode> GlobalNodes, IAstNode MainNode) : IAstNode {
    public IEnumerable<IAstNode> Children => new[] { MainNode };

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == MainNode => this with { MainNode = to },
            _ => this with { GlobalNodes = GlobalNodes.SafeReplace(from, to) }
        };
    }
}