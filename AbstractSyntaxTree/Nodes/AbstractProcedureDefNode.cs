﻿using AbstractSyntaxTree.Roles;

namespace AbstractSyntaxTree.Nodes;

public record AbstractProcedureDefNode(IAstNode Signature, int Line, int Column) : IAstNode, IHasScope {
    public IEnumerable<IAstNode> Children => new[] { Signature };

    public IAstNode Replace(IAstNode from, IAstNode to) => new AbstractProcedureDefNode(to, 0, 0);

    public string Name => ((INamedAstNode)Signature).Name;
}