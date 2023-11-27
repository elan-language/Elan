using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record FuncTypeNode(ImmutableArray<IAstNode> Types, IAstNode ReturnType) : IAstNode {
    public IEnumerable<IAstNode> Children => Types.Append(ReturnType);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == ReturnType => this with { ReturnType = to },
            _ => this with { Types = Types.SafeReplace(from, to) }
        };
    }
}