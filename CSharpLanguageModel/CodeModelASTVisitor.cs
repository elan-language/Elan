using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using AbstractSyntaxTree.Roles;
using CSharpLanguageModel.Models;
using StandardLibrary;
using SymbolTable;
using SymbolTable.Symbols;

namespace CSharpLanguageModel;

public class CodeModelAstVisitor : AbstractAstVisitor<ICodeModel> {

    public CodeModelAstVisitor(IScope currentScope) {
        CurrentScope = currentScope;
    }


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

    private FileCodeModel BuildFileModel(FileNode fileNode) {
        var main = fileNode.MainNode is { } mn ? Visit(mn) : null;
        var globals = fileNode.GlobalNodes.Select(Visit);
        return new FileCodeModel(globals.ToArray(), main);
    }

    private MainCodeModel BuildMainModel(MainNode mainNode) => new(Visit(mainNode.StatementBlock));

    private StatementBlockModel BuildStatementBlockModel(StatementBlockNode statementBlock) => new(statementBlock.Statements.Select(Visit).ToArray());

    public override ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            FileNode n => HandleScope(BuildFileModel, n),
            MainNode n => HandleScope(BuildMainModel, n),
            SystemAccessorCallNode n => HandleScope(BuildSystemAccessorModel, n),
            ProcedureCallNode n => HandleScope(BuildProcedureCallModel, n),
            FunctionCallNode n => HandleScope(BuildFunctionCallModel, n),
            IScalarValueNode n => HandleScope(BuildScalarValueModel, n),
            VarDefNode n => HandleScope(BuildVarDefModel, n),
            ConstantDefNode n => HandleScope(BuildConstantDefModel, n),
            AssignmentNode n => HandleScope(BuildAssignmentModel, n),
            IdentifierNode n => HandleScope(BuildIdentifierModel, n),
            BinaryNode n => HandleScope(BuildBinaryModel, n),
            UnaryNode n => HandleScope(BuildUnaryModel, n),
            OperatorNode n => HandleScope(BuildOperatorModel, n),
            IfStatementNode n => HandleScope(BuildIfStatementModel, n),
            IfExpressionNode n => HandleScope(BuildIfExpressionModel, n),
            WhileStatementNode n => HandleScope(BuildWhileStatementModel, n),
            RepeatStatementNode n => HandleScope(BuildRepeatStatementModel, n),
            StatementBlockNode n => HandleScope(BuildStatementBlockModel, n),
            BracketNode n => HandleScope(BuildBracketModel, n),
            CallStatementNode n => HandleScope(BuildCallStatementModel, n),
            LiteralListNode n => HandleScope(BuildLiteralListModel, n),
            LiteralTupleNode n => HandleScope(BuildLiteralTupleModel, n),
            LiteralDictionaryNode n => HandleScope(BuildLiteralDictionaryModel, n),
            IndexedExpressionNode n => HandleScope(BuildIndexedExpressionModel, n),
            ItemizedExpressionNode n => HandleScope(BuildItemizedExpressionModel, n),
            RangeExpressionNode n => HandleScope(BuildRangeExpressionModel, n),
            NewInstanceNode n => HandleScope(BuildNewInstanceModel, n),
            DataStructureTypeNode n => HandleScope(BuildDataStructureTypeModel, n),
            ValueTypeNode n => HandleScope(BuildValueTypeModel, n),
            ForStatementNode n => HandleScope(BuildForStatementModel, n),
            ForEachStatementNode n => HandleScope(BuildForInStatementModel, n),
            TwoDIndexExpressionNode n => HandleScope(Build2DIndexModel, n),
            ProcedureDefNode n => HandleScope(BuildProcedureDefModel, n),
            FunctionDefNode n => HandleScope(BuildFunctionDefModel, n),
            LambdaDefNode n => HandleScope(BuildLambdaDefModel, n),
            SystemAccessorDefNode n => HandleScope(BuildSystemAccessorDefModel, n),
            MethodSignatureNode n => HandleScope(BuildMethodSignatureModel, n),
            ParameterNode n => HandleScope(BuildParameterModel, n),
            SwitchStatementNode n => HandleScope(BuildSwitchStatementModel, n),
            CaseNode n => HandleScope(BuildCaseModel, n),
            TryCatchNode n => HandleScope(BuildTryCatchModel, n),
            PropertyCallNode n => HandleScope(BuildPropertyModel, n),
            EnumDefNode n => HandleScope(BuildEnumDefModel, n),
            EnumValueNode n => HandleScope(BuildEnumValueModel, n),
            PairNode n => HandleScope(BuildKvpModel, n),
            ClassDefNode n => HandleScope(BuildClassDefModel, n),
            AbstractClassDefNode n => HandleScope(BuildAbstractClassDefModel, n),
            ConstructorNode n => HandleScope(BuildConstructorModel, n),
            PropertyDefNode n => HandleScope(BuildPropertyDefModel, n),
            TypeNode n => HandleScope(BuildTypeModel, n),
            PropertyPrefixNode n => HandleScope(BuildPropertyPrefixModel, n),
            ThisInstanceNode n => HandleScope(BuildThisInstanceModel, n),
            GlobalPrefixNode n => HandleScope(BuildGlobalModel, n),
            LibraryNode n => HandleScope(BuildNamespaceModel, n),
            ReturnExpressionNode n => HandleScope(BuildReturnExpressionModel, n),
            QualifiedNode n => HandleScope(BuildQualifiedModel, n),
            DefaultNode n => HandleScope(BuildDefaultModel, n),
            WithNode n => HandleScope(BuildWithModel, n),
            DeconstructionNode n => HandleScope(BuildDeconstructionModel, n),
            ThrowNode n => HandleScope(BuildThrowModel, n),
            PrintNode n => HandleScope(BuildPrintModel, n),
            InputNode n => HandleScope(BuildInputModel, n),
            ParameterCallNode n => HandleScope(BuildParameterCallModel, n),
            LambdaTypeNode n => HandleScope(BuildFuncTypeModel, n),
            TupleTypeNode n => HandleScope(BuildTupleTypeModel, n),
            TupleDefNode n => HandleScope(BuildTupleDefModel, n),
            AbstractFunctionDefNode n => HandleScope(BuildAbstractFunctionDefModel,n),
            AbstractProcedureDefNode n => HandleScope(BuildAbstractProcedureDefModel, n),
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private ICodeModel BuildReturnExpressionModel(ReturnExpressionNode n) => Visit(n.Expression);

    private ICodeModel BuildAbstractProcedureDefModel(AbstractProcedureDefNode n) => Visit(n.Signature);

    private ICodeModel BuildAbstractFunctionDefModel(AbstractFunctionDefNode n) => Visit(n.Signature);

    private VarDefModel BuildVarDefModel(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Rhs));

    private ConstantDefModel BuildConstantDefModel(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToPrefixedCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel BuildAssignmentModel(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Rhs), assignmentNode.Inline);

    private ProcedureCallModel BuildProcedureCallModel(ProcedureCallNode procedureCallNode) {
        var symbol = SymbolHelpers.ResolveCall(procedureCallNode, CurrentScope) as ProcedureSymbol;
        var parameters = procedureCallNode.Parameters.Select(Visit);
        var qNode = NameSpaceToNode(symbol?.NameSpace) ?? procedureCallNode.Qualifier;
        var qModel = qNode is { } q ? Visit(q) : null;

        // TODO cloned code fix once working

        var calledOnNode = procedureCallNode.CalledOn;
        var calledOnModel = calledOnNode is { } con ? Visit(con) : null;

        if (qNode is null && symbol?.NameSpace is NameSpace.UserLocal) {
            qModel = Visit(calledOnNode!);
        }
        else if (calledOnModel is not null) {
            parameters = parameters.Prepend(calledOnModel);
        }

        return new ProcedureCallModel(Visit(procedureCallNode.Id), qModel, parameters.ToArray());
    }

    private MethodCallModel BuildSystemAccessorModel(SystemAccessorCallNode systemAccessorCallNode) {
        var symbol = CurrentScope.Resolve(systemAccessorCallNode.Name) as SystemAccessorSymbol;
        var qNode = NameSpaceToNode(symbol?.NameSpace) ?? systemAccessorCallNode.Qualifier;
        var qModel = qNode is { } q ? Visit(q) : null;
        return new MethodCallModel(CodeHelpers.MethodType.SystemCall, Visit(systemAccessorCallNode.Id), qModel, systemAccessorCallNode.Parameters.Select(Visit).ToArray());
    }

    private static IAstNode? NameSpaceToNode(NameSpace? ns) => ns switch {
        NameSpace.System => new LibraryNode("StandardLibrary.SystemAccessors"),
        NameSpace.LibraryFunction => new LibraryNode("StandardLibrary.Functions"),
        NameSpace.LibraryProcedure => new LibraryNode("StandardLibrary.Procedures"),
        NameSpace.UserGlobal => new GlobalPrefixNode(),
        _ => null
    };

    private MethodCallModel BuildFunctionCallModel(FunctionCallNode functionCallNode) {
        var symbol = SymbolHelpers.ResolveCall(functionCallNode, CurrentScope) as FunctionSymbol;
        var qNode =  functionCallNode.Qualifier ?? NameSpaceToNode(symbol?.NameSpace);
        var qModel = qNode is { } q ? Visit(q) : null;

        // TODO cloned code fix once working

        var calledOnNode = functionCallNode.CalledOn;
        var calledOnModel = calledOnNode is { } con ? Visit(con) : null;

        var parameters = functionCallNode.Parameters.Select(Visit);

        if (qNode is null && calledOnNode is not null && symbol?.NameSpace is NameSpace.UserLocal) {

            qModel = Visit(calledOnNode);

        }
        else if (calledOnModel is not null) {
            parameters = parameters.Prepend(calledOnModel);
        }
        else if (qModel is ScalarValueModel {Value : "this"} qm) {
            parameters = parameters.Prepend(qm);
            qModel = Visit(NameSpaceToNode(symbol?.NameSpace));
        }


        return new MethodCallModel(CodeHelpers.MethodType.Function, Visit(functionCallNode.Id), qModel, parameters.ToArray());
    }

    private static ScalarValueModel BuildScalarValueModel(IScalarValueNode scalarValueNode) => new(scalarValueNode.Value);

    private static IdentifierModel BuildIdentifierModel(IdentifierNode identifierNode) => new(identifierNode.Id);

    private IfStatementModel BuildIfStatementModel(IfStatementNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        var statementBlocks = ifStatementNode.StatementBlocks.Select(Visit);

        return new IfStatementModel(expressions.ToArray(), statementBlocks.ToArray());
    }

    private IfExpressionModel BuildIfExpressionModel(IfExpressionNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        return new IfExpressionModel(expressions.ToArray());
    }

    private SwitchStatementModel BuildSwitchStatementModel(SwitchStatementNode switchStatementNode) {
        var expression = Visit(switchStatementNode.Expression);
        var cases = switchStatementNode.Cases.Select(Visit);
        var defaultCase = Visit(switchStatementNode.DefaultCase);

        return new SwitchStatementModel(expression, cases.ToArray(), defaultCase);
    }

    private ForStatementModel BuildForStatementModel(ForStatementNode forStatementNode) {
        var id = Visit(forStatementNode.Id);
        var expressions = forStatementNode.Expressions.Select(Visit);
        var statementBlock = Visit(forStatementNode.StatementBlock);
        var step = forStatementNode.Step is { } i ? Visit(i) : null;
        var neg = forStatementNode.Neg;

        return new ForStatementModel(id, expressions.ToArray(), step, neg, statementBlock);
    }

    private ForInStatementModel BuildForInStatementModel(ForEachStatementNode forEachStatementNode) {
        var id = Visit(forEachStatementNode.Id);
        var expression = Visit(forEachStatementNode.Expression);
        var statementBlock = Visit(forEachStatementNode.StatementBlock);

        return new ForInStatementModel(id, expression, statementBlock);
    }

    private WhileStatementModel BuildWhileStatementModel(WhileStatementNode whileStatementNode) {
        var expression = Visit(whileStatementNode.Expression);
        var statementBlock = Visit(whileStatementNode.StatementBlock);

        return new WhileStatementModel(expression, statementBlock);
    }

    private RepeatStatementModel BuildRepeatStatementModel(RepeatStatementNode repeatStatementNode) {
        var expression = Visit(repeatStatementNode.Expression);
        var statementBlock = Visit(repeatStatementNode.StatementBlock);

        return new RepeatStatementModel(expression, statementBlock);
    }

    private LiteralListModel BuildLiteralListModel(LiteralListNode literalListNode) {
        var type = CodeHelpers.NodeToCSharpType(literalListNode.ItemNodes.First());
        var items = literalListNode.ItemNodes.Select(Visit);

        return new LiteralListModel(items.ToArray(), type);
    }

    private TupleModel BuildLiteralTupleModel(LiteralTupleNode literalListNode) {
        var items = literalListNode.ItemNodes.Select(Visit);

        return new TupleModel(items.ToArray());
    }

    private LiteralDictionaryModel BuildLiteralDictionaryModel(LiteralDictionaryNode literalDictionaryNode) {
        var type = CodeHelpers.NodeToCSharpType(literalDictionaryNode.ItemNodes.First());
        var items = literalDictionaryNode.ItemNodes.Select(Visit);

        return new LiteralDictionaryModel(items.ToArray(), type);
    }

    private static ICodeModel BuildOperatorModel(OperatorNode operatorNode) {
        var (value, isFunc) = CodeHelpers.OperatorToCSharpOperator(operatorNode.Value);
        return isFunc ? new IdentifierModel(value) : new ScalarValueModel(value);
    }

    private BracketModel BuildBracketModel(BracketNode bracketNode) => new(Visit(bracketNode.BracketedNode));

    private CallStatementModel BuildCallStatementModel(CallStatementNode callStatementNode) => new(Visit(callStatementNode.CallNode));

    private BinaryModel BuildBinaryModel(BinaryNode binaryNode) => new(Visit(binaryNode.Operator), Visit(binaryNode.Operand1), Visit(binaryNode.Operand2));

    private UnaryModel BuildUnaryModel(UnaryNode binaryNode) => new(Visit(binaryNode.Operator), Visit(binaryNode.Operand));

    private IndexedExpressionModel BuildIndexedExpressionModel(IndexedExpressionNode indexedExpressionNode) {
        var expression = Visit(indexedExpressionNode.Expression);
        var range = Visit(indexedExpressionNode.Range);

        return new IndexedExpressionModel(expression, range);
    }

    private ItemizedExpressionModel BuildItemizedExpressionModel(ItemizedExpressionNode itemizedExpressionNode) {
        var expression = Visit(itemizedExpressionNode.Expression);
        var range = Visit(itemizedExpressionNode.Range);

        return new ItemizedExpressionModel(expression, range);
    }

    private RangeExpressionModel BuildRangeExpressionModel(RangeExpressionNode rangeExpressionNode) {
        var expression1 = Visit(rangeExpressionNode.Expression1);
        var expression2 = rangeExpressionNode.Expression2 is { } e ? Visit(e) : null;

        return new RangeExpressionModel(rangeExpressionNode.Prefix, expression1, expression2);
    }

    private NewInstanceModel BuildNewInstanceModel(NewInstanceNode newInstanceNode) {
        var type = Visit(newInstanceNode.Type);
        var args = newInstanceNode.Arguments.Select(Visit);

        return new NewInstanceModel(type, args.ToArray());
    }

    private DataStructureTypeModel BuildDataStructureTypeModel(DataStructureTypeNode dataStructureTypeNode) {
        var types = dataStructureTypeNode.GenericTypes.Select(Visit);

        return new DataStructureTypeModel(types.ToArray(), CodeHelpers.DataStructureTypeToCSharpType(dataStructureTypeNode.Type));
    }

    private static ScalarValueModel BuildValueTypeModel(ValueTypeNode valueTypeNode) => new(CodeHelpers.ValueTypeToCSharpType(valueTypeNode.Type));

    private TwoDIndexModel Build2DIndexModel(TwoDIndexExpressionNode twoDIndexExpressionNode) => new(Visit(twoDIndexExpressionNode.Expression1), Visit(twoDIndexExpressionNode.Expression2));

    private ProcedureDefModel BuildProcedureDefModel(ProcedureDefNode procedureDefNode) {
        var signature = Visit(procedureDefNode.Signature);
        var statementBlock = Visit(procedureDefNode.StatementBlock);

        return new ProcedureDefModel(signature, statementBlock, procedureDefNode.Standalone);
    }

    private FunctionDefModel BuildFunctionDefModel(FunctionDefNode functionDefNode) {
        var signature = Visit(functionDefNode.Signature);
        var statementBlock = Visit(functionDefNode.StatementBlock);
        var ret = Visit(functionDefNode.Return);

        return new FunctionDefModel(signature, statementBlock, ret, functionDefNode.Standalone);
    }

    private LambdaDefModel BuildLambdaDefModel(LambdaDefNode lambdaDefNode) {
        var expr = Visit(lambdaDefNode.Expression);
        var arguments = lambdaDefNode.Arguments.Select(Visit);

        return new LambdaDefModel(arguments.ToArray(), expr);
    }

    private FunctionDefModel BuildSystemAccessorDefModel(SystemAccessorDefNode functionDefNode) {
        var signature = Visit(functionDefNode.Signature);
        var statementBlock = Visit(functionDefNode.StatementBlock);
        var ret = Visit(functionDefNode.Return);

        return new FunctionDefModel(signature, statementBlock, ret, functionDefNode.Standalone);
    }

    private MethodSignatureModel BuildMethodSignatureModel(MethodSignatureNode methodSignatureNode) {
        var id = Visit(methodSignatureNode.Id);
        var parameters = methodSignatureNode.Parameters.Select(Visit);
        var returnType = methodSignatureNode.ReturnType is { } rt ? Visit(rt) : null;
        return new MethodSignatureModel(id, parameters.ToArray(), returnType);
    }

    private ParameterModel BuildParameterModel(ParameterNode parameterNode) {
        var id = Visit(parameterNode.Id);
        var type = Visit(parameterNode.TypeNode);
        var isRef = parameterNode.IsRef;

        return new ParameterModel(id, type, isRef);
    }

    private CaseModel BuildCaseModel(CaseNode caseNode) {
        var id = caseNode.Value is not null ? Visit(caseNode.Value) : null;
        var type = Visit(caseNode.StatementBlock);

        return new CaseModel(id, type);
    }

    private TryCatchModel BuildTryCatchModel(TryCatchNode tryCatchNode) {
        var id = Visit(tryCatchNode.Id);
        var triedCode = Visit(tryCatchNode.TriedCode);
        var caughtCode = Visit(tryCatchNode.CaughtCode);

        return new TryCatchModel(triedCode, id, caughtCode);
    }

    private PropertyModel BuildPropertyModel(PropertyCallNode propertyCallNode) {
        var property = Visit(propertyCallNode.Property);
        var expression = Visit(propertyCallNode.Expression);

        return new PropertyModel(expression, property);
    }

    private EnumDefModel BuildEnumDefModel(EnumDefNode enumDefNode) {
        var type = Visit(enumDefNode.Type);
        var values = enumDefNode.Values.Select(Visit);

        return new EnumDefModel(type, values.ToArray());
    }

    private EnumValueModel BuildEnumValueModel(EnumValueNode enumValueNode) {
        var type = Visit(enumValueNode.TypeNode);
        var value = enumValueNode.Value;

        return new EnumValueModel(value, type);
    }

    private PairModel BuildKvpModel(PairNode pairNode) {
        var key = Visit(pairNode.Key);
        var value = Visit(pairNode.Value);

        return new PairModel(key, value);
    }

    private ClassDefModel BuildClassDefModel(ClassDefNode classDefNode) {
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

        var defaultClassModel = new DefaultClassDefModel(type, dProperties, pSignatures, Array.Empty<ICodeModel>());

        return new ClassDefModel(type, inherits, constructor, properties, functions, classDefNode.HasDefaultConstructor, defaultClassModel);
    }

    private AbstractClassDefModel BuildAbstractClassDefModel(AbstractClassDefNode abstractClassDefNode) {
        var type = Visit(abstractClassDefNode.Type);
        var inherits = abstractClassDefNode.Inherits.Select(Visit).ToArray();
        var properties = abstractClassDefNode.Properties.Select(Visit).OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Abstract }).ToArray();
        var methodSignatures = abstractClassDefNode.Methods.Select(Visit).ToArray();

        var dProperties = properties.Select(p => p with { PropertyType = PropertyType.AbstractDefault }).Cast<ICodeModel>().ToArray();

        var dProcedureSignatures = methodSignatures.OfType<MethodSignatureModel>().Where(ms => ms.ReturnType is null).Cast<ICodeModel>().ToArray();
        var dFunctionSignatures = methodSignatures.OfType<MethodSignatureModel>().Where(ms => ms.ReturnType is not null).Cast<ICodeModel>().ToArray();

        var defaultClassModel = new DefaultClassDefModel(type, dProperties, dProcedureSignatures, dFunctionSignatures, true);

        return new AbstractClassDefModel(type, inherits, properties.Cast<ICodeModel>().ToArray(), methodSignatures, defaultClassModel);
    }

    private ConstructorModel BuildConstructorModel(ConstructorNode constructorNode) {
        var body = Visit(constructorNode.StatementBlock);
        var parameters = constructorNode.Parameters.Select(Visit);

        return new ConstructorModel(parameters.ToArray(), body);
    }

    private PropertyDefModel BuildPropertyDefModel(PropertyDefNode propertyDefNode) {
        var id = Visit(propertyDefNode.Id);
        var type = Visit(propertyDefNode.Type);

        return new PropertyDefModel(id, type, PropertyType.Mutable, propertyDefNode.IsPrivate);
    }

    private TypeModel BuildTypeModel(TypeNode typeNode) {
        var id = Visit(typeNode.TypeName);

        return new TypeModel(id);
    }

    private FuncTypeModel BuildFuncTypeModel(LambdaTypeNode typeNode) {
        var types = typeNode.Types.Select(Visit);
        var ret = Visit(typeNode.ReturnType);

        return new FuncTypeModel(types.ToArray(), ret);
    }

    private TupleModel BuildTupleTypeModel(TupleTypeNode typeNode) {
        var types = typeNode.Types.Select(Visit);

        return new TupleModel(types.ToArray());
    }

    private TupleModel BuildTupleDefModel(TupleDefNode typeNode) {
        var types = typeNode.Expressions.Select(Visit);

        return new TupleModel(types.ToArray());
    }

    private static ScalarValueModel BuildPropertyPrefixModel(PropertyPrefixNode selfPrefixNode) => new("this");

    private static ScalarValueModel BuildThisInstanceModel(ThisInstanceNode thisInstanceNode) => new("this");

    private static ScalarValueModel BuildGlobalModel(GlobalPrefixNode globalPrefixNode) => new("Globals");

    private static ScalarValueModel BuildNamespaceModel(LibraryNode l) => new(l.Type);

    private QualifiedValueModel BuildQualifiedModel(QualifiedNode qualifiedNode) => new(Visit(qualifiedNode.Qualifier), Visit(qualifiedNode.Qualified));

    private DefaultModel BuildDefaultModel(DefaultNode defaultNode) {
        var id = Visit(defaultNode.Type);

        return new DefaultModel(id, defaultNode.TypeType);
    }

    private WithModel BuildWithModel(WithNode withNode) {
        var expr = Visit(withNode.Expression);
        var assignments = withNode.AssignmentNodes.Select(Visit);

        return new WithModel(expr, assignments.ToArray());
    }

    private DeconstructionModel BuildDeconstructionModel(DeconstructionNode defaultNode) {
        var ids = defaultNode.ItemNodes.Select(Visit);
        var isNew = defaultNode.IsNew;
        return new DeconstructionModel(ids.ToArray(), isNew);
    }

  

    private ThrowModel BuildThrowModel(ThrowNode defaultNode) {
        var thrown = Visit(defaultNode.Thrown);
        return new ThrowModel(thrown);
    }

    private PrintModel BuildPrintModel(PrintNode defaultNode) {
        var thrown = defaultNode.Expression is { } e ? Visit(e) : null;
        return new PrintModel(thrown);
    }

    private InputModel BuildInputModel(InputNode defaultNode) {
        var thrown = defaultNode.Prompt;
        return new InputModel(thrown);
    }

    private ParameterCallModel BuildParameterCallModel(ParameterCallNode defaultNode) => new(Visit(defaultNode.Expression), defaultNode.IsRef);
}