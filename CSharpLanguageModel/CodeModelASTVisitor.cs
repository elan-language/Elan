using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using CSharpLanguageModel.Models;

namespace CSharpLanguageModel;

public class CodeModelAstVisitor : AbstractAstVisitor<ICodeModel> {
    protected override void Enter(IAstNode node) { }

    protected override void Exit(IAstNode node) { }

    private FileCodeModel BuildFileModel(FileNode fileNode) {
        var main = fileNode.MainNode is { } mn ? Visit(mn) : null;
        var globals = fileNode.GlobalNodes.Select(Visit);
        return new FileCodeModel(globals, main);
    }

    private MainCodeModel BuildMainModel(MainNode mainNode) => new(Visit(mainNode.StatementBlock));

    private StatementBlockModel BuildStatementBlockModel(StatementBlockNode statementBlock) => new(statementBlock.Statements.Select(Visit));

    public override ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            FileNode n => HandleScope(BuildFileModel, n),
            MainNode n => HandleScope(BuildMainModel, n),
            SystemAccessorCallNode n => HandleScope(BuildSystemCallModel, n),
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
            ForInStatementNode n => HandleScope(BuildForInStatementModel, n),
            TwoDIndexExpressionNode n => HandleScope(Build2DIndexModel, n),
            ProcedureDefNode n => HandleScope(BuildProcedureDefModel, n),
            FunctionDefNode n => HandleScope(BuildFunctionDefModel, n),
            SystemAccessorDefNode n => HandleScope(BuildSystemAccessorDefModel, n),
            MethodSignatureNode n => HandleScope(BuildMethodSignatureModel, n),
            ParameterNode n => HandleScope(BuildParameterModel, n),
            SwitchStatementNode n => HandleScope(BuildSwitchStatementModel, n),
            CaseNode n => HandleScope(BuildCaseModel, n),
            TryCatchNode n => HandleScope(BuildTryCatchModel, n),
            PropertyNode n => HandleScope(BuildPropertyModel, n),
            EnumDefNode n => HandleScope(BuildEnumDefModel, n),
            EnumValueNode n => HandleScope(BuildEnumValueModel, n),
            PairNode n => HandleScope(BuildKvpModel, n),
            ClassDefNode n => HandleScope(BuildClassDefModel, n),
            AbstractClassDefNode n => HandleScope(BuildAbstractClassDefModel, n),
            ConstructorNode n => HandleScope(BuildConstructorModel, n),
            PropertyDefNode n => HandleScope(BuildPropertyDefModel, n),
            TypeNode n => HandleScope(BuildTypeModel, n),
            SelfNode n => HandleScope(BuildSelfModel, n),
            GlobalNode n => HandleScope(BuildGlobalModel, n),
            LibraryNode n => HandleScope(BuildNamespaceModel, n),
            ReturnExpressionNode n => Visit(n.Expression),
            QualifiedNode n => HandleScope(BuildQualifiedModel, n),
            DefaultNode n => HandleScope(BuildDefaultModel, n),
            WithNode n => HandleScope(BuildWithModel, n),
            DeconstructionNode n => HandleScope(BuildDeconstructionModel, n),
            ThrowNode n => HandleScope(BuildThrowModel, n),
            PrintNode n => HandleScope(BuildPrintModel, n),
            ParameterCallNode n => HandleScope(BuildParameterCallModel, n),
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private VarDefModel BuildVarDefModel(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Expression));

    private ConstantDefModel BuildConstantDefModel(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToPrefixedCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel BuildAssignmentModel(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Expression), assignmentNode.Inline);

    private ProcedureCallModel BuildProcedureCallModel(ProcedureCallNode procedureCallNode) {
        var parameters = procedureCallNode.Parameters.Select(Visit);
        var qualifier = procedureCallNode.Qualifier is { } q ? Visit(q) : null;

        return new ProcedureCallModel(Visit(procedureCallNode.Id), qualifier, parameters);
    }

    private MethodCallModel BuildSystemCallModel(SystemAccessorCallNode systemAccessorCallNode) => new(CodeHelpers.MethodType.SystemCall, Visit(systemAccessorCallNode.Id), null, systemAccessorCallNode.Parameters.Select(Visit));

    private MethodCallModel BuildFunctionCallModel(FunctionCallNode functionCallNode) {
        var qual = functionCallNode.Qualifier is { } q ? Visit(q) : null;
        return new MethodCallModel(CodeHelpers.MethodType.Function, Visit(functionCallNode.Id), qual, functionCallNode.Parameters.Select(Visit));
    }

    private ScalarValueModel BuildScalarValueModel(IScalarValueNode scalarValueNode) => new(scalarValueNode.Value);

    private IdentifierModel BuildIdentifierModel(IdentifierNode identifierNode) => new(identifierNode.Id);

    private IfStatementModel BuildIfStatementModel(IfStatementNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        var statementBlocks = ifStatementNode.StatementBlocks.Select(Visit);

        return new IfStatementModel(expressions, statementBlocks);
    }

    private SwitchStatementModel BuildSwitchStatementModel(SwitchStatementNode switchStatementNode) {
        var expression = Visit(switchStatementNode.Expression);
        var cases = switchStatementNode.Cases.Select(Visit);
        var defaultCase = Visit(switchStatementNode.DefaultCase);

        return new SwitchStatementModel(expression, cases, defaultCase);
    }

    private ForStatementModel BuildForStatementModel(ForStatementNode forStatementNode) {
        var id = Visit(forStatementNode.Id);
        var expressions = forStatementNode.Expressions.Select(Visit);
        var statementBlock = Visit(forStatementNode.StatementBlock);
        var step = forStatementNode.Step is { } i ? Visit(i) : null;
        var neg = forStatementNode.Neg;

        return new ForStatementModel(id, expressions, step, neg, statementBlock);
    }

    private ForInStatementModel BuildForInStatementModel(ForInStatementNode forInStatementNode) {
        var id = Visit(forInStatementNode.Id);
        var expression = Visit(forInStatementNode.Expression);
        var statementBlock = Visit(forInStatementNode.StatementBlock);

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

        return new LiteralListModel(items, type);
    }

    private LiteralTupleModel BuildLiteralTupleModel(LiteralTupleNode literalListNode) {
        var items = literalListNode.ItemNodes.Select(Visit);

        return new LiteralTupleModel(items);
    }

    private LiteralDictionaryModel BuildLiteralDictionaryModel(LiteralDictionaryNode literalDictionaryNode) {
        var type = CodeHelpers.NodeToCSharpType(literalDictionaryNode.ItemNodes.First());
        var items = literalDictionaryNode.ItemNodes.Select(Visit);

        return new LiteralDictionaryModel(items, type);
    }

    private ICodeModel BuildOperatorModel(OperatorNode operatorNode) {
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

        return new NewInstanceModel(type, args);
    }

    private DataStructureTypeModel BuildDataStructureTypeModel(DataStructureTypeNode dataStructureTypeNode) {
        var types = dataStructureTypeNode.GenericTypes.Select(Visit);

        return new DataStructureTypeModel(types, CodeHelpers.DataStructureTypeToCSharpType(dataStructureTypeNode.Type));
    }

    private ScalarValueModel BuildValueTypeModel(ValueTypeNode valueTypeNode) => new(CodeHelpers.ValueTypeToCSharpType(valueTypeNode.Type));

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
        return new MethodSignatureModel(id, parameters, returnType);
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

    private PropertyModel BuildPropertyModel(PropertyNode propertyNode) {
        var property = Visit(propertyNode.Property);
        var expression = Visit(propertyNode.Expression);

        return new PropertyModel(expression, property);
    }

    private EnumDefModel BuildEnumDefModel(EnumDefNode enumDefNode) {
        var type = Visit(enumDefNode.Type);
        var values = enumDefNode.Values.Select(Visit);

        return new EnumDefModel(type, values);
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

    private static IAstNode GetSignatures(IAstNode node) {
        return node switch {
            ProcedureDefNode pdn => pdn.Signature,
            FunctionDefNode fdn => fdn.Signature,
            _ => throw new NotImplementedException(node.GetType().ToString())
        };
    }

    private ClassDefModel BuildClassDefModel(ClassDefNode classDefNode) {
        var type = Visit(classDefNode.Type);
        var inherits = classDefNode.Inherits.Select(Visit);
        var constructor = Visit(classDefNode.Constructor);
        var properties = classDefNode.Properties.Select(Visit).ToList();
        var functions = classDefNode.Methods.Select(Visit);

        if (classDefNode.Immutable) {
            properties = properties.OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Immutable }).Cast<ICodeModel>().ToList();
        }

        var pSignatures = classDefNode.Methods.OfType<ProcedureDefNode>().Select(n => n.Signature).Select(Visit);

        var dProperties = properties.OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Default });

        var defaultClassModel = new DefaultClassDefModel(type, dProperties, pSignatures);

        return new ClassDefModel(type, inherits, constructor, properties, functions, classDefNode.HasDefaultConstructor, defaultClassModel);
    }

    private AbstractClassDefModel BuildAbstractClassDefModel(AbstractClassDefNode abstractClassDefNode) {
        var type = Visit(abstractClassDefNode.Type);
        var inherits = abstractClassDefNode.Inherits.Select(Visit);
        var properties = abstractClassDefNode.Properties.Select(Visit).OfType<PropertyDefModel>().Select(p => p with { PropertyType = PropertyType.Abstract });
        var functions = abstractClassDefNode.Methods.Select(Visit);

        return new AbstractClassDefModel(type, inherits, properties, functions);
    }

    private ConstructorModel BuildConstructorModel(ConstructorNode constructorNode) {
        var body = Visit(constructorNode.StatementBlock);
        var parameters = constructorNode.Parameters.Select(Visit);

        return new ConstructorModel(parameters, body);
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

    private ScalarValueModel BuildSelfModel(SelfNode selfNode) => new("this");

    private ScalarValueModel BuildGlobalModel(GlobalNode globalNode) => new("Globals");

    private ScalarValueModel BuildNamespaceModel(LibraryNode l) => new(l.Type);

    private QualifiedValueModel BuildQualifiedModel(QualifiedNode qualifiedNode) => new(Visit(qualifiedNode.Qualifier), Visit(qualifiedNode.Qualified));

    private DefaultModel BuildDefaultModel(DefaultNode defaultNode) {
        var id = Visit(defaultNode.Type);

        return new DefaultModel(id, defaultNode.TypeType);
    }

    private WithModel BuildWithModel(WithNode withNode) {
        var expr = Visit(withNode.Expression);
        var assignments = withNode.AssignmentNodes.Select(Visit);

        return new WithModel(expr, assignments);
    }

    private DeconstructionModel BuildDeconstructionModel(DeconstructionNode defaultNode) {
        var ids = defaultNode.ItemNodes.Select(Visit);
        var isNew = defaultNode.IsNew;
        return new DeconstructionModel(ids, isNew);
    }

    private ThrowModel BuildThrowModel(ThrowNode defaultNode) {
        var thrown = Visit(defaultNode.Thrown);
        return new ThrowModel(thrown);
    }

    private PrintModel BuildPrintModel(PrintNode defaultNode) {
        var thrown = defaultNode.Expression is { } e ? Visit(e) : null;
        return new PrintModel(thrown);
    }

    private ParameterCallModel BuildParameterCallModel(ParameterCallNode defaultNode) => new(Visit(defaultNode.Expression), defaultNode.IsRef);
}