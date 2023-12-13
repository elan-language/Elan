using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using ValueType = AbstractSyntaxTree.ValueType;

namespace Compiler;

public static class CompilerTransforms {
    #region rules

    public static IAstNode? TransformLiteralListNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            LiteralListNode lln when lln.ItemNodes.First() is IdentifierNode idn and not IdentifierWithTypeNode => lln.Replace(idn, TypeIdentifier(idn, currentScope)),
            _ => null
        };

    // must come after TransformMethodCallNodes
    public static IAstNode? TransformProcedureParameterNodes(IAstNode[] nodes, IScope currentScope) {
        static IAstNode? TransformProcedureCallNode(ProcedureCallNode pcn, IScope scope) {
            var procedureScope = ResolveMethodCall(scope, pcn).Item1 as ProcedureSymbol ?? GetProcedure(pcn, scope); // may be no such procedure

            var parameterNodes = pcn.Parameters.Where(p => p is not ParameterCallNode).ToArray();

            if (procedureScope is not null && parameterNodes.Any()) {
                var transformedParameters = pcn.Parameters.Select((p, i) => TransformParameter(p, i, procedureScope));
                return pcn with { Parameters = transformedParameters.ToImmutableArray() };
            }

            return null;
        }

        static IAstNode TransformParameter(IAstNode p, int i, ProcedureSymbol scope) {
            // avoid failed with mismatched parameter counts
            if (i >= scope.ParameterNames.Length) {
                return new ParameterCallNode(p, false);
            }

            var matchingParm = scope.ParameterNames[i];

            var symbol = scope.Resolve(matchingParm) as ParameterSymbol ?? throw new NullReferenceException();
            return new ParameterCallNode(p, symbol.ByRef);
        }

        return nodes.Last() switch {
            ProcedureCallNode pcn => TransformProcedureCallNode(pcn, currentScope),
            _ => null
        };
    }

    #endregion

    #region helpers


    private static ISymbolType? GetQualifierType(ICallNode callNode, IScope currentScope) =>
        callNode.CalledOn switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => vs.ReturnType,
                ParameterSymbol ps => ps.ReturnType,
                _ => null
            },
            _ => null
        };

    private static ProcedureSymbol? GetProcedure(ICallNode mcn, IScope currentScope) {
        var type = GetQualifierType(mcn, currentScope);
        return type is ClassSymbolType cst ? GetProcedureSymbolFromClass(mcn, currentScope, cst) : null;
    }

    private static ProcedureSymbol? GetProcedureSymbolFromClass(ICallNode mcn, IScope currentScope, ClassSymbolType cst) =>
        currentScope.Resolve(cst.Name) switch {
            ClassSymbol cs => cs.Resolve(mcn.MethodName) as ProcedureSymbol,
            _ => throw new NotImplementedException()
        };

    private static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        _ => null
    };

    private static (ISymbol?, bool) ResolveMethodCall(IScope currentScope, ICallNode mcn) {
        var isGlobal = mcn.Qualifier is GlobalPrefixNode;
        var qualifiedId = GetId(mcn.CalledOn);

        if (qualifiedId is not null) {
            switch (currentScope.Resolve(qualifiedId)) {
                case VariableSymbol { ReturnType: ClassSymbolType }:
                    return (null, false);
                case ParameterSymbol { ReturnType: ClassSymbolType }:
                    return (null, false);
                case VariableSymbol or ParameterSymbol or null:
                    return (GetGlobalScope(currentScope).Resolve(mcn.MethodName), isGlobal);
            }
        }

        var scope = isGlobal ? GetGlobalScope(currentScope) : currentScope;
        return (scope.Resolve(mcn.MethodName), isGlobal);
    }

    private static IScope GetGlobalScope(IScope scope) =>
        scope is GlobalScope
            ? scope
            : scope.EnclosingScope is { } s
                ? GetGlobalScope(s)
                : scope;

    private static IAstNode MapSymbolToTypeNode(ISymbolType? type, IScope currentScope) {
        return type switch {
            ClassSymbolType cst => new TypeNode(new IdentifierNode(cst.Name)),
            IntSymbolType => new ValueTypeNode(ValueType.Int),
            FloatSymbolType => new ValueTypeNode(ValueType.Float),
            CharSymbolType => new ValueTypeNode(ValueType.Char),
            StringSymbolType => new ValueTypeNode(ValueType.String),
            BoolSymbolType => new ValueTypeNode(ValueType.Bool),
            LambdaSymbolType t => new LambdaTypeNode(t.Arguments.Select(a => MapSymbolToTypeNode(a, currentScope)).ToImmutableArray(), MapSymbolToTypeNode(t.ReturnType, currentScope)),
            TupleSymbolType t => new TupleTypeNode(t.Types.Select(a => MapSymbolToTypeNode(a, currentScope)).ToImmutableArray()),
            _ => throw new NotImplementedException(type?.ToString())
        };
    }

    private static IAstNode TypeIdentifier(IdentifierNode node, IScope currentScope) {
        var type = GetExpressionType(node, currentScope);
        var typeNode = MapSymbolToTypeNode(type, currentScope);

        return new IdentifierWithTypeNode(node.Id, typeNode);
    }

    private static ISymbolType GetTypeFromDepth(ISymbolType type, int depth) {
        if (depth is 0) {
            return type;
        }

        return type switch {
            ListSymbolType lst => GetTypeFromDepth(lst.OfType, depth - 1),
            _ => throw new NotImplementedException()
        };
    }

    private static ISymbolType? ResolveGenericType(GenericSymbolType gst, GenericFunctionSymbol fst, FunctionCallNode fcn, IScope currentScope) {
        var name = gst.TypeName;
        var indexAndDepth = fst.GenericParameters[name];
        var matchingParameter = fcn.Parameters[indexAndDepth.Item1];
        var symbolType = GetExpressionType(matchingParameter, currentScope);

        return symbolType is not null ? GetTypeFromDepth(symbolType, indexAndDepth.Item2) : null;
    }

    private static ISymbolType? EnsureResolved(ISymbolType symbolType, GenericFunctionSymbol fs, FunctionCallNode fcn, IScope currentScope) {
        return symbolType switch {
            PendingResolveSymbolType => symbolType,
            GenericSymbolType gst => ResolveGenericType(gst, fs, fcn, currentScope),
            _ => symbolType
        };
    }

    private static ISymbolType? GetExpressionType(IAstNode expression, IScope currentScope) {
        return expression switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => vs.ReturnType,
                ParameterSymbol ps => ps.ReturnType,
                _ => null
            },
            FunctionCallNode fcn => currentScope.Resolve(fcn.MethodName) switch {
                GenericFunctionSymbol fs => EnsureResolved(fs.ReturnType, fs, fcn, currentScope),
                FunctionSymbol fs => fs.ReturnType,
                _ => null
            },
            _ => SymbolHelpers.MapNodeToSymbolType(expression)
        };
    }

    #endregion
}