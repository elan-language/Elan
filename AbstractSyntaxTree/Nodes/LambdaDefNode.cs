﻿using System.Collections.Immutable;
using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record LambdaDefNode(ImmutableArray<IAstNode> Arguments, IAstNode Expression) : IAstNode, IHasScope {

    public string Name { get; } = Helpers.UniqueLambdaName;

    public IEnumerable<IAstNode> Children => Arguments.Append(Expression);

    public IAstNode Replace(IAstNode from, IAstNode to) {
        return from switch {
            _ when from == Expression => this with { Expression = to },
            _ => this with { Arguments = Arguments.SafeReplace(from, to) }
        };
    }
}