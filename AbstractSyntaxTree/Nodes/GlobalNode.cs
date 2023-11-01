﻿namespace AbstractSyntaxTree.Nodes;

public record GlobalNode : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this;
}