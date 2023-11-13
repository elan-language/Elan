using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using ValueType = AbstractSyntaxTree.ValueType;

namespace Compiler;

public static class CompilerTransforms {

    private static ISymbolType? EnsureResolved(ISymbolType symbolType, IScope currentScope) {
        if (symbolType is PendingResolveSymbol rr) {
            return currentScope.Resolve(rr.Name) switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ClassSymbol cs => new ClassSymbolType(cs.Name),
                FunctionSymbol fs => EnsureResolved(fs.ReturnType, currentScope),
                _ => throw new NotImplementedException()
            };
        }

        return symbolType;
    }


    private static ISymbolType? GetExpressionType(IAstNode expression, IScope currentScope) {
        return expression switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                _ => null
            },
            _ => null
        };
    }

    private static IAstNode? TransformClassMethods(MethodCallNode mcn, IAstNode[] nodes, IScope currentScope) {
        var id = mcn.Qualifier is IdentifierNode idn ? idn.Id : null;

        if (id != null) {
            var varSymbol = currentScope.Resolve(id);
            var type = varSymbol is VariableSymbol vs ? EnsureResolved(vs.ReturnType, currentScope) : null;

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
            VariableSymbol { ReturnType: ClassSymbolType } vs => GetNode(mcn, currentScope, (ClassSymbolType)vs.ReturnType),
            VariableSymbol { ReturnType: FloatSymbolType } => new ValueTypeNode(ValueType.Float),
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
            MethodCallNode mcn => ResolveMethodCall(currentScope, mcn) switch {
                (SystemCallSymbol, _) => new SystemCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                (ProcedureSymbol, false) => new ProcedureCallNode(mcn),
                (ProcedureSymbol, true) => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (FunctionSymbol fs, false) => new FunctionCallNode(mcn, NameSpaceToNode(fs.NameSpace)),
                (FunctionSymbol, true) => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => TransformClassMethods(mcn, nodes, currentScope)
            },
            _ => null
        };

    private static IAstNode MapSymbolToTypeNode(ISymbolType? type) {
        return type switch {
            ClassSymbolType cst => new TypeNode(new IdentifierNode(cst.Name)),
            IntSymbolType => new ValueTypeNode(ValueType.Int),
            FloatSymbolType => new ValueTypeNode(ValueType.Float),
            CharSymbolType => new ValueTypeNode(ValueType.Char),
            StringSymbolType => new ValueTypeNode(ValueType.String),
            BoolSymbolType => new ValueTypeNode(ValueType.Bool),
            _ => throw new NotImplementedException()
        };
    }

    private static IAstNode TypeIdentifier(IdentifierNode node, IScope currentScope) {
        var type = GetExpressionType(node, currentScope);
        var typeNode = MapSymbolToTypeNode(type);

        return new IdentifierWithTypeNode(node.Id, typeNode);
    }

    public static IAstNode? TransformLiteralListNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            LiteralListNode lln when lln.ItemNodes.First() is IdentifierNode idn and not IdentifierWithTypeNode => lln.Replace(idn, TypeIdentifier(idn, currentScope)),
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

    private static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        _ => null
    };

    private static (ISymbol?, bool) ResolveMethodCall(IScope currentScope, MethodCallNode mcn) {
        var isGlobal = mcn.Qualifier is GlobalNode;
        var qualifiedId = GetId(mcn.Qualifier);

        if (qualifiedId is not null) {
            var symbol = currentScope.Resolve(qualifiedId);

            if (symbol is VariableSymbol vs &&  EnsureResolved(vs.ReturnType, currentScope) is ClassSymbolType ) {
                return (null, false);
            }
            if (symbol is VariableSymbol or null) {
                return (GetGlobalScope(currentScope).Resolve(mcn.Name), isGlobal);
            }
            
        }

        var scope = isGlobal ? GetGlobalScope(currentScope) : currentScope;
        return (scope.Resolve(mcn.Name), isGlobal);
    }
}