using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record MethodSignatureNode(IAstNode Id, ImmutableArray<IAstNode> Parameters, IAstNode? ReturnType, int Line, int Column) : INamedAstNode {
    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id).SafeAppend(ReturnType);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == ReturnType => this with { ReturnType = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }

    public string Name => ((IdentifierNode)Id).Id;
}