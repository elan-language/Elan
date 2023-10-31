using System.Formats.Asn1;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerTransforms {

    private static IAstNode? TransformClassMethods(MethodCallNode mcn,   IAstNode[] nodes, IScope currentScope) {

        var id = mcn.Qualifier is IdentifierNode idn ? idn.Id : null;

        if (id != null) {
            var varSymbol = currentScope.Resolve(id);
            var type = varSymbol is VariableSymbol vs ? vs.ReturnType : throw new NotSupportedException();

            if (type is ClassSymbolType cst) {
                return GetNode(mcn, currentScope, cst);
            }
        }

        return null;
    }

    private static IAstNode? GetNode(MethodCallNode mcn, IScope currentScope, ClassSymbolType cst) {
        var rhs = currentScope.Resolve(cst.Name);

        return rhs switch {
            IScope scope => scope.Resolve(mcn.Name) switch {
                ProcedureSymbol => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                FunctionSymbol => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => null
            },
            VariableSymbol vs => GetNode(mcn, currentScope, (ClassSymbolType)vs.ReturnType),
            _ => null
        };
    }

    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => Resolve(currentScope, mcn) switch {
                (SystemCallSymbol, _) => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                (ProcedureSymbol, false) => new ProcedureCallNode(mcn),
                (ProcedureSymbol, true) => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (FunctionSymbol, false) => new FunctionCallNode(mcn),
                (FunctionSymbol, true) => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => TransformClassMethods(mcn, nodes, currentScope)
            },
            _ => null
        };

    private static IScope GetGlobalScope(IScope scope) =>
        scope is GlobalScope
            ? scope
            : scope.EnclosingScope is { } s
                ? GetGlobalScope(s)
                : scope;

    private static (ISymbol?, bool) Resolve(IScope currentScope, MethodCallNode mcn) {
        var isGlobal = mcn.Qualifier is GlobalNode;
        var scope = isGlobal ? GetGlobalScope(currentScope) : currentScope;
        return (scope.Resolve(mcn.Name), isGlobal);
    }
}