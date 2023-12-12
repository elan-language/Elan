using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ClassDefNode(IAstNode Type, ImmutableArray<IAstNode> Inherits, IAstNode Constructor, ImmutableArray<IAstNode> Properties, ImmutableArray<IAstNode> Methods, bool Immutable) : IAstNode, INamedAstNode, IHasScope {
    public bool HasDefaultConstructor => Constructor is ConstructorNode cn && !cn.Parameters.Any();

    public IEnumerable<IAstNode> Children => Inherits.Prepend(Type).Append(Constructor).Concat(Properties).Concat(Methods);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ when from == Constructor => this with { Constructor = to },
            _ => this with {
                Inherits = Inherits.SafeReplace(from, to),
                Properties = Properties.SafeReplace(from, to),
                Methods = Methods.SafeReplace(from, to)
            }
        };
    }

    public string Name => ((IdentifierNode)Type).Id;
}