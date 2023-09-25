﻿namespace AbstractSyntaxTree.Nodes;

public record ConstantDefNode : IAstNode {
    public ConstantDefNode(IAstNode Id, IAstNode Expression) {
        this.Id = Id;
        this.Expression = Expression;
    }

    public IAstNode Id { get; init; }
    public IAstNode Expression { get; init; }
    public IEnumerable<IAstNode> Children => new[] { Id, Expression };

    public void Deconstruct(out IAstNode Id, out IAstNode Expression) {
        Id = this.Id;
        Expression = this.Expression;
    }
}