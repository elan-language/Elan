using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;

namespace Compiler;

public static class CompilerRules {
    public static string? NoProcedureInFunctionRule(IAstNode[] nodes, IScope currentScope) {
        //var leafNode = nodes.Last();
        //if (leafNode is MethodCallNode msn && msn.Id is IdentifierNode idn)
        //{
        //    if (currentScope.Resolve(idn.Id) is MethodSymbol { MethodType: MethodType.Procedure or MethodType.SystemCall })
        //    {
        //        if (nodes.Any(n => n is FunctionDefinitionNode))
        //        {
        //            return "Cannot have procedure/system call in function";
        //        }
        //    }
        //}
        return null;
    }

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
}