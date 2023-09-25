using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerRules {
   

    public static string? ExpressionMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn && currentScope.Resolve(mcn.Name) is FunctionSymbol)
        {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode))
            {
                return "Cannot have unassigned expression";
            }
        }
        return null;
    }

    public static string? SystemCallMustBeAssignedRule(IAstNode[] nodes, IScope currentScope) {
        var leafNode = nodes.Last();
        if (leafNode is MethodCallNode mcn && currentScope.Resolve(mcn.Name) is SystemCallSymbol scs && scs.ReturnType != VoidSymbolType.Instance)
        {
            var otherNodes = nodes.SkipLast(1).ToArray();
            if (!otherNodes.Any(n => n is AssignmentNode or VarDefNode))
            {
                return "Cannot have unassigned system call";
            }
        }
        return null;
    }
}