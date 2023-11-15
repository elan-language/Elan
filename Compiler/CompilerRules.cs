﻿using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree.Roles;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerRules {
    public static string? ExpressionMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is FunctionCallNode mcn) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is ICanWrapExpression)) {
                return $"Result generated by expression is not being used : {leafNode}";
            }
        }

        return null;
    }

    public static string? SystemCallMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is SystemAccessorCallNode scn && currentScope.Resolve(scn.Name) is SystemAccessorSymbol scs && scs.ReturnType != VoidSymbolType.Instance) {
            var otherNodes = nodes.SkipLast(1).ToArray();

            if (scn.DotCalled) {
                return $"Cannot use a system call in an expression {leafNode}";
            }

            if (otherNodes.Any(n => n is FunctionDefNode)) {
                return $"Cannot have system call in function : {leafNode}";
            }

            if (otherNodes.Any(n => n is  PrintNode)) {
                return $"Cannot use a print in an expression : {leafNode}";
            }
            
            if (!(otherNodes.Last() is VarDefNode or AssignmentNode)) {
                return $"Cannot use a system call in an expression : {leafNode}";
            }

            if (!otherNodes.Any(n => n is ICanWrapExpression)) {
                return $"System call generates a result that is neither assigned nor returned : {leafNode}";
            }
        }

        return null;
    }

    private static bool Match(IAstNode n1, IAstNode n2) {
        return n2 switch {
            IdentifierNode idn2 when n1 is IdentifierNode idn1 => idn1.Id == idn2.Id,
            IndexedExpressionNode ien when n1 is IdentifierNode idn1 => Match(idn1, ien.Expression) || ien.Expression.Children.Any(c => Match(idn1, c)),
            ParameterCallNode pn when n1 is IdentifierNode idn1 => idn1.Id == pn.Id,
            _ => false
        };
    }

    private static IEnumerable<IAstNode> Expand(this IEnumerable<IAstNode> nodes) {
        return nodes.SelectMany(n => n is StatementBlockNode ? n.Children : new[] { n });
    }

    public static string? CannotMutateTupleRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is ItemizedExpressionNode ien) {
            var parent = nodes.SkipLast(1).Last();

            if (parent is AssignmentNode an) {
                if (an.Id == ien) {
                    return $"Cannot modify an element within a tuple : {leafNode}";
                }
            }
        }

        return null;
    }

    public static string? NoMutableConstantsRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        var otherNodes = nodes.SkipLast(1).ToArray();

        if (otherNodes.Any(n => n is ConstantDefNode)) {
            if (leafNode is TypeNode tn) {
                var id = tn is { TypeName: IdentifierNode idn } ? idn.Id : "";

                var type = currentScope.Resolve(id);

                if (type is ClassSymbol { ClassType: ClassSymbolTypeType.Mutable }) {
                    return $"A class cannot be constant unless it is immutable {leafNode}";
                }
            }

            if (leafNode is DataStructureTypeNode) {
                return $"An array may not be a constant : {leafNode}";
            }
        }

        return null;
    }

    public static string? CannotMutateControlVariableRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is AssignmentNode an) {
            var otherNodes = nodes.SkipLast(1).ToArray();

            foreach (var forNode in otherNodes.OfType<ForStatementNode>()) {
                if (Match(forNode.Id, an.Id)) {
                    return $"Cannot modify control variable : {leafNode}";
                }
            }

            foreach (var forInNode in otherNodes.OfType<ForInStatementNode>()) {
                if (forInNode.Expression is IdentifierNode idn) {
                    if (Match(idn, an.Id)) {
                        return $"Cannot modify control variable : {leafNode}";
                    }
                }
            }
        }

        if (leafNode is ProcedureCallNode pcn) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            var parameters = pcn.Parameters;

            foreach (var pp in parameters) {
                foreach (var forNode in otherNodes.OfType<ForStatementNode>()) {
                    if (Match(forNode.Id, pp)) {
                        return $"Cannot pass control variable into a procedure (consider declaring a new variable copying the value) : {leafNode}";
                    }
                }

                foreach (var forInNode in otherNodes.OfType<ForInStatementNode>()) {
                    if (forInNode.Id is IdentifierNode idn) {
                        if (Match(idn, pp)) {
                            return $"Cannot pass control variable into a procedure (consider declaring a new variable copying the value) : {leafNode}";
                        }
                    }
                }
            }
        }

        return null;
    }

    public static string? ArrayInitialization(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is NewInstanceNode { Type : DataStructureTypeNode { Type: DataStructure.Array } } nin) {
            if (nin.Arguments.Length is 0) {
                return $"Array must have size specified in round brackets : {leafNode}";
            }
        }

        return null;
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

                if (paramNodes.Any(pn => Match(pn.Id, an.Id))) {
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
            AbstractClassDefNode acdn => acdn.Inherits.OfType<TypeNode>(),
            _ => Array.Empty<TypeNode>()
        };

        var inherits = typeNodes.Select(tn => ((IdentifierNode)tn.TypeName).Id);
        var classSymbols = inherits.Select(currentScope.Resolve).OfType<ClassSymbol>();
        return classSymbols.Any(s => s.ClassType is not ClassSymbolTypeType.Abstract) ? "Cannot inherit from concrete class" : null;
    }

    public static string? MethodCallsShouldBeResolvedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn) {
            return $"Calling unknown method : {leafNode}";
        }

        return null;
    }
}