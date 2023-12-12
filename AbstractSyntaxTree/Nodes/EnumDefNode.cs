using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record EnumDefNode(IAstNode Type, ImmutableArray<IAstNode> Values) : IAstNode, INamedAstNode {
    public IEnumerable<IAstNode> Children => Values.Prepend(Type);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ => this with { Values = Values.SafeReplace(from, to) }
        };
    }

    public string Name => ((IdentifierNode)Type).Id;
}