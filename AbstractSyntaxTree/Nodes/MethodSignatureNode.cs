using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodSignatureNode(IAstNode Id, ImmutableArray<IAstNode> Parameters, IAstNode? ReturnType = null) : IAstNode {
    public IEnumerable<IAstNode> Children => ReturnType is null ? Parameters.Prepend(Id) : Parameters.Prepend(Id).Append(ReturnType);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == ReturnType => this with { ReturnType = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}