using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record AbstractClassDefNode(IAstNode Type, ImmutableArray<IAstNode> Properties, ImmutableArray<IAstNode> Methods) : IAstNode {
    public IEnumerable<IAstNode> Children => Properties.Prepend(Type).Concat(Methods);


    public string Name => ((IdentifierNode)Type).Id;

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Type => this with { Type = to },
            _ => this with { Properties = Properties.SafeReplace(from, to), Methods = Methods.SafeReplace(from, to)}

        };
    }
}