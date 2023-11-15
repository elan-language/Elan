using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodCallNode(IAstNode Id, IAstNode? Qualifier, ImmutableArray<IAstNode> Parameters) : IAstNode, ICallNode {
    
    public bool DotCalled { get; init; } = false;

    public string Name => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public IEnumerable<IAstNode> Children => Parameters.SafePrepend(Qualifier).Prepend(Id);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ when from == Qualifier => this with { Qualifier = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}