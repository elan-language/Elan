using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using System.Reflection.Metadata;
using ValueType = AbstractSyntaxTree.ValueType;

namespace Compiler;

public static class CompilerTransforms {

    #region rules

    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => ResolveMethodCall(currentScope, mcn) switch {
                (SystemAccessorSymbol, _) => new SystemAccessorCallNode(mcn.Id, mcn.Parameters) { DotCalled = mcn.DotCalled },
                (ProcedureSymbol, false) => new ProcedureCallNode(mcn),
                (ProcedureSymbol, true) => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (FunctionSymbol fs, false) => new FunctionCallNode(mcn, NameSpaceToNode(fs.NameSpace)),
                (FunctionSymbol, true) => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => TransformClassMethods(mcn, nodes, currentScope)
            },
            _ => null
        };

    
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

    // must come after TransformMethodCallNodes
    public static IAstNode? TransformProcedureParameterNodes(IAstNode[] nodes, IScope currentScope) {

        static IAstNode TransformParameter(IAstNode p, int i, ProcedureSymbol scope) {
            // avoid failed with mismatched parameter counts
            if (i >= scope.ParameterNames.Length) {
                return new ParameterCallNode(p, false);
            }

            var matchingParm = scope.ParameterNames[i];

            var symbol = scope.Resolve(matchingParm) as ParameterSymbol ?? throw new NullReferenceException();
            return new ParameterCallNode(p, symbol.ByRef);
        }

        switch (nodes.Last()) {
            case ProcedureCallNode pcn:

                var procedureScope =  ResolveMethodCall(currentScope, pcn).Item1 as ProcedureSymbol ?? GetProcedure(pcn, currentScope) ?? throw new NullReferenceException();

                var parameterNodes = pcn.Parameters.Where(p => p is not ParameterCallNode).ToArray();

                if (parameterNodes.Any()) {
                    var transformedParameters = pcn.Parameters.Select((p, i) => TransformParameter(p, i, procedureScope));
                    return pcn with { Parameters = transformedParameters.ToImmutableArray() };
                }

                return null;
            default:
                return null;
        }
    }

    #endregion

    #region helpers

    private static IAstNode? TransformClassMethods(MethodCallNode mcn, IAstNode[] nodes, IScope currentScope) {
        var id = mcn.Qualifier is IdentifierNode idn ? idn.Id : null;

        if (id != null) {
            var varSymbol = currentScope.Resolve(id);
            var type = varSymbol switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ParameterSymbol ps => EnsureResolved(ps.ReturnType, currentScope),
                _ => null
            };

            if (type is ClassSymbolType cst) {
                return GetNode(mcn, currentScope, cst);
            }
        }

        return null;
    }

    private static ProcedureSymbol? GetProcedure(ICallNode mcn, IScope currentScope) {
        var id = mcn.Qualifier is IdentifierNode idn ? idn.Id : null;

        if (id != null) {
            var varSymbol = currentScope.Resolve(id);
            var type = varSymbol switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ParameterSymbol ps => EnsureResolved(ps.ReturnType, currentScope),
                _ => null
            };

            if (type is ClassSymbolType cst) {
                return (currentScope.Resolve(cst.Name) as ClassSymbol)?.Resolve(mcn.Name) as ProcedureSymbol;
            }
        }

        return null;
    }


    private static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        _ => null
    };

    private static (ISymbol?, bool) ResolveMethodCall(IScope currentScope, ICallNode mcn) {
        var isGlobal = mcn.Qualifier is GlobalNode;
        var qualifiedId = GetId(mcn.Qualifier);

        if (qualifiedId is not null) {
            var symbol = currentScope.Resolve(qualifiedId);

            if (symbol is VariableSymbol vs &&  EnsureResolved(vs.ReturnType, currentScope) is ClassSymbolType ) {
                return (null, false);
            }
            
            if (symbol is ParameterSymbol ps &&  EnsureResolved(ps.ReturnType, currentScope) is ClassSymbolType ) {
                return (null, false);
            }

            if (symbol is VariableSymbol or ParameterSymbol or null) {
                return (GetGlobalScope(currentScope).Resolve(mcn.Name), isGlobal);
            }
        }

        var scope = isGlobal ? GetGlobalScope(currentScope) : currentScope;
        return (scope.Resolve(mcn.Name), isGlobal);
    }

    private static IScope GetGlobalScope(IScope scope) =>
        scope is GlobalScope
            ? scope
            : scope.EnclosingScope is { } s
                ? GetGlobalScope(s)
                : scope;

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
        NameSpace.LibraryFunction => new LibraryNode("StandardLibrary.Functions"),
        NameSpace.LibraryProcedure => new LibraryNode("StandardLibrary.Procedure"),
        NameSpace.UserGlobal => new GlobalNode(),
        _ => null
    };

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


    #endregion
}