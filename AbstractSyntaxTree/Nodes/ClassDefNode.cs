using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record ClassDefNode(IAstNode Type, IAstNode Constructor, ImmutableArray<IAstNode> Properties) : IAstNode {
    public IEnumerable<IAstNode> Children => Properties.Prepend(Constructor).Prepend(Type);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ when from == Constructor => this with { Constructor = to },
            _ => this with { Properties = Properties.SafeReplace(from, to) }
        };
    }
}