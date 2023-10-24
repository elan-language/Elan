using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;

namespace Compiler;

public static class CompilerTransforms {
    public static IAstNode? TransformSystemCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => currentScope.Resolve(mcn.Name) switch {
                SystemCallSymbol => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                ProcedureSymbol => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                FunctionSymbol => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => null
            },
            _ => null
        };
}