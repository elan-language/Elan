﻿using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record ReturnExpressionNode(IAstNode Expression) : IAstNode, ICanWrapExpression {
    public IEnumerable<IAstNode> Children => new[] { Expression };

    public IAstNode Replace(IAstNode from, IAstNode to) => new ReturnExpressionNode(to);
}