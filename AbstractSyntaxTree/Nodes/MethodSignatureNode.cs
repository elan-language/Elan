﻿using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record MethodSignatureNode(IAstNode Id, ImmutableArray<IAstNode> Parameters) : IAstNode {
    public IEnumerable<IAstNode> Children => Parameters.Prepend(Id);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Id => this with { Id = to },
            _ => this with { Parameters = Parameters.SafeReplace(from, to) }
        };
    }
}