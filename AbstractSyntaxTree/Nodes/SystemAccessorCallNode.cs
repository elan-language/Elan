using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record SystemAccessorCallNode(IAstNode Id, IAstNode? Qualifier, ImmutableArray<IAstNode> Parameters, int Line, int Column) : IAstNode, ICanWrapExpression {
    public string Name => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public string MethodName => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Qualifier => this with { Qualifier = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}