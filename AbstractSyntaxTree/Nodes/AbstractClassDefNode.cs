using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AbstractClassDefNode(IAstNode Type, ImmutableArray<IAstNode> Inherits, ImmutableArray<IAstNode> Properties, ImmutableArray<IAstNode> Methods) : IAstNode, INamedAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => Inherits.Prepend(Type).Concat(Properties).Concat(Methods);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ => this with {
                Inherits = Inherits.SafeReplace(from, to),
                Properties = Properties.SafeReplace(from, to),
                Methods = Methods.SafeReplace(from, to)
            }
        };
    }

    public string Name => ((IdentifierNode)Type).Id;
}