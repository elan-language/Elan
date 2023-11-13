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

        ISymbolType rt = null; // todo

        var ms = new FunctionSymbol(name, rt, currentScope, currentScope is GlobalScope ? NameSpace.UserGlobal : NameSpace.UserLocal);
        currentScope.Define(ms);
        currentScope = ms;
        VisitChildren(functionDefNode);
        currentScope = currentScope.EnclosingScope ?? throw new Exception("unexpected null scope");
        return functionDefNode;
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

    private static ISymbolType ConvertToBuiltInSymbol(string type) =>
        type switch {
            "Int" => IntSymbolType.Instance,
            "String" => StringSymbolType.Instance,
            "Float" => FloatSymbolType.Instance,
            "Decimal" => DecimalSymbolType.Instance,
            "Bool" => BooleanSymbolType.Instance,
            "Char" => CharSymbolType.Instance,
            "Tuple" => new TupleSymbolType(),
            "Array" => new ArraySymbolType(),
            "List" => new ListSymbolType(),
            "Dictionary" => new DictionarySymbolType(),
            _ when type.StartsWith("Pending:") => new ReturnResultSymbolType(type.Split(":").Last()),
            _ => new ClassSymbolType(type),
        };

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



    private static string GetTypeName(IAstNode node) {
        return node switch {
            IdentifierNode idn => $"Pending:{idn.Id}",
            TypeNode tn => GetTypeName(tn.TypeName),
            NewInstanceNode nin => GetTypeName(nin.Type),
            ValueNode vn => GetTypeName(vn.TypeNode),
            ValueTypeNode vtn => vtn.Type.ToString(),
            LiteralTupleNode => "Tuple",
            LiteralListNode => "List",
            LiteralDictionaryNode => "Dictionary",
            DataStructureTypeNode {Type:DataStructure.Array} => "Array",
            DataStructureTypeNode {Type:DataStructure.List} => "List",
            DataStructureTypeNode {Type:DataStructure.Dictionary} => "Dictionary",
            MethodCallNode mcn => $"Pending:{mcn.Name}",
            BinaryNode { Operator: OperatorNode op } when OperatorEvaluatesToBoolean(op.Value)  => "Bool",
            BinaryNode bn => GetTypeName(bn.Operand1),
            _ => ""
        };
    }

    private IAstNode VisitVarDefNode(VarDefNode varDefNode) {
        var name = varDefNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(varDefNode.Id.GetType().ToString());
        var type = ConvertToBuiltInSymbol(GetTypeName(varDefNode.Expression));

        var ms = new VariableSymbol(name, type, currentScope);
        currentScope.Define(ms);
        VisitChildren(varDefNode);
        return varDefNode;
    }

    private IAstNode VisitParameterNode(ParameterNode parameterNode) {
        var name = parameterNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(parameterNode.Id.GetType().ToString());
        var type = ConvertToBuiltInSymbol(GetTypeName(parameterNode.TypeNode));

        var ms = new VariableSymbol(name, type, currentScope);
        currentScope.Define(ms);
        VisitChildren(parameterNode);
        return parameterNode;
    }

    private IAstNode VisitPropertyDefNode(PropertyDefNode propertyNode) {
        var name = propertyNode.Id is IdentifierNode n ? n.Id : throw new NotImplementedException(propertyNode.Id.GetType().ToString());
        var type = ConvertToBuiltInSymbol(GetTypeName(propertyNode.Type));

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