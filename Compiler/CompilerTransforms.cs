using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

namespace Compiler;

public static class CompilerTransforms {
    private static ISymbolType? GetExpressionType(IAstNode expression, IScope currentScope) {
        return expression switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => vs.ReturnType,
                _ => null
            },
            _ => null
        };
    }

    private static IAstNode? TransformClassMethods(MethodCallNode mcn, IAstNode[] nodes, IScope currentScope) {
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

    private static IAstNode? NameSpaceToNode(NameSpace ns) => ns switch {
        NameSpace.Library => new FunctionsNode(),
        NameSpace.UserGlobal => new GlobalNode(),
        _ => null
    };

    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => Resolve(currentScope, mcn) switch {
                (SystemCallSymbol, _) => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                (ProcedureSymbol, false) => new ProcedureCallNode(mcn),
                (ProcedureSymbol, true) => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (FunctionSymbol fs, false) => new FunctionCallNode(mcn, NameSpaceToNode(fs.NameSpace)),
                (FunctionSymbol, true) => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => TransformClassMethods(mcn, nodes, currentScope)
            },
            _ => null
        };

    public static IAstNode? TransformIndexNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            IndexedExpressionNode ien when GetExpressionType(ien.Expression, currentScope) is TupleSymbolType => new ItemizedExpressionNode(ien.Expression, ien.Range),
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
        var qualifiedId = mcn is { Qualifier: IdentifierNode idn } ? idn.Id : null;

        if (qualifiedId is not null) {
            var symbol = GetGlobalScope(currentScope).Resolve(qualifiedId);

            if (symbol is null) {
                return (GetGlobalScope(currentScope).Resolve(mcn.Name), isGlobal);
            }
        }

        var scope = isGlobal ? GetGlobalScope(currentScope) : currentScope;
        return (scope.Resolve(mcn.Name), isGlobal);
    }
}