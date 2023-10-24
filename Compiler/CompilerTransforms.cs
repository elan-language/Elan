using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;

namespace Compiler;

public static class CompilerTransforms {
    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => currentScope.Resolve(mcn.Name) switch {
                SystemCallSymbol => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                ProcedureSymbol => new ProcedureCallNode(mcn),
                FunctionSymbol => new FunctionCallNode(mcn),
                _ => null
            },
            _ => null
        };
}