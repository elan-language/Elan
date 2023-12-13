using System.Collections.Immutable;
using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using ValueType = AbstractSyntaxTree.ValueType;

namespace SymbolTable;

public class SymbolHelpers {
    public static ISymbolType MapNodeToSymbolType(IAstNode node) {
        return node switch {
            IdentifierNode idn => new PendingResolveSymbolType(idn.Id),
            TypeNode tn => MapNodeToSymbolType(tn.TypeName),
            NewInstanceNode nin => MapNodeToSymbolType(nin.Type),
            ValueNode vn => MapNodeToSymbolType(vn.TypeNode),
            ValueTypeNode vtn => MapElanValueTypeToSymbolType(vtn.Type.ToString()),
            LiteralTupleNode ltn => new TupleSymbolType(ltn.ItemNodes.Select(MapNodeToSymbolType).ToArray()),
            LiteralListNode lln => new ListSymbolType(MapNodeToSymbolType(lln.ItemNodes.First())),
            LiteralDictionaryNode => new DictionarySymbolType(),
            DataStructureTypeNode { Type: DataStructure.Iter } dsn => new IterSymbolType(MapNodeToSymbolType(dsn.GenericTypes.Single())),
            DataStructureTypeNode { Type: DataStructure.Array } dsn => new ArraySymbolType(MapNodeToSymbolType(dsn.GenericTypes.Single())),
            DataStructureTypeNode { Type: DataStructure.List } dsn => new ListSymbolType(MapNodeToSymbolType(dsn.GenericTypes.Single())),
            DataStructureTypeNode { Type: DataStructure.Dictionary } => new DictionarySymbolType(),
            FunctionCallNode mcn => new PendingResolveSymbolType(mcn.MethodName),
            ProcedureCallNode mcn => new PendingResolveSymbolType(mcn.MethodName),
            SystemAccessorCallNode mcn => new PendingResolveSymbolType(mcn.MethodName),
            BinaryNode { Operator: OperatorNode op } when OperatorEvaluatesToBoolean(op.Value) => BoolSymbolType.Instance,
            BinaryNode bn => MapNodeToSymbolType(bn.Operand1),
            IndexedExpressionNode ien => MapNodeToSymbolType(ien.Expression),
            BracketNode bn => MapNodeToSymbolType(bn.BracketedNode),
            UnaryNode un => MapNodeToSymbolType(un.Operand),
            EnumValueNode en => MapNodeToSymbolType(en.TypeNode),
            DefaultNode dn => MapNodeToSymbolType(dn.Type),
            WithNode wn => MapNodeToSymbolType(wn.Expression),
            PropertyCallNode pn => MapNodeToSymbolType(pn.Expression),
            ReturnExpressionNode ren => MapNodeToSymbolType(ren.Expression),
            QualifiedNode qn => MapNodeToSymbolType(qn.Qualified),
            LambdaTypeNode fn => new LambdaSymbolType(fn.Types.Select(MapNodeToSymbolType).ToArray(), MapNodeToSymbolType(fn.ReturnType)),
            MethodSignatureNode { ReturnType: not null } msn => MapNodeToSymbolType(msn.ReturnType),
            InputNode => StringSymbolType.Instance,
            TupleTypeNode ttn => new TupleSymbolType(ttn.Types.Select(MapNodeToSymbolType).ToArray()),
            TupleDefNode ttn => new TupleSymbolType(ttn.Expressions.Select(MapNodeToSymbolType).ToArray()),
            LambdaDefNode ldn => new LambdaSymbolType(ldn.Arguments.Select(MapNodeToSymbolType).ToArray(), MapNodeToSymbolType(ldn.Expression)),
            IfExpressionNode n => MapNodeToSymbolType(n.Expressions.First()),
            _ => throw new NotImplementedException(node.GetType().ToString())
        };
    }

    private static bool OperatorEvaluatesToBoolean(Operator op) =>
        op switch {
            Operator.And => true,
            Operator.Plus => false,
            Operator.Minus => false,
            Operator.Multiply => false,
            Operator.Divide => false,
            Operator.Power => false,
            Operator.Modulus => false,
            Operator.IntDivide => false,
            Operator.Equal => true,
            Operator.Or => true,
            Operator.Xor => true,
            Operator.LessThan => true,
            Operator.GreaterThanEqual => true,
            Operator.GreaterThan => true,
            Operator.LessThanEqual => true,
            Operator.NotEqual => true,
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };

    private static ISymbolType MapElanValueTypeToSymbolType(string type) =>
        type switch {
            IntSymbolType.Name => IntSymbolType.Instance,
            StringSymbolType.Name => StringSymbolType.Instance,
            FloatSymbolType.Name => FloatSymbolType.Instance,
            BoolSymbolType.Name => BoolSymbolType.Instance,
            CharSymbolType.Name => CharSymbolType.Instance,
            _ => throw new NotImplementedException()
        };

  

    private static IScope GetGlobalScope(IScope scope) =>
        scope is GlobalScope
            ? scope
            : scope.EnclosingScope is { } s
                ? GetGlobalScope(s)
                : scope;



    private static ISymbol? GetSymbolForCallNode(ICallNode mcn, IScope currentScope, ClassSymbolType cst) {
        var classSymbol = currentScope.Resolve(cst.Name);

        return classSymbol switch {
            IScope classScope => classScope.Resolve(mcn.MethodName),
            _ => null
        };
    }

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
        callNode.CalledOn switch {
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

    private static ISymbol? GetSpecificCallNodeForClassMethod(ICallNode mcn, IScope currentScope) {
        var type = GetQualifierType(mcn, currentScope);
        return type is ClassSymbolType cst ? GetSymbolForCallNode(mcn, currentScope, cst) : null;
    }

    public static ISymbolType? EnsureResolved(ISymbolType symbolType, IScope currentScope, int rc = 0) {
        return symbolType switch {
            _ when rc > 1 => symbolType,
            PendingResolveSymbolType rr => currentScope.Resolve(rr.Name) switch {
                VariableSymbol s => EnsureResolved(s.ReturnType, currentScope, ++rc),
                ClassSymbol s => new ClassSymbolType(s.Name),
                EnumSymbol s => new EnumSymbolType(s.Name),
                FunctionSymbol s => EnsureResolved(s.ReturnType, currentScope, ++rc),
                ParameterSymbol s => EnsureResolved(s.ReturnType, currentScope, ++rc),
                SystemAccessorSymbol s => EnsureResolved(s.ReturnType, currentScope, ++rc),
                _ => rr
            },
            PendingDeconstructionResolveSymbol rr => EnsureResolved(rr.Tuple, currentScope) switch {
                TupleSymbolType st => st.Types[rr.Index - 1],
                ListSymbolType st => st.OfType,
                ArraySymbolType st => st.OfType,
                IterSymbolType st => st.OfType,
                _ => rr
            },
            _ => symbolType
        };
    }

    public static ISymbol? ResolveCall(ICallNode callNode, IScope currentScope) {
        if (callNode.CalledOn is { } co) {
            var qualifiedId = GetId(co);

            if (qualifiedId is not null) {
                switch (currentScope.Resolve(qualifiedId)) {
                    case VariableSymbol vs when EnsureResolved(vs.ReturnType, currentScope) is ClassSymbolType:
                        return GetSpecificCallNodeForClassMethod(callNode, currentScope);
                    case ParameterSymbol ps when EnsureResolved(ps.ReturnType, currentScope) is ClassSymbolType:
                        return GetSpecificCallNodeForClassMethod(callNode, currentScope);
                    default: return GetGlobalScope(currentScope).Resolve(callNode.MethodName);
                }
            }
        }

        return currentScope.Resolve(callNode.MethodName);
    }

    public static void ResolveReturnType(string name, IScope currentScope) {
        var vs = currentScope.Resolve(name) as IHasReturnType ?? throw new NullReferenceException(name);

        if (vs.ReturnType is IPendingResolveSymbolType prs) {
            var rt = EnsureResolved(prs, currentScope) ?? throw new NullReferenceException(name);
            vs.ReturnType = rt;
        }
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

    public static ISymbolType? ResolveGenericType(GenericSymbolType gst, GenericFunctionSymbol fst, FunctionCallNode fcn, IScope currentScope) {
        var name = gst.TypeName;
        var indexAndDepth = fst.GenericParameters[name];
        var matchingParameter = fcn.Parameters[indexAndDepth.Item1];
        var symbolType = GetExpressionType(matchingParameter, currentScope);

        return symbolType is not null ? GetTypeFromDepth(symbolType, indexAndDepth.Item2) : null;
    }

    public static IAstNode TypeIdentifier(IdentifierNode node, IScope currentScope) {
        var type = GetExpressionType(node, currentScope);
        var typeNode = MapSymbolToTypeNode(type, currentScope);

        return new IdentifierWithTypeNode(node.Id, typeNode);
    }

    public static IAstNode MapSymbolToTypeNode(ISymbolType? type, IScope currentScope) {
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

    public static ProcedureSymbol? GetProcedure(ICallNode mcn, IScope currentScope) {
        var type = GetQualifierType(mcn, currentScope);
        return type is ClassSymbolType cst ? GetProcedureSymbolFromClass(mcn, currentScope, cst) : null;
    }

    private static ProcedureSymbol? GetProcedureSymbolFromClass(ICallNode mcn, IScope currentScope, ClassSymbolType cst) =>
        currentScope.Resolve(cst.Name) switch {
            ClassSymbol cs => cs.Resolve(mcn.MethodName) as ProcedureSymbol,
            _ => throw new NotImplementedException()
        };

    public static string? GetId(IAstNode? node) => node switch {
        IdentifierNode idn => idn.Id,
        IndexedExpressionNode ien => GetId(ien.Expression),
        PropertyCallNode pcn => GetId(pcn.Expression),
        _ => null
    };

    public static (ISymbol?, bool) ResolveMethodCall(IScope currentScope, ICallNode mcn) {
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

  
  
    

    private static ISymbolType? EnsureResolved(ISymbolType symbolType, GenericFunctionSymbol fs, FunctionCallNode fcn, IScope currentScope) {
        return symbolType switch {
            PendingResolveSymbolType => symbolType,
            GenericSymbolType gst => SymbolHelpers.ResolveGenericType(gst, fs, fcn, currentScope),
            _ => symbolType
        };
    }

   

}