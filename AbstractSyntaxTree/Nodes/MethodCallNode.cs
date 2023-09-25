using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodCallNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) : IAstNode {
    public MethodCallNode(MethodCallNode node, IAstNode parameter) : this(node.Id, node.Parameters.Prepend(parameter).ToImmutableArray()) { }

    public string Name => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);
}