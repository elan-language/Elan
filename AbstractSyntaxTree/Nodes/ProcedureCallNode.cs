using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ProcedureCallNode(IAstNode Id, IAstNode? Qualifier, ImmutableArray<IAstNode> Parameters, IAstNode? CalledOn) : IAstNode, ICanWrapExpression, ICallNode {
    private string UniqueId { get; } = Helpers.UniqueID;

    public IEnumerable<IAstNode> Children => Parameters.SafePrepend(Qualifier).Prepend(Id).SafeAppend(CalledOn);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Qualifier => this with { Qualifier = to },
            _ when from == CalledOn => this with { CalledOn = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }

    public string Name => Id is IdentifierNode idn ? $"_{idn.Id}{UniqueId}" : throw new NotImplementedException();

    public string MethodName => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();
}