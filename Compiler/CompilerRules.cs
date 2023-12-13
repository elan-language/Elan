﻿using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree.Roles;
using SymbolTable;
using SymbolTable.Symbols;

namespace Compiler;

public static class CompilerRules {
    private static bool AnyOtherNodeIs(this IAstNode[] nodes, Func<IAstNode, bool> check) => nodes.SkipLast(1).ToArray().Any(check);

    public static string? ExpressionMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is FunctionCallNode) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is ICanWrapExpression)) {
                return $"Result generated by expression is not being used : {leafNode}";
            }
        }

        return null;
    }

    public static string? CannotAccessSystemInAFunction(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is SystemAccessorCallNode scn && currentScope.Resolve(scn.MethodName) is SystemAccessorSymbol) {
            if (nodes.AnyOtherNodeIs(n => n is FunctionDefNode)) {
                return $"Cannot access system within a function : {leafNode}";
            }
        }

        return null;
    }

    public static string? CannotUseInputInAFunction(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is InputNode) {
            if (nodes.AnyOtherNodeIs(n => n is FunctionDefNode)) {
                return $"Cannot use 'input' within a function : {leafNode}";
            }
        }

        return null;
    }

    private static bool Match(IAstNode n1, IAstNode n2) {
        return n2 switch {
            IdentifierNode idn2 when n1 is IdentifierNode idn1 => idn1.Id == idn2.Id,
            IndexedExpressionNode ien when n1 is IdentifierNode idn1 => Match(idn1, ien.Expression) || ien.Expression.Children.Any(c => Match(idn1, c)),
            ParameterCallNode pn when n1 is IdentifierNode idn1 && pn.Expression is IdentifierNode idn2 => idn1.Id == idn2.Id,
            DeconstructionNode dn => dn.ItemNodes.Any(n => Match(n1, n)),
            _ => false
        };
    }

    private static IEnumerable<IAstNode> Expand(this IEnumerable<IAstNode> nodes) {
        return nodes.SelectMany(n => n is StatementBlockNode ? n.Children : new[] { n });
    }

    public static string? NoMutableConstantsRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (nodes.AnyOtherNodeIs(n => n is ConstantDefNode)) {
            return leafNode switch {
                TypeNode tn => currentScope.Resolve(tn.Name) is ClassSymbol { ClassType: ClassSymbolTypeType.Mutable } ? $"A class cannot be constant unless it is immutable {leafNode}" : null,
                DataStructureTypeNode => $"An array may not be a constant : {leafNode}",
                _ => null
            };
        }

        return null;
    }

    public static string? CannotMutateControlVariableRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        switch (leafNode) {
            case AssignmentNode an: {
                var otherNodes = nodes.SkipLast(1).ToArray();

                if (otherNodes.OfType<ForStatementNode>().Any(n => Match(n.Id, an.Id))) {
                    return $"Cannot modify control variable : {leafNode}";
                }

                if (otherNodes.OfType<EachStatementNode>().Any(n => n.Parameter is EachParameterNode epn && Match(epn.Expression, an.Id))) {
                    return $"Cannot modify control variable : {leafNode}";
                }

                break;
            }
            case ProcedureCallNode pcn: {
                var otherNodes = nodes.SkipLast(1).ToArray();
                var parameters = pcn.Parameters;

                foreach (var pp in parameters) {
                    if (otherNodes.OfType<ForStatementNode>().Any(n => Match(n.Id, pp))) {
                        return $"Cannot pass control variable into a procedure (consider declaring a new variable copying the value) : {leafNode}";
                    }

                    if (otherNodes.OfType<EachStatementNode>().Any(n => n.Parameter is EachParameterNode epn && Match(epn.Id, pp))) {
                        return $"Cannot pass control variable into a procedure (consider declaring a new variable copying the value) : {leafNode}";
                    }
                }

                break;
            }
        }

        return null;
    }

    public static string? ArrayInitialization(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        return leafNode is NewInstanceNode { Type : DataStructureTypeNode { Type: DataStructure.Array }, Arguments.Length: 0 }
            ? $"Array must have size specified in round brackets : {leafNode}"
            : null;
    }

    private static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        _ => null
    };

    public static string? FunctionConstraintsRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        var otherNodes = nodes.SkipLast(1).ToArray();

        string? CheckParams(AssignmentNode an, IAstNode[] expandedOtherNodes) {
            if (expandedOtherNodes.Any(n => n is FunctionDefNode)) {
                var varNodes = expandedOtherNodes.OfType<VarDefNode>();
                if (!varNodes.Any(vn => Match(vn.Id, an.Id))) {
                    return $"Cannot modify param in function : {leafNode}";
                }
            }

            if (expandedOtherNodes.Any(n => n is ProcedureDefNode)) {
                var pdn = expandedOtherNodes.OfType<ProcedureDefNode>().Last();

                var nonRefParameters = pdn.Signature.Children.OfType<ParameterNode>().Where(p => !p.IsRef);

                if (nonRefParameters.Any(vn => Match(vn.Id, an.Id))) {
                    return $"Parameter {GetId(an.Id)} may not be updated : {leafNode}";
                }
            }

            return null;
        }

        return leafNode switch {
            SystemAccessorCallNode => otherNodes.Any(n => n is FunctionDefNode) ? $"Cannot have system call in function : {leafNode}" : null,
            ProcedureCallNode => otherNodes.Any(n => n is FunctionDefNode) ? $"Cannot call a procedure within a function : {leafNode}" : null,
            ThrowNode => otherNodes.Any(n => n is FunctionDefNode) ? $"Cannot throw exception in function : {leafNode}" : null,
            PrintNode => otherNodes.Any(n => n is FunctionDefNode) ? $"Cannot print in function : {leafNode}" : null,
            AssignmentNode an => CheckParams(an, otherNodes.Expand().ToArray()),
            _ => null
        };
    }

    public static string? ConstructorConstraintsRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();

        if (leafNode is AssignmentNode an) {
            var otherNodes = nodes.SkipLast(1).Expand().ToArray();
            if (otherNodes.Any(n => n is ConstructorNode)) {
                var paramNodes = otherNodes.OfType<ConstructorNode>().Single().Parameters.OfType<ParameterNode>();

                if (paramNodes.Any(n => Match(n.Id, an.Id))) {
                    return $"Cannot modify param in constructor : {leafNode}";
                }
            }
        }

        return null;
    }

    public static string? ClassMustHaveAsString(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();

        if (leafNode is ClassDefNode cdn) {
            var children = cdn.Methods.OfType<FunctionDefNode>();
            if (!children.Any(n => n is { Signature: MethodSignatureNode { Id : IdentifierNode { Id : "asString" } } })) {
                return $"Class must have asString method: {leafNode}";
            }
        }

        return null;
    }

    public static string? ClassCannotInheritConcreteClass(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();

        var typeNodes = leafNode switch {
            ClassDefNode cdn => cdn.Inherits.OfType<TypeNode>(),
            AbstractClassDefNode ac => ac.Inherits.OfType<TypeNode>(),
            _ => Array.Empty<TypeNode>()
        };

        var inherits = typeNodes.Select(tn => GetId(tn.TypeName));
        var classSymbols = inherits.Select(currentScope.Resolve).OfType<ClassSymbol>();
        return classSymbols.Any(s => s.ClassType is not ClassSymbolTypeType.Abstract) ? "Cannot inherit from concrete class" : null;
    }
}