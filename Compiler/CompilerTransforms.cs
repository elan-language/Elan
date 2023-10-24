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

                var classSymbol = currentScope.Resolve(cst.Name) as IScope;

                return classSymbol?.Resolve(mcn.Name) switch {
                    ProcedureSymbol => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                    FunctionSymbol => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                    _ => null
                };
            }
        }

        return null;
    }



    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => currentScope.Resolve(mcn.Name) switch {
                SystemCallSymbol => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                ProcedureSymbol => new ProcedureCallNode(mcn),
                FunctionSymbol => new FunctionCallNode(mcn),
                _ => TransformClassMethods(mcn, nodes, currentScope)
            },
            _ => null
        };
}