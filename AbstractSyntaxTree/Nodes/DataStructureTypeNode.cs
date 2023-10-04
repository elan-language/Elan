﻿using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record DataStructureTypeNode(DataStructure Type, ImmutableArray<IAstNode> GenericTypes) : IAstNode {
    public IEnumerable<IAstNode> Children => GenericTypes;

    public IAstNode Replace(IAstNode from, IAstNode to) => this with { GenericTypes = GenericTypes.SafeReplace(from, to) };
}