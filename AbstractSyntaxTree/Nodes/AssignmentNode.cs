﻿namespace AbstractSyntaxTree.Nodes;

public record AssignmentNode : IAstNode {
    public AssignmentNode(IAstNode Id, IAstNode Expression) {
        this.Id = Id;
        this.Expression = Expression;
    }
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };
    public IAstNode Id { get; init; }
    public IAstNode Expression { get; init; }
    public void Deconstruct(out IAstNode Id, out IAstNode Expression) {
        Id = this.Id;
        Expression = this.Expression;
    }
}