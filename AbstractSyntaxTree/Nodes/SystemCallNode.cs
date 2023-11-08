using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record SystemCallNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) : IAstNode, ICanWrapExpression {
    public bool DotCalled { get; init; } = false;

    public string Name => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}