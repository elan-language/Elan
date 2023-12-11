using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using Antlr4.Runtime.Atn;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;
using System.Xml.Linq;
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

    public VisitorPass Pass { get; set; }= VisitorPass.First;

    public SymbolTableVisitor() => currentScope = SymbolTable.GlobalScope;

    public SymbolTableImpl SymbolTable { get; } = new();

    public IAstNode Visit(IAstNode astNode) {
        return astNode switch {
            MainNode n => VisitMainNode(n),
            ProcedureDefNode n => VisitProcedureDefNode(n),
            ForEachStatementNode n => VisitForEachNode(n),
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
            EnumDefNode n => VisitEnumDefNode(n),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

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

    private IAstNode VisitForEachNode(ForEachStatementNode forEachStatementNode) {
        var parameterIds = new string[] { GetId(forEachStatementNode.Id)! };

        if (Pass is VisitorPass.First) {

            var ms = new ScopedStatementSymbol(forEachStatementNode.Name, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(forEachStatementNode.Name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(forEachStatementNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return forEachStatementNode;
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

        if (Pass is VisitorPass.First) {

            var ms = new FunctionSymbol(name, rt, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitLambdaDefNode(LambdaDefNode lambda) {
        var parameterIds = lambda.Arguments.OfType<IdentifierNode>().Select(idn => idn.Id).ToArray();
        var rt = MapNodeToSymbolType(lambda.Expression);

        if (Pass is VisitorPass.First) {

            var ms = new LambdaSymbol(lambda.Name, rt, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(lambda.Name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(lambda);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return lambda;
    }

    private IAstNode VisitAbstractFunctionDefNode(AbstractFunctionDefNode functionDefNode) {
        var (name, parameterIds) = NameAndParameterIds(functionDefNode.Signature);
        var rt = MapNodeToSymbolType(functionDefNode.Signature);

        if (Pass is VisitorPass.First) {

            var ms = new FunctionSymbol(name, rt, NameSpace.UserLocal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitSystemAccessorNode(SystemAccessorDefNode systemAccessorDefNode) {
        var (name, parameterIds) = NameAndParameterIds(systemAccessorDefNode.Signature);

        var rt = MapNodeToSymbolType(systemAccessorDefNode.Return);

        if (Pass is VisitorPass.First) {

            var ms = new SystemAccessorSymbol(name, rt, NameSpace.UserGlobal, parameterIds, currentScope);
            currentScope.Define(ms);
            currentScope = ms;
        }
        else {
            currentScope = currentScope.Resolve(name) as IScope ?? throw new Exception("unexpected null scope");
        }

        VisitChildren(systemAccessorDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return systemAccessorDefNode;
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
                VisitDeconstructionNode(dn);
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
            SymbolHelpers.ResolveVariable(name, currentScope);
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

    private void VisitDeconstructionNode(DeconstructionNode dn) {
        var names = dn.ItemNodes.OfType<IdentifierNode>().Select(i => i.Id).ToArray();

        if (Pass is VisitorPass.First) {
            var types = names.Select((n, i) => new PendingTupleResolveSymbol(n, i + 1));
            var zip = names.Zip(types);

            foreach (var (name, type) in zip) {
                var ms = new VariableSymbol(name, type, currentScope);
                currentScope.Define(ms);
            }
        }
        else {
            foreach (var name in names) {
                ResolveVariable(name, currentScope);
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
            SymbolHelpers.ResolveVariable(name, currentScope);
        }
    }

    private IAstNode VisitParameterNode(ParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(parameterNode.TypeNode);

        if (Pass is VisitorPass.First) {

            var ms = new ParameterSymbol(name, type, parameterNode.IsRef, currentScope);
            currentScope.Define(ms);

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