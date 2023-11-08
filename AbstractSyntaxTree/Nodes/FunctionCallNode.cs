using System.Collections.Immutable;
using System.Reflection.Metadata;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record FunctionCallNode(IAstNode Id, IAstNode? Qualifier, ImmutableArray<IAstNode> Parameters) : IAstNode, ICanWrapExpression {
    public FunctionCallNode(MethodCallNode node, IAstNode? nsNode) : this(node.Id, nsNode, node.Parameters.SafePrepend(node.Qualifier).ToImmutableArray()) { }
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