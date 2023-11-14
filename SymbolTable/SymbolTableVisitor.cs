using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using SymbolTable.Symbols;
using SymbolTable.SymbolTypes;

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
            FunctionDefNode n => VisitFunctionDefNode(n),
            SystemAccessorDefNode n => VisitSystemAccessorNode(n),
            ConstructorNode n => VisitConstructorNode(n),
            ClassDefNode n => VisitClassDefNode(n),
            AbstractClassDefNode n => VisitAbstractClassDefNode(n),
            VarDefNode n => VisitVarDefNode(n),
            ParameterNode n => VisitParameterNode(n),
            PropertyDefNode n => VisitPropertyDefNode(n),
            null => throw new NotImplementedException("null"),
            _ => VisitChildren(astNode)
        };
    }

    private IAstNode VisitProcedureDefNode(ProcedureDefNode procedureDefNode) {
        var name = procedureDefNode.Signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException("null")
        };

        var ms = new ProcedureSymbol(name, currentScope, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(procedureDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return procedureDefNode;
    }

    private IAstNode VisitConstructorNode(ConstructorNode constructorNode) {
        var ms = new ProcedureSymbol(Constants.WellKnownConstructorId, currentScope, NameSpace.UserLocal);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(constructorNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return constructorNode;
    }

    private IAstNode VisitFunctionDefNode(FunctionDefNode functionDefNode) {
        var name = functionDefNode.Signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException("null")
        };

        var rt = MapNodeToSymbolType(functionDefNode.Return);

        var ms = new FunctionSymbol(name, rt, currentScope, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
    }

    private IAstNode VisitSystemAccessorNode(SystemAccessorDefNode systemAccessorDefNode) {
        var name = systemAccessorDefNode.Signature switch {
            MethodSignatureNode { Id: IdentifierNode idn } => idn.Id,
            _ => throw new NotImplementedException("null")
        };

        var rt = MapNodeToSymbolType(systemAccessorDefNode.Return);

        var ms = new SystemAccessorSymbol(name, rt,  currentScope,  NameSpace.UserGlobal);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(systemAccessorDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return systemAccessorDefNode;
    }

    private IAstNode VisitMainNode(MainNode mainNode) {
        var ms = new ProcedureSymbol(Constants.WellKnownMainId, currentScope, NameSpace.System);
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
            Operator.Not => true,
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

    private static ISymbolType MapNodeToSymbolType(IAstNode node) {
        return node switch {
            IdentifierNode idn =>  new PendingResolveSymbol(idn.Id),
            TypeNode tn => MapNodeToSymbolType(tn.TypeName),
            NewInstanceNode nin => MapNodeToSymbolType(nin.Type),
            ValueNode vn => MapNodeToSymbolType(vn.TypeNode),
            ValueTypeNode vtn => MapElanValueTypeToSymbolType(vtn.Type.ToString()),
            LiteralTupleNode => new TupleSymbolType(),
            LiteralListNode => new ListSymbolType(),
            LiteralDictionaryNode => new DictionarySymbolType(),
            DataStructureTypeNode {Type:DataStructure.Array} => new ArraySymbolType(),
            DataStructureTypeNode {Type:DataStructure.List} => new ListSymbolType(),
            DataStructureTypeNode {Type:DataStructure.Dictionary} => new DictionarySymbolType(),
            MethodCallNode mcn => new PendingResolveSymbol(mcn.Name),
            BinaryNode { Operator: OperatorNode op } when OperatorEvaluatesToBoolean(op.Value)  => BoolSymbolType.Instance,
            BinaryNode bn => MapNodeToSymbolType(bn.Operand1),
            IndexedExpressionNode ien =>  MapNodeToSymbolType(ien.Expression),
            BracketNode bn => MapNodeToSymbolType(bn.BracketedNode),
            UnaryNode un => MapNodeToSymbolType(un.Operand),
            EnumValueNode en => MapNodeToSymbolType(en.TypeNode),
            DefaultNode dn => MapNodeToSymbolType(dn.Type),
            WithNode wn => MapNodeToSymbolType(wn.Expression),
            PropertyNode pn => MapNodeToSymbolType(pn.Expression),
            ReturnExpressionNode ren => MapNodeToSymbolType(ren.Expression),
            QualifiedNode qn => MapNodeToSymbolType(qn.Qualified),
            _ => throw new NotImplementedException()
        };
    }

    private IAstNode VisitVarDefNode(VarDefNode varDefNode) {
        if (varDefNode.Id is IdentifierNode idn) {
            var name =  idn.Id;
            var type = MapNodeToSymbolType(varDefNode.Expression);
            var ms = new VariableSymbol(name, type, currentScope);
            currentScope.Define(ms);
        }

        if (varDefNode.Id is DeconstructionNode dn) {
            var names = dn.ItemNodes.OfType<IdentifierNode>().Select(i => i.Id).ToArray();
            var types = names.Select((n, i) => new PendingTupleResolveSymbol(n, i + 1));
            var zip = names.Zip(types);

            foreach (var (name, type) in zip) {
                var ms = new VariableSymbol(name, type, currentScope);
                currentScope.Define(ms);
            }
        }

        VisitChildren(varDefNode);
        return varDefNode;
    }


    private IAstNode VisitParameterNode(ParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = MapNodeToSymbolType(parameterNode.TypeNode);

        var ms = new VariableSymbol(name, type, currentScope);
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