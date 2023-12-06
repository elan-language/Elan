using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime.Atn;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using static SymbolTable.SymbolHelpers;

namespace SymbolTable;

public class SymbolTableVisitor {
    public readonly IList<string> SymbolErrors = new List<string>();

    private IScope currentScope;

    public SymbolTableVisitor() => currentScope = SymbolTable.GlobalScope;

    public SymbolTableImpl SymbolTable { get; } = new();

    public IAstNode Visit(IAstNode astNode) {
        return astNode switch {
            MainNode n => VisitMainNode(n),
            ProcedureDefNode n => VisitProcedureDefNode(n),
            AbstractProcedureDefNode n => VisitAbstractProcedureDefNode(n),
            FunctionDefNode n => VisitFunctionDefNode(n),
            LambdaDefNode n => VisitLambdaDefNode(n),
            AbstractFunctionDefNode n => VisitAbstractFunctionDefNode(n),
            SystemAccessorDefNode n => VisitSystemAccessorNode(n),
            ConstructorNode n => VisitConstructorNode(n),
            ClassDefNode n => VisitClassDefNode(n),
            AbstractClassDefNode n => VisitAbstractClassDefNode(n),
            VarDefNode n => VisitVarDefNode(n),
            ConstantDefNode n => VisitConstantDefNode(n),
            ParameterNode n => VisitParameterNode(n),
            PropertyDefNode n => VisitPropertyDefNode(n),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

    private IAstNode VisitProcedureDefNode(ProcedureDefNode procedureDefNode) {
        var (name, parameterIds) = NameAndParameterIds(procedureDefNode.Signature);

        var ms = new ProcedureSymbol(name, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(procedureDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitAbstractProcedureDefNode(AbstractProcedureDefNode procedureDefNode) {
        var (name, parameterIds) = NameAndParameterIds(procedureDefNode.Signature);

        var ms = new ProcedureSymbol(name,  NameSpace.UserLocal, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitConstructorNode(ConstructorNode constructorNode) {
        var ms = new ProcedureSymbol(Constants.WellKnownConstructorId, NameSpace.UserLocal, Array.Empty<string>(), currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(constructorNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return constructorNode;
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

    private IAstNode VisitFunctionDefNode(FunctionDefNode functionDefNode) {
        var (name, parameterIds) = NameAndParameterIds(functionDefNode.Signature);
        var rt = MapNodeToSymbolType(functionDefNode.Signature);

        var ms = new FunctionSymbol(name, rt, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitLambdaDefNode(LambdaDefNode lambda) {
        var parameterIds = lambda.Arguments.OfType<IdentifierNode>().Select(idn => idn.Id).ToArray();
        var rt = MapNodeToSymbolType(lambda.Expression);

        var ms = new LambdaSymbol(lambda.Name, rt, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(lambda);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return lambda;
    }

    private IAstNode VisitAbstractFunctionDefNode(AbstractFunctionDefNode functionDefNode) {
        var (name, parameterIds) = NameAndParameterIds(functionDefNode.Signature);
        var rt = MapNodeToSymbolType(functionDefNode.Signature);

        var ms = new FunctionSymbol(name, rt, NameSpace.UserLocal, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitSystemAccessorNode(SystemAccessorDefNode systemAccessorDefNode) {
        var (name, parameterIds) = NameAndParameterIds(systemAccessorDefNode.Signature);

        var rt = MapNodeToSymbolType(systemAccessorDefNode.Return);

        var ms = new SystemAccessorSymbol(name, rt, NameSpace.UserGlobal, parameterIds, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(systemAccessorDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return systemAccessorDefNode;
    }

    private IAstNode VisitMainNode(MainNode mainNode) {
        var ms = new ProcedureSymbol(Constants.WellKnownMainId, NameSpace.System, Array.Empty<string>(), currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(mainNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return mainNode;
    }

    private IAstNode VisitClassDefNode(ClassDefNode classDefNode) {
        var ms = new ClassSymbol(classDefNode.Name, classDefNode.Immutable ? ClassSymbolTypeType.Immutable : ClassSymbolTypeType.Mutable, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(classDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return classDefNode;
    }

    private IAstNode VisitAbstractClassDefNode(AbstractClassDefNode abstractClassDefNode) {
        var ms = new ClassSymbol(abstractClassDefNode.Name, ClassSymbolTypeType.Abstract, currentScope);
        currentScope.Define(ms);
        currentScope = ms;
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
                VisitDeconstructionNode(dn);
                break;
            }
        }

        VisitChildren(varDefNode);
        return varDefNode;
    }

    private IAstNode VisitConstantDefNode(ConstantDefNode constantDefNode) {
        var name = constantDefNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(constantDefNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(constantDefNode.Expression);

        var ms = new VariableSymbol(name, type, currentScope);
        currentScope.Define(ms);
        VisitChildren(constantDefNode);
        return constantDefNode;
    }

    private void VisitDeconstructionNode(DeconstructionNode dn) {
        var names = dn.ItemNodes.OfType<IdentifierNode>().Select(i => i.Id).ToArray();
        var types = names.Select((n, i) => new PendingTupleResolveSymbol(n, i + 1));
        var zip = names.Zip(types);

        foreach (var (name, type) in zip) {
            var ms = new VariableSymbol(name, type, currentScope);
            currentScope.Define(ms);
        }
    }

    private void VisitIdentifierNode(VarDefNode varDefNode, IdentifierNode idn) {
        var name = idn.Id;
        var type = MapNodeToSymbolType(varDefNode.Rhs);
        var ms = new VariableSymbol(name, type, currentScope);
        currentScope.Define(ms);
    }

    private IAstNode VisitParameterNode(ParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(parameterNode.TypeNode);

        var ms = new ParameterSymbol(name, type, parameterNode.IsRef, currentScope);
        currentScope.Define(ms);
        VisitChildren(parameterNode);
        return parameterNode;
    }

    private IAstNode VisitPropertyDefNode(PropertyDefNode propertyNode) {
        var name = propertyNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(propertyNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(propertyNode.Type);

        var ms = new VariableSymbol(name, type, currentScope);
        currentScope.Define(ms);
        VisitChildren(propertyNode);
        return propertyNode;
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
}