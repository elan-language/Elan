using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodCallNode : IAstNode {
    public MethodCallNode(MethodCallNode node, IAstNode parameter) : this(node.Id, node.Parameters.Prepend(parameter).ToImmutableArray()) { }
    public MethodCallNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) {
        this.Id = Id;
        this.Parameters = Parameters;
    }

    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);
    public IAstNode Id { get; init; }

    public string Name => Id is IdentifierNode idn ? idn.Id : throw new NotImplementedException();

    public ImmutableArray<IAstNode> Parameters { get; init; }
    public void Deconstruct(out IAstNode Id, out ImmutableArray<IAstNode> Parameters) {
        Id = this.Id;
        Parameters = this.Parameters;
    }
}