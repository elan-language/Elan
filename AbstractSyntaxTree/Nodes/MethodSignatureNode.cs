using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodSignatureNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) : IAstNode {
    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);
}