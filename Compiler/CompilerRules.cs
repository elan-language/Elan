using System.Runtime.CompilerServices;
using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerRules {
    public static string? ExpressionMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is FunctionCallNode mcn) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode or ProcedureCallNode or FunctionCallNode or SystemCallNode)) {
                return "Cannot have unassigned expression";
            }
        }

        return null;
    }

    public static string? SystemCallMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is SystemCallNode mcn && currentScope.Resolve(mcn.Name) is SystemCallSymbol scs && scs.ReturnType != VoidSymbolType.Instance) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (otherNodes.Any(n => n is SystemCallNode or FunctionCallNode or ProcedureCallNode)) {
                return "Cannot have system call in expression";
            }

            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode)) {
                return "Cannot have unassigned system call";
            }
        }

        return null;
    }

    private static bool Match(IAstNode n1, IAstNode n2) => n1 is IdentifierNode idn1 && n2 is IdentifierNode idn2 && idn1.Id == idn2.Id;

    private static IEnumerable<IAstNode> Expand(this IEnumerable<IAstNode> nodes) {
        return nodes.SelectMany(n => n is StatementBlockNode ? n.Children : new[] { n });
    }


    public static string? CannotMutateControlVariableRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is AssignmentNode mcn) {
            var otherNodes = nodes.SkipLast(1).ToArray();

            foreach (var forNode in otherNodes.OfType<ForStatementNode>()) {
                if (Match(forNode.Id, mcn.Id)) {
                    return "May not mutate control variable";
                }
            }

            foreach (var forInNode in otherNodes.OfType<ForInStatementNode>()) {
                if (forInNode.Expression is IdentifierNode idn) {
                    if (Match(idn, mcn.Id)) {
                        return "May not mutate control variable";
                    }
                }
            }
        }

        return null;
    }

    public static string? ArrayInitialization(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is NewInstanceNode { Type : DataStructureTypeNode { Type: DataStructure.Array } } nin) {
            if (nin.Arguments.Length is 0 && nin.Init.Length is 0) {
                return "Array must have size or initializer";
            }

            if (nin.Arguments.Length > 0 && nin.Init.Length > 0) {
                return "Array cannot have size and initializer";
            }
        }

        return null;
    }

    public static string? FunctionConstraintsRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is SystemCallNode or ProcedureCallNode) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (otherNodes.Any(n => n is FunctionDefNode)) {
                return "Cannot have system call in function";
            }
        }

        if (leafNode is AssignmentNode an) {
            var otherNodes = nodes.SkipLast(1).Expand().ToArray();
            if (otherNodes.Any(n => n is FunctionDefNode)) {

                var varNodes = otherNodes.OfType<VarDefNode>();

                if (!varNodes.Any(vn => Match(vn.Id, an.Id))) {
                    return "Cannot modify param in function";
                }
            }
        }

        return null;
    }



    public static string? MethodCallsShouldBeResolvedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn) {
            return "Unresolved method call";
        }

        return null;
    }
}