using System.Collections.Immutable;
using AbstractSyntaxTree.Nodes;
using CSharpLanguageModel.Models;
using SymbolTable;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using ValueType = AbstractSyntaxTree.ValueType;

namespace Compiler;

public static class CompilerTransforms {
    #region rules

    public static IAstNode? TransformMethodCallNodes(IAstNode[] nodes, IScope currentScope) =>
        nodes.Last() switch {
            MethodCallNode mcn => ResolveMethodCall(currentScope, mcn) switch {
                (SystemAccessorSymbol sas, _) when mcn.Qualifier is SystemAccessorPrefixNode => new SystemAccessorCallNode(mcn.Id, NameSpaceToNode(sas.NameSpace), mcn.Parameters) { DotCalled = mcn.DotCalled },
                (ProcedureSymbol ps, false) => new ProcedureCallNode(mcn, NameSpaceToNode(ps.NameSpace)),
                (ProcedureSymbol, true) => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (FunctionSymbol fs, false) => new FunctionCallNode(mcn, NameSpaceToNode(fs.NameSpace)),
                (FunctionSymbol, true) => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                (ParameterSymbol { ReturnType: FuncSymbolType }, _) => new FunctionCallNode(mcn.Id, null, mcn.Parameters),
                (VariableSymbol vs, _) when EnsureResolved(vs.ReturnType, currentScope) is FuncSymbolType => new FunctionCallNode(mcn.Id, null, mcn.Parameters),
                _ => GetSpecificCallNodeForClassMethod(mcn, currentScope)
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
        static IAstNode? TransformProcedureCallNode(ProcedureCallNode pcn, IScope scope) {
            var procedureScope = ResolveMethodCall(scope, pcn).Item1 as ProcedureSymbol ?? GetProcedure(pcn, scope) ?? throw new NullReferenceException();

            var parameterNodes = pcn.Parameters.Where(p => p is not ParameterCallNode).ToArray();

            if (parameterNodes.Any()) {
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

    private static ISymbolType? ResolveProperty(PropertyCallNode pn, ClassSymbolType? symbolType, IScope currentScope) {
        var cst = symbolType is not null ? GetGlobalScope(currentScope).Resolve(symbolType.Name) as IScope : null;
        var name = pn.Property is IdentifierNode idn ? idn.Id : throw new NullReferenceException();
        var symbol = cst?.Resolve(name);

        return symbol switch {
            VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
            _ => null
        };
    }

    private static ISymbol? ResolveToIdentifier(IAstNode? node, IScope currentScope) =>
        node switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id),
            PropertyCallNode pn => ResolveToIdentifier(pn.Expression, currentScope),
            _ => null
        };

    private static ISymbolType? GetQualifierType(ICallNode callNode, IScope currentScope) =>
        callNode.Qualifier switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ParameterSymbol ps => EnsureResolved(ps.ReturnType, currentScope),
                _ => null
            },
            PropertyCallNode pn => ResolveToIdentifier(pn, currentScope) switch {
                VariableSymbol vs => ResolveProperty(pn, EnsureResolved(vs.ReturnType, currentScope) as ClassSymbolType, currentScope),
                _ => null
            },
            _ => null
        };

    private static IAstNode? GetSpecificCallNode(MethodCallNode mcn, IScope currentScope, ClassSymbolType cst) {
        var classSymbol = currentScope.Resolve(cst.Name);

        return classSymbol switch {
            IScope classScope => classScope.Resolve(mcn.Name) switch {
                ProcedureSymbol { NameSpace: NameSpace.UserLocal } => new ProcedureCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                ProcedureSymbol ps => new ProcedureCallNode(mcn, NameSpaceToNode(ps.NameSpace)),
                FunctionSymbol { NameSpace: NameSpace.UserLocal } => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                FunctionSymbol fs => new FunctionCallNode(mcn, NameSpaceToNode(fs.NameSpace)),
                VariableSymbol { ReturnType: FuncSymbolType } => new FunctionCallNode(mcn.Id, mcn.Qualifier, mcn.Parameters),
                _ => null
            },
            _ => null
        };
    }

    private static IAstNode? GetSpecificCallNodeForClassMethod(MethodCallNode mcn, IScope currentScope) {
        var type = GetQualifierType(mcn, currentScope);
        return type is ClassSymbolType cst ? GetSpecificCallNode(mcn, currentScope, cst) : null;
    }

    private static ProcedureSymbol? GetProcedure(ICallNode mcn, IScope currentScope) {
        var type = GetQualifierType(mcn, currentScope);
        return type is ClassSymbolType cst ? GetProcedureSymbolFromClass(mcn, currentScope, cst) : null;
    }

    private static ProcedureSymbol? GetProcedureSymbolFromClass(ICallNode mcn, IScope currentScope, ClassSymbolType cst) =>
        currentScope.Resolve(cst.Name) switch {
            ClassSymbol cs => cs.Resolve(mcn.Name) as ProcedureSymbol,
            _ => throw new NotImplementedException()
        };

    private static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        _ => null
    };

    private static (ISymbol?, bool) ResolveMethodCall(IScope currentScope, ICallNode mcn) {
        var isGlobal = mcn.Qualifier is GlobalPrefixNode;
        var qualifiedId = GetId(mcn.Qualifier);

        if (qualifiedId is not null) {
            switch (currentScope.Resolve(qualifiedId)) {
                case VariableSymbol vs when EnsureResolved(vs.ReturnType, currentScope) is ClassSymbolType:
                    return (null, false);
                case ParameterSymbol ps when EnsureResolved(ps.ReturnType, currentScope) is ClassSymbolType:
                    return (null, false);
                case VariableSymbol or ParameterSymbol or null:
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

    private static IAstNode? NameSpaceToNode(NameSpace ns) => ns switch {
        NameSpace.System => new LibraryNode("StandardLibrary.SystemAccessors"),
        NameSpace.LibraryFunction => new LibraryNode("StandardLibrary.Functions"),
        NameSpace.LibraryProcedure => new LibraryNode("StandardLibrary.Procedures"),
        NameSpace.UserGlobal => new GlobalPrefixNode(),
        _ => null
    };

    private static ISymbolType? EnsureResolved(ISymbolType symbolType, IScope currentScope) {
        return symbolType switch {
            PendingResolveSymbol rr => currentScope.Resolve(rr.Name) switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ClassSymbol cs => new ClassSymbolType(cs.Name),
                FunctionSymbol fs => EnsureResolved(fs.ReturnType, currentScope),
                _ => throw new NotImplementedException()
            },
            _ => symbolType
        };
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


    private static ISymbolType ResolveGenericType(GenericSymbolType gst, GenericFunctionSymbol fst, FunctionCallNode fcn, IScope currentScope) {
        var name = gst.TypeName;
        var indexAndDepth = fst.GenericParameters[name];
        var matchingParameter = fcn.Parameters[indexAndDepth.Item1];

        var st = GetExpressionType(matchingParameter, currentScope);

        var actualType = GetTypeFromDepth(st, indexAndDepth.Item2);


        return actualType;
    }


    private static ISymbolType? EnsureResolved(ISymbolType symbolType, GenericFunctionSymbol fs, FunctionCallNode fcn, IScope currentScope) {
        return symbolType switch {
            PendingResolveSymbol => EnsureResolved(symbolType, currentScope),
            GenericSymbolType gst => ResolveGenericType(gst, fs, fcn, currentScope),
            _ => symbolType
        };
    }

    private static ISymbolType? GetExpressionType(IAstNode expression, IScope currentScope) {
        return expression switch {
            IdentifierNode idn => currentScope.Resolve(idn.Id) switch {
                VariableSymbol vs => EnsureResolved(vs.ReturnType, currentScope),
                ParameterSymbol ps => EnsureResolved(ps.ReturnType, currentScope),
                _ => null
            },
            FunctionCallNode fcn => currentScope.Resolve(fcn.Name) switch {
                GenericFunctionSymbol fs => EnsureResolved(fs.ReturnType, fs, fcn, currentScope),
                FunctionSymbol fs => EnsureResolved(fs.ReturnType, currentScope),
                _ => null
            },
            LiteralTupleNode => new TupleSymbolType(),
            _ => null
        };
    }

    #endregion
}