﻿using System.Collections.Immutable;

namespace AbstractSyntaxTree.Nodes;

public record ForStatementNode(IAstNode Id, ImmutableArray<IAstNode> Expressions, IAstNode? Step, bool Neg, IAstNode StatementBlock) : IAstNode {
    public IEnumerable<IAstNode> Children => Expressions.Prepend(Id).Append(StatementBlock);
}