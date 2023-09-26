using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record DataStructureTypeNode(DataStructure Type, ImmutableArray<IAstNode> GenericTypes) : IAstNode {
    public IEnumerable<IAstNode> Children => GenericTypes;
}