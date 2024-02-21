using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree.Roles;
using CSharpLanguageModel.Models;
using SymbolTable;
using SymbolTable.Symbols;

namespace CSharpLanguageModel;

public class CodeModelAstVisitor : AbstractAstVisitor<ICodeModel> {
    public CodeModelAstVisitor(IScope currentScope) => CurrentScope = currentScope;

    private IScope CurrentScope { get; set; }

    protected override void Enter(IAstNode node) {
        if (node is IHasScope hs) {
            CurrentScope = CurrentScope.Resolve(hs.Name) as IScope ?? throw new NullReferenceException();
        }
    }

    protected override void Exit(IAstNode node) {
        if (node is IHasScope) {
            CurrentScope = CurrentScope.EnclosingScope ?? throw new NullReferenceException();
        }
    }

    public override ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            AbstractClassDefNode n => HandleScope(Build, n),
            AbstractFunctionDefNode n => HandleScope(Build, n),
            AbstractProcedureDefNode n => HandleScope(Build, n),
            AssertNode n => HandleScope(Build, n),
            AssignmentNode n => HandleScope(Build, n),
            BinaryNode n => HandleScope(Build, n),
            BracketNode n => HandleScope(Build, n),
            CallStatementNode n => HandleScope(Build, n),
            CaseNode n => HandleScope(Build, n),
            ClassDefNode n => HandleScope(Build, n),
            ConstantDefNode n => HandleScope(Build, n),
            ConstructorNode n => HandleScope(Build, n),
            DataStructureTypeNode n => HandleScope(Build, n),
            DeconstructionNode n => HandleScope(Build, n),
            DefaultNode n => HandleScope(Build, n),
            EachParameterNode n => HandleScope(Build, n),
            EachStatementNode n => HandleScope(Build, n),
            EnumDefNode n => HandleScope(Build, n),
            EnumValueNode n => HandleScope(Build, n),
            FileNode n => HandleScope(Build, n),
            ForStatementNode n => HandleScope(Build, n),
            FunctionCallNode n => HandleScope(Build, n),
            FunctionDefNode n => HandleScope(Build, n),
            GlobalPrefixNode n => HandleScope(Build, n),
            IScalarValueNode n => HandleScope(Build, n),
            IdentifierNode n => HandleScope(Build, n),
            IfExpressionNode n => HandleScope(Build, n),
            IfStatementNode n => HandleScope(Build, n),
            IndexedExpressionNode n => HandleScope(Build, n),
            InputNode n => HandleScope(Build, n),
            LambdaDefNode n => HandleScope(Build, n),
            LambdaTypeNode n => HandleScope(Build, n),
            LibraryNode n => HandleScope(Build, n),
            LiteralDictionaryNode n => HandleScope(Build, n),
            LiteralListNode n => HandleScope(Build, n),
            LiteralTupleNode n => HandleScope(Build, n),
            MainNode n => HandleScope(Build, n),
            MethodSignatureNode n => HandleScope(Build, n),
            NewInstanceNode n => HandleScope(Build, n),
            OperatorNode n => HandleScope(Build, n),
            PairNode n => HandleScope(Build, n),
            ParameterCallNode n => HandleScope(Build, n),
            ParameterNode n => HandleScope(Build, n),
            PrintNode n => HandleScope(Build, n),
            ProcedureCallNode n => HandleScope(Build, n),
            ProcedureDefNode n => HandleScope(Build, n),
            PropertyCallNode n => HandleScope(Build, n),
            PropertyDefNode n => HandleScope(Build, n),
            PropertyPrefixNode n => HandleScope(Build, n),
            QualifiedNode n => HandleScope(Build, n),
            RangeExpressionNode n => HandleScope(Build, n),
            RepeatStatementNode n => HandleScope(Build, n),
            ReturnExpressionNode n => HandleScope(Build, n),
            StatementBlockNode n => HandleScope(Build, n),
            SwitchStatementNode n => HandleScope(Build, n),
            SystemAccessorCallNode n => HandleScope(Build, n),
            TestDefNode n => HandleScope(Build, n),
            ThisInstanceNode n => HandleScope(Build, n),
            ThrowNode n => HandleScope(Build, n),
            TryCatchNode n => HandleScope(Build, n),
            TupleTypeNode n => HandleScope(Build, n),
            TupleDefNode n => HandleScope(Build, n),
            TwoDIndexExpressionNode n => HandleScope(Build, n),
            TypeNode n => HandleScope(Build, n),
            UnaryNode n => HandleScope(Build, n),
            ValueTypeNode n => HandleScope(Build, n),
            VarDefNode n => HandleScope(Build, n),
            WithNode n => HandleScope(Build, n),
            WhileStatementNode n => HandleScope(Build, n),

            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private FileCodeModel Build(FileNode fileNode) {
        var main = fileNode.MainNode is { } mn ? Visit(mn) : null;
        var globals = fileNode.GlobalNodes.Select(Visit);
        var tests = fileNode.TestNodes.Select(Visit);
        return new FileCodeModel(globals.ToArray(), main, tests.ToArray());
    }

    private MainCodeModel Build(MainNode mainNode) => new(Visit(mainNode.StatementBlock));

    private StatementBlockModel Build(StatementBlockNode statementBlock) => new(statementBlock.Statements.Select(Visit).ToArray());

    private ICodeModel Build(ReturnExpressionNode n) => Visit(n.Expression);

    private ICodeModel Build(AbstractProcedureDefNode n) => Visit(n.Signature);

    private ICodeModel Build(AbstractFunctionDefNode n) => Visit(n.Signature);

    private VarDefModel Build(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Rhs));

    private ConstantDefModel Build(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToPrefixedCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel Build(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Rhs), assignmentNode.Inline);

   

    private FunctionOrSystemCallModel Build(SystemAccessorCallNode systemAccessorCallNode) {
        var symbol = CurrentScope.Resolve(systemAccessorCallNode.Name) as SystemAccessorSymbol;
        var qNode = NameSpaceToNode(symbol?.NameSpace) ?? systemAccessorCallNode.Qualifier;
        var qModel = qNode is { } q ? Visit(q) : null;
        return new FunctionOrSystemCallModel(Visit(systemAccessorCallNode.Id), qModel, systemAccessorCallNode.Parameters.Select(Visit).ToArray());
    }

    private static IAstNode? NameSpaceToNode(NameSpace? ns) => ns switch {
        NameSpace.System => new LibraryNode("StandardLibrary.SystemAccessors", 0, 0),
        NameSpace.LibraryFunction => new LibraryNode("StandardLibrary.Functions", 0, 0),
        NameSpace.LibraryProcedure => new LibraryNode("StandardLibrary.Procedures", 0, 0),
        NameSpace.UserGlobal => new GlobalPrefixNode(0, 0),
        _ => null
    };

    private (ICodeModel?, ICodeModel[]) GetQualifierAndParameters(ICallNode callNode) {
        var symbol = SymbolHelpers.ResolveCall(callNode, CurrentScope);
        var parameters = callNode.Parameters.Select(Visit);
        NameSpace? nameSpace;
        IAstNode? qualifierNode;

        if (callNode is ProcedureCallNode) {
            nameSpace = symbol is ProcedureSymbol ps ? ps.NameSpace : null;
            qualifierNode = NameSpaceToNode(nameSpace) ?? callNode.Qualifier;
        }
        else {
            nameSpace = symbol is MethodSymbol ms ? ms.NameSpace : NameSpace.UserLocal;
            qualifierNode = callNode.Qualifier ?? NameSpaceToNode(nameSpace);
        }

        var qualifierModel = qualifierNode is not null ? Visit(qualifierNode) : null;
        var calledOnNode = callNode.CalledOn;
        var calledOnModel = calledOnNode is not null ? Visit(calledOnNode) : null;

        if (qualifierNode is null && calledOnNode is not null && nameSpace is NameSpace.UserLocal) {
            qualifierModel = Visit(calledOnNode);
        }
        else if (calledOnModel is not null) {
            parameters = parameters.Prepend(calledOnModel);
        }
        else if (qualifierModel is ScalarValueModel { Value : "this" } qm) {
            parameters = parameters.Prepend(qm);
            qualifierModel = Visit(NameSpaceToNode(nameSpace)!);
        }

        return (qualifierModel, parameters.ToArray());
    }


    private ProcedureCallModel Build(ProcedureCallNode procedureCallNode) {
        var (qModel, parameters) = GetQualifierAndParameters(procedureCallNode);
        return new ProcedureCallModel(Visit(procedureCallNode.Id), qModel, parameters.ToArray());
    }

    private FunctionOrSystemCallModel Build(FunctionCallNode functionCallNode) {
        var (qModel, parameters) = GetQualifierAndParameters(functionCallNode);
        return new FunctionOrSystemCallModel(Visit(functionCallNode.Id), qModel, parameters);
    }

    private static ScalarValueModel Build(IScalarValueNode scalarValueNode) => new(scalarValueNode.Value);

    private static IdentifierModel Build(IdentifierNode identifierNode) => new(identifierNode.Id);

    private IfStatementModel Build(IfStatementNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        var statementBlocks = ifStatementNode.StatementBlocks.Select(Visit);

        return new IfStatementModel(expressions.ToArray(), statementBlocks.ToArray());
    }

    private IfExpressionModel Build(IfExpressionNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        return new IfExpressionModel(expressions.ToArray());
    }

    private SwitchStatementModel Build(SwitchStatementNode switchStatementNode) {
        var expression = Visit(switchStatementNode.Expression);
        var cases = switchStatementNode.Cases.Select(Visit);
        var defaultCase = Visit(switchStatementNode.DefaultCase);

        return new SwitchStatementModel(expression, cases.ToArray(), defaultCase);
    }

    private ForStatementModel Build(ForStatementNode forStatementNode) {
        var id = Visit(forStatementNode.Id);
        var expressions = forStatementNode.Expressions.Select(Visit);
        var statementBlock = Visit(forStatementNode.StatementBlock);
        var step = forStatementNode.Step is { } i ? Visit(i) : null;
        var neg = forStatementNode.Neg;

        return new ForStatementModel(id, expressions.ToArray(), step, neg, statementBlock);
    }

    private EachStatementModel Build(EachStatementNode eachStatementNode) {
        var parameter = Visit(eachStatementNode.Parameter);
        var statementBlock = Visit(eachStatementNode.StatementBlock);

        return new EachStatementModel(parameter, statementBlock);
    }

    private WhileStatementModel Build(WhileStatementNode whileStatementNode) {
        var expression = Visit(whileStatementNode.Expression);
        var statementBlock = Visit(whileStatementNode.StatementBlock);

        return new WhileStatementModel(expression, statementBlock);
    }

    private RepeatStatementModel Build(RepeatStatementNode repeatStatementNode) {
        var expression = Visit(repeatStatementNode.Expression);
        var statementBlock = Visit(repeatStatementNode.StatementBlock);

        return new RepeatStatementModel(expression, statementBlock);
    }

    private LiteralListModel Build(LiteralListNode literalListNode) {
        var type = CodeHelpers.NodeToCSharpType(literalListNode.ItemNodes.First(), CurrentScope);
        var items = literalListNode.ItemNodes.Select(Visit);

        return new LiteralListModel(items.ToArray(), type);
    }

    private TupleModel Build(LiteralTupleNode literalListNode) {
        var items = literalListNode.ItemNodes.Select(Visit);

        return new TupleModel(items.ToArray());
    }

    private LiteralDictionaryModel Build(LiteralDictionaryNode literalDictionaryNode) {
        var type = CodeHelpers.NodeToCSharpType(literalDictionaryNode.ItemNodes.First(), CurrentScope);
        var items = literalDictionaryNode.ItemNodes.Select(Visit);

        return new LiteralDictionaryModel(items.ToArray(), type);
    }

    private static ICodeModel Build(OperatorNode operatorNode) {
        var (value, isFunc) = CodeHelpers.OperatorToCSharpOperator(operatorNode.Value);
        return isFunc ? new IdentifierModel(value) : new ScalarValueModel(value);
    }

    private BracketModel Build(BracketNode bracketNode) => new(Visit(bracketNode.BracketedNode));

    private CallStatementModel Build(CallStatementNode callStatementNode) => new(Visit(callStatementNode.CallNode));

    private BinaryModel Build(BinaryNode binaryNode) => new(Visit(binaryNode.Operator), Visit(binaryNode.Operand1), Visit(binaryNode.Operand2));

    private UnaryModel Build(UnaryNode binaryNode) => new(Visit(binaryNode.Operator), Visit(binaryNode.Operand));

    private IndexedExpressionModel Build(IndexedExpressionNode indexedExpressionNode) {
        var expression = Visit(indexedExpressionNode.Expression);
        var range = Visit(indexedExpressionNode.Range);

        return new IndexedExpressionModel(expression, range);
    }

    private RangeExpressionModel Build(RangeExpressionNode rangeExpressionNode) {
        var expression1 = Visit(rangeExpressionNode.Expression1);
        var expression2 = rangeExpressionNode.Expression2 is { } e ? Visit(e) : null;

        return new RangeExpressionModel(rangeExpressionNode.Prefix, expression1, expression2);
    }

    private NewInstanceModel Build(NewInstanceNode newInstanceNode) {
        var type = Visit(newInstanceNode.Type);
        var args = newInstanceNode.Arguments.Select(Visit);

        return new NewInstanceModel(type, args.ToArray());
    }

    private DataStructureTypeModel Build(DataStructureTypeNode dataStructureTypeNode) {
        var types = dataStructureTypeNode.GenericTypes.Select(Visit);

        return new DataStructureTypeModel(types.ToArray(), CodeHelpers.DataStructureTypeToCSharpType(dataStructureTypeNode.Type));
    }

    private static ScalarValueModel Build(ValueTypeNode valueTypeNode) => new(CodeHelpers.ValueTypeToCSharpType(valueTypeNode.Type));

    private TwoDIndexModel Build(TwoDIndexExpressionNode twoDIndexExpressionNode) => new(Visit(twoDIndexExpressionNode.Expression1), Visit(twoDIndexExpressionNode.Expression2));

    private ProcedureDefModel Build(ProcedureDefNode procedureDefNode) {
        var signature = Visit(procedureDefNode.Signature);
        var statementBlock = Visit(procedureDefNode.StatementBlock);

        return new ProcedureDefModel(signature, statementBlock, procedureDefNode.Standalone);
    }

    private FunctionDefModel Build(FunctionDefNode functionDefNode) {
        var signature = Visit(functionDefNode.Signature);
        var statementBlock = Visit(functionDefNode.StatementBlock);
        var ret = Visit(functionDefNode.Return);

        return new FunctionDefModel(signature, statementBlock, ret, functionDefNode.Standalone);
    }

    private LambdaDefModel Build(LambdaDefNode lambdaDefNode) {
        var expr = Visit(lambdaDefNode.Expression);
        var arguments = lambdaDefNode.Arguments.Select(Visit);

        return new LambdaDefModel(arguments.ToArray(), expr);
    }

    private MethodSignatureModel Build(MethodSignatureNode methodSignatureNode) {
        var id = Visit(methodSignatureNode.Id);
        var parameters = methodSignatureNode.Parameters.Select(Visit);
        var returnType = methodSignatureNode.ReturnType is { } rt ? Visit(rt) : null;
        return new MethodSignatureModel(id, parameters.ToArray(), returnType);
    }

    private ParameterModel Build(ParameterNode parameterNode) {
        var id = Visit(parameterNode.Id);
        var type = Visit(parameterNode.TypeNode);
        var isRef = parameterNode.IsRef;

        return new ParameterModel(id, type, isRef);
    }

    private EachParameterModel Build(EachParameterNode parameterNode) {
        var id = Visit(parameterNode.Id);
        var expression = Visit(parameterNode.Expression);

        return new EachParameterModel(id, expression);
    }

    private CaseModel Build(CaseNode caseNode) {
        var id = caseNode.Value is not null ? Visit(caseNode.Value) : null;
        var type = Visit(caseNode.StatementBlock);

        return new CaseModel(id, type);
    }

    private TryCatchModel Build(TryCatchNode tryCatchNode) {
        var id = Visit(tryCatchNode.Id);
        var triedCode = Visit(tryCatchNode.TriedCode);
        var caughtCode = Visit(tryCatchNode.CaughtCode);

        return new TryCatchModel(triedCode, id, caughtCode);
    }

    private PropertyModel Build(PropertyCallNode propertyCallNode) {
        var property = Visit(propertyCallNode.Property);
        var expression = Visit(propertyCallNode.Expression);

        return new PropertyModel(expression, property);
    }

    private EnumDefModel Build(EnumDefNode enumDefNode) {
        var type = Visit(enumDefNode.Type);
        var values = enumDefNode.Values.Select(Visit);

        return new EnumDefModel(type, values.ToArray());
    }

    private EnumValueModel Build(EnumValueNode enumValueNode) {
        var type = Visit(enumValueNode.TypeNode);
        var value = enumValueNode.Value;

        return new EnumValueModel(value, type);
    }

    private PairModel Build(PairNode pairNode) {
        var key = Visit(pairNode.Key);
        var value = Visit(pairNode.Value);

        return new PairModel(key, value);
    }

    private ClassDefModel Build(ClassDefNode classDefNode) {
        var type = Visit(classDefNode.Type);
        var inherits = classDefNode.Inherits.Select(Visit).ToArray();
        var constructor = Visit(classDefNode.Constructor);
        var properties = classDefNode.Properties.Select(Visit).ToArray();
        var functions = classDefNode.Methods.Select(Visit).ToArray();

        if (classDefNode.Immutable) {
            properties = properties.OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Immutable }).Cast<ICodeModel>().ToArray();
        }

        var pSignatures = classDefNode.Methods.OfType<ProcedureDefNode>().Select(n => n.Signature).Select(Visit).ToArray();

        var dProperties = properties.OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Default }).Cast<ICodeModel>().ToArray();

        var defaultClassModel = new DefaultClassDefModel(type, dProperties, pSignatures, Array.Empty<ICodeModel>(), classDefNode.HasAsString, false);

        return new ClassDefModel(type, inherits, constructor, properties, functions, classDefNode.HasDefaultConstructor, defaultClassModel);
    }

    private AbstractClassDefModel Build(AbstractClassDefNode abstractClassDefNode) {
        var type = Visit(abstractClassDefNode.Type);
        var inherits = abstractClassDefNode.Inherits.Select(Visit).ToArray();
        var properties = abstractClassDefNode.Properties.Select(Visit).OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Abstract }).ToArray();
        var methodSignatures = abstractClassDefNode.Methods.Select(Visit).ToArray();

        var dProperties = properties.Select(p => p with { PropertyType = PropertyType.AbstractDefault }).Cast<ICodeModel>().ToArray();

        var dProcedureSignatures = methodSignatures.OfType<MethodSignatureModel>().Where(ms => ms.ReturnType is null).Cast<ICodeModel>().ToArray();
        var dFunctionSignatures = methodSignatures.OfType<MethodSignatureModel>().Where(ms => ms.ReturnType is not null).Cast<ICodeModel>().ToArray();

        var defaultClassModel = new DefaultClassDefModel(type, dProperties, dProcedureSignatures, dFunctionSignatures, false, true);

        return new AbstractClassDefModel(type, inherits, properties.Cast<ICodeModel>().ToArray(), methodSignatures, defaultClassModel);
    }

    private ConstructorModel Build(ConstructorNode constructorNode) {
        var body = Visit(constructorNode.StatementBlock);
        var parameters = constructorNode.Parameters.Select(Visit);

        return new ConstructorModel(parameters.ToArray(), body);
    }

    private PropertyDefModel Build(PropertyDefNode propertyDefNode) {
        var id = Visit(propertyDefNode.Id);
        var type = Visit(propertyDefNode.Type);

        return new PropertyDefModel(id, type, PropertyType.Mutable, propertyDefNode.IsPrivate);
    }

    private TypeModel Build(TypeNode typeNode) {
        var id = Visit(typeNode.TypeName);

        return new TypeModel(id);
    }

    private FuncTypeModel Build(LambdaTypeNode typeNode) {
        var types = typeNode.Types.Select(Visit);
        var ret = Visit(typeNode.ReturnType);

        return new FuncTypeModel(types.ToArray(), ret);
    }

    private TupleModel Build(TupleTypeNode typeNode) {
        var types = typeNode.Types.Select(Visit);

        return new TupleModel(types.ToArray());
    }

    private TupleModel Build(TupleDefNode typeNode) {
        var types = typeNode.Expressions.Select(Visit);

        return new TupleModel(types.ToArray());
    }

    private static ScalarValueModel Build(PropertyPrefixNode selfPrefixNode) => new("this");

    private static ScalarValueModel Build(ThisInstanceNode thisInstanceNode) => new("this");

    private static ScalarValueModel Build(GlobalPrefixNode globalPrefixNode) => new("Globals");

    private static ScalarValueModel Build(LibraryNode l) => new(l.Type);

    private QualifiedValueModel Build(QualifiedNode qualifiedNode) => new(Visit(qualifiedNode.Qualifier), Visit(qualifiedNode.Qualified));

    private DefaultModel Build(DefaultNode defaultNode) {
        var id = Visit(defaultNode.Type);

        return new DefaultModel(id, defaultNode.TypeType);
    }

    private WithModel Build(WithNode withNode) {
        var expr = Visit(withNode.Expression);
        var assignments = withNode.AssignmentNodes.Select(Visit);

        return new WithModel(expr, assignments.ToArray());
    }

    private DeconstructionModel Build(DeconstructionNode defaultNode) {
        var ids = defaultNode.ItemNodes.Select(Visit);
        var isNew = defaultNode.IsNew;
        return new DeconstructionModel(ids.ToArray(), isNew);
    }

    private ThrowModel Build(ThrowNode defaultNode) {
        var thrown = Visit(defaultNode.Thrown);
        return new ThrowModel(thrown);
    }

    private PrintModel Build(PrintNode defaultNode) {
        var thrown = defaultNode.Expression is { } e ? Visit(e) : null;
        return new PrintModel(thrown);
    }

    private InputModel Build(InputNode defaultNode) {
        var thrown = defaultNode.Prompt;
        return new InputModel(thrown);
    }

    private ParameterCallModel Build(ParameterCallNode defaultNode) => new(Visit(defaultNode.Expression), defaultNode.IsRef);

    private TestModel Build(TestDefNode testDefNode) {
        var statements = Visit(testDefNode.TestStatements);
        var id = Visit(testDefNode.Id);

        return new TestModel(id, statements);
    }

    private AssertModel Build(AssertNode assertNode) {
        var actual = Visit(assertNode.Actual);
        var expected = Visit(assertNode.Expected);
        return new AssertModel(actual, expected);
    }
}