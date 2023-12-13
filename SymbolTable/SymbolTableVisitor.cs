using AbstractSyntaxTree.Nodes;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using static SymbolTable.SymbolHelpers;

namespace SymbolTable;

public enum VisitorPass {
    Pending,
    First,
    Second
}

public class SymbolTableVisitor {
    public readonly IList<string> SymbolErrors = new List<string>();

    private IScope currentScope;

    public SymbolTableVisitor() => currentScope = SymbolTable.GlobalScope;

    public VisitorPass Pass { get; set; } = VisitorPass.First;

    public SymbolTableImpl SymbolTable { get; } = new();

    public IAstNode Visit(IAstNode astNode) {
        return astNode switch {
            MainNode n => VisitMainNode(n),
            ProcedureDefNode n => VisitProcedureDefNode(n),
            EachStatementNode n => VisitForEachNode(n),
            EachParameterNode n => VisitForEachParameterNode(n),
            AbstractProcedureDefNode n => VisitAbstractProcedureDefNode(n),
            FunctionDefNode n => VisitFunctionDefNode(n),
            AbstractFunctionDefNode n => VisitAbstractFunctionDefNode(n),
            ConstructorNode n => VisitConstructorNode(n),
            ClassDefNode n => VisitClassDefNode(n),
            AbstractClassDefNode n => VisitAbstractClassDefNode(n),
            VarDefNode n => VisitVarDefNode(n),
            ConstantDefNode n => VisitConstantDefNode(n),
            ParameterNode n => VisitParameterNode(n),
            PropertyDefNode n => VisitPropertyDefNode(n),
            EnumDefNode n => VisitEnumDefNode(n),
            ICallNode n => VisitCallNode(n),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

    public void Validate() {
        ValidateScope(SymbolTable.GlobalScope);
    }

    private void ValidateScope(IScope scope) {
        var symbols = ((BaseScope)scope).Symbols;

        foreach (var symbol in symbols) {
            if (symbol is IHasReturnType { ReturnType: IPendingResolveSymbolType }) {
                SymbolErrors.Add($"Unresolved Symbol {symbol}");
            }

            if (symbol is IScope subScope) {
                ValidateScope(subScope);
            }
        }
    }

    private static string[] ParameterIds(MethodSignatureNode node) {
        return node.Parameters.OfType<ParameterNode>().Select(pn => Id(pn.Id)).ToArray();
    }

    private static string Id(IAstNode node) => node is IdentifierNode idn ? idn.Id : throw new NotImplementedException($"{node}");

    private static (string, string[]) NameAndParameterIds(IAstNode node) {
        return node switch {
            MethodSignatureNode msn => (Id(msn.Id), ParameterIds(msn)),
            _ => throw new NotImplementedException("null")
        };
    }

    private void VisitDeconstructionNode(VarDefNode varDefNode, DeconstructionNode dn) {
        var names = dn.ItemNodes.OfType<IdentifierNode>().Select(i => i.Id).ToArray();

        if (Pass is VisitorPass.First) {
            var types = names.Select((_, i) => new PendingDeconstructionResolveSymbol(MapNodeToSymbolType(varDefNode.Rhs), i + 1));
            var zip = names.Zip(types);

            foreach (var (name, type) in zip) {
                var ms = new VariableSymbol(name, type, currentScope);
                currentScope.Define(ms);
            }
        }
        else {
            foreach (var name in names) {
                ResolveReturnType(name, currentScope);
            }
        }
    }

    private void VisitIdentifierNode(VarDefNode varDefNode, IdentifierNode idn) {
        var name = idn.Id;

        if (Pass is VisitorPass.First) {
            var type = MapNodeToSymbolType(varDefNode.Rhs);
            var ms = new VariableSymbol(name, type, currentScope);
            currentScope.Define(ms);
        }
        else {
            ResolveReturnType(name, currentScope);
        }
    }

    private IAstNode VisitChildren(IAstNode node) {
        foreach (var child in node.Children) {
            try {
                Visit(child);
            }
            catch (SymbolException e) {
                SymbolErrors.Add(e.Message);
            }
        }

        return node;
    }

    #region visit functions

    private IAstNode VisitProcedureDefNode(ProcedureDefNode procedureDefNode) {
        var (name, parameterIds) = NameAndParameterIds(procedureDefNode.Signature);

        if (Pass is VisitorPass.First) {
            var ms = new ProcedureSymbol(name, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(procedureDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitForEachNode(EachStatementNode eachStatementNode) {
        var parameterIds = new[] { GetId(eachStatementNode.Parameter)! };

        if (Pass is VisitorPass.First) {
            var ms = new ScopedStatementSymbol(eachStatementNode.Name, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(eachStatementNode.Name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(eachStatementNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return eachStatementNode;
    }

    private IAstNode VisitAbstractProcedureDefNode(AbstractProcedureDefNode procedureDefNode) {
        var (name, parameterIds) = NameAndParameterIds(procedureDefNode.Signature);

        if (Pass is VisitorPass.First) {
            var ms = new ProcedureSymbol(name, NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitConstructorNode(ConstructorNode constructorNode) {
        if (Pass is VisitorPass.First) {
            var ms = new ProcedureSymbol(Constants.WellKnownConstructorId, NameSpace.UserLocal, Array.Empty<string>(), currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(Constants.WellKnownConstructorId) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(constructorNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return constructorNode;
    }

    private IAstNode VisitFunctionDefNode(FunctionDefNode functionDefNode) {
        var (name, parameterIds) = NameAndParameterIds(functionDefNode.Signature);

        if (Pass is VisitorPass.First) {
            var rt = MapNodeToSymbolType(functionDefNode.Signature);
            var ms = new FunctionSymbol(name, rt, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            ResolveReturnType(name, currentScope);
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitAbstractFunctionDefNode(AbstractFunctionDefNode functionDefNode) {
        var (name, parameterIds) = NameAndParameterIds(functionDefNode.Signature);

        if (Pass is VisitorPass.First) {
            var rt = MapNodeToSymbolType(functionDefNode.Signature);
            var ms = new FunctionSymbol(name, rt, NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            ResolveReturnType(name, currentScope);
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitMainNode(MainNode mainNode) {
        if (Pass is VisitorPass.First) {
            var ms = new ProcedureSymbol(Constants.WellKnownMainId, NameSpace.System, Array.Empty<string>(), currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(Constants.WellKnownMainId) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(mainNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return mainNode;
    }

    private IAstNode VisitClassDefNode(ClassDefNode classDefNode) {
        if (Pass is VisitorPass.First) {
            var ms = new ClassSymbol(classDefNode.Name, classDefNode.Immutable ? ClassSymbolTypeType.Immutable : ClassSymbolTypeType.Mutable, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(classDefNode.Name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(classDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return classDefNode;
    }

    private IAstNode VisitAbstractClassDefNode(AbstractClassDefNode abstractClassDefNode) {
        if (Pass is VisitorPass.First) {
            var ms = new ClassSymbol(abstractClassDefNode.Name, ClassSymbolTypeType.Abstract, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(abstractClassDefNode.Name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(abstractClassDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return abstractClassDefNode;
    }

    private IAstNode VisitVarDefNode(VarDefNode varDefNode) {
        switch (varDefNode.Id) {
            case IdentifierNode idn: {
                VisitIdentifierNode(varDefNode, idn);
                break;
            }
            case DeconstructionNode dn: {
                VisitDeconstructionNode(varDefNode, dn);
                break;
            }
        }

        VisitChildren(varDefNode);
        return varDefNode;
    }

    private IAstNode VisitConstantDefNode(ConstantDefNode constantDefNode) {
        var name = constantDefNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(constantDefNode.Id.GetType().ToString());

        if (Pass is VisitorPass.First) {
            var type = MapNodeToSymbolType(constantDefNode.Expression);
            var ms = new VariableSymbol(name, type, currentScope);
            currentScope.Define(ms);
        }
        else {
            ResolveReturnType(name, currentScope);
        }

        VisitChildren(constantDefNode);
        return constantDefNode;
    }

    private IAstNode VisitEnumDefNode(EnumDefNode enumDefNode) {
        var name = enumDefNode.Type is TypeNode { TypeName : IdentifierNode idn } ? idn.Id : throw new NotImplementedException(enumDefNode.Type.GetType().ToString());

        if (Pass is VisitorPass.First) {
            var ms = new EnumSymbol(name, currentScope);
            currentScope.Define(ms);
        }

        VisitChildren(enumDefNode);
        return enumDefNode;
    }

    private void AddLambdaArguments(LambdaDefNode lambda, ISymbolType[] arguments, LambdaSymbol ls) {
        var zipArgs = lambda.Arguments.Zip(arguments);

        foreach (var valueTuple in zipArgs) {
            var symbol = new ParameterSymbol(GetId(valueTuple.First)!, valueTuple.Second, false, ls);
            ls.Define(symbol);
        }
    }

    private void ReplaceLambdaArguments(LambdaDefNode lambda, ISymbolType[] arguments, LambdaSymbol ls) {
        var zipArgs = lambda.Arguments.Zip(arguments);

        foreach (var valueTuple in zipArgs) {
            var vs = ls.Resolve(GetId(valueTuple.First)!) as ParameterSymbol;

            if (vs?.ReturnType is IPendingResolveSymbolType) {
                vs.ReturnType = valueTuple.Second;
            }
        }
    }

    private IAstNode VisitCallNode(ICallNode callNode) {
        var sig = ResolveCall(callNode, currentScope);

        // check they match
        if ((sig is ProcedureSymbol && callNode is not ProcedureCallNode) || (sig is FunctionSymbol && callNode is not FunctionCallNode)) {
            throw new SymbolException($"Mismatched Procedure/Function call {callNode.MethodName}");
        }

        var names = sig switch {
            MethodSymbol ms => ms.ParameterNames,
            ParameterSymbol ps => new[] { ps.Name },
            VariableSymbol { ReturnType: LambdaSymbolType } => Array.Empty<string>(),
            _ => Array.Empty<string>()
        };

        var scopeName = $"{callNode.Name}";

        if (Pass is VisitorPass.First) {
            var ss = new ScopedStatementSymbol(scopeName, names, currentScope);
            currentScope.Define(ss);
            currentScope = ss;
        }
        else {
            var ss = currentScope.Resolve(scopeName) as ScopedStatementSymbol ?? throw new Exception("unexpected null scope");
            ss.ParameterNames = names;
            currentScope = ss;
        }

        if (callNode.Parameters.Any(p => p is LambdaDefNode)) {
            var index = callNode.Parameters.Length == names.Length ? 0 : 1; // to ignore 'this' parameter if not passed in 
            foreach (var node in callNode.Parameters) {
                if (node is LambdaDefNode lambda) {
                    if (Pass is VisitorPass.First) {
                        var parameterIds = lambda.Arguments.OfType<IdentifierNode>().Select(idn => idn.Id).ToArray();
                        LambdaSymbol? ls = null;

                        if (sig is IScope sigScope) {
                            var lambdaSymbolType = GetLambdaSymbolType(names, index, sigScope);
                            ls = new LambdaSymbol(lambda.Name, lambdaSymbolType.ReturnType, parameterIds, currentScope);
                            var arguments = GetLambdaArguments(callNode, sig, lambdaSymbolType);
                            AddLambdaArguments(lambda, arguments.ToArray(), ls);
                        }
                        else {
                            ls = new LambdaSymbol(lambda.Name, new PendingResolveSymbolType(lambda.Name), parameterIds, currentScope);
                        }

                        currentScope.Define(ls);
                        currentScope = ls;
                    }
                    else {
                        if (sig is IScope sigScope) {
                            var lambdaSymbolType = GetLambdaSymbolType(names, index, sigScope);
                            var ls = currentScope.Resolve(lambda.Name) as LambdaSymbol;
                            var arguments = GetLambdaArguments(callNode, sig, lambdaSymbolType);

                            ReplaceLambdaArguments(lambda, arguments.ToArray(), ls!);
                        }

                        ResolveReturnType(lambda.Name, currentScope);
                        currentScope = currentScope.Resolve(lambda.Name) as IScope ?? throw new Exception("unexpected null scope");
                    }

                    VisitChildren(lambda);
                    currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
                }

                index++;
            }
        }

        VisitChildren(callNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return callNode;
    }

    private static LambdaSymbolType GetLambdaSymbolType(string[] names, int index, IScope sigScope) {
        var matchingSymbolName = names[index];
        var matchingSymbol = sigScope.Resolve(matchingSymbolName) as ParameterSymbol;
        var lambdaSymbolType = matchingSymbol?.ReturnType as LambdaSymbolType;
        return lambdaSymbolType!;
    }

    private IEnumerable<ISymbolType> GetLambdaArguments(ICallNode callNode, ISymbol sig, LambdaSymbolType lambdaSymbolType) {
        return
            sig is GenericFunctionSymbol gfs && callNode is FunctionCallNode fcn
                ? lambdaSymbolType.Arguments.Select(a => EnsureResolved(a, gfs, fcn, currentScope)!)
                : lambdaSymbolType.Arguments.Select(a => EnsureResolved(a, currentScope)!);
    }

    private IAstNode VisitParameterNode(ParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(parameterNode.TypeNode);

        if (Pass is VisitorPass.First) {
            var ms = new ParameterSymbol(name, type, parameterNode.IsRef, currentScope);
            currentScope.Define(ms);
        }
        else {
            ResolveReturnType(name, currentScope);
        }

        VisitChildren(parameterNode);
        return parameterNode;
    }

    private IAstNode VisitForEachParameterNode(EachParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(parameterNode.Expression); // todo this type will be collection not item!

        if (Pass is VisitorPass.First) {
            var ms = new ParameterSymbol(name, type, false, currentScope);
            currentScope.Define(ms);
        }
        else {
            ResolveReturnType(name, currentScope);
        }

        VisitChildren(parameterNode);
        return parameterNode;
    }

    private IAstNode VisitPropertyDefNode(PropertyDefNode propertyNode) {
        var name = propertyNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(propertyNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(propertyNode.Type);

        if (Pass is VisitorPass.First) {
            var ms = new VariableSymbol(name, type, currentScope);
            currentScope.Define(ms);
        }
        else {
            ResolveReturnType(name, currentScope);
        }

        VisitChildren(propertyNode);
        return propertyNode;
    }

    #endregion region
}