using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerRules {
    public static string? ExpressionMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn && currentScope.Resolve(mcn.Name) is FunctionSymbol) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode or MethodCallNode)) {
                return "Cannot have unassigned expression";
            }
        }

        return null;
    }

    public static string? SystemCallMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn && currentScope.Resolve(mcn.Name) is SystemCallSymbol scs && scs.ReturnType != VoidSymbolType.Instance) {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (otherNodes.Any(n => n is MethodCallNode)) {
                return "Cannot have system call in expression";
            }

            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode)) {
                return "Cannot have unassigned system call";
            }
        }

        return null;
    }

    private static bool Match(IAstNode n1, IAstNode n2) => n1 is IdentifierNode idn1 && n2 is IdentifierNode idn2 && idn1.Id == idn2.Id;

    public static string? CannotMutateControlVariableRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is AssignmentNode mcn) {
            var otherNodes = nodes.SkipLast(1).ToArray();

            foreach (var forNode in otherNodes.OfType<ForStatementNode>()) {
                if (Match(forNode.Id, mcn.Id)) {
                    return "May not mutate control variable";
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
}