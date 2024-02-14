﻿namespace AbstractSyntaxTree.Nodes;

public record GlobalPrefixNode(int Line, int Column) : IAstNode {
    public IEnumerable<IAstNode> Children => Array.Empty<IAstNode>();
    public IAstNode Replace(IAstNode from, IAstNode to) => this;
}