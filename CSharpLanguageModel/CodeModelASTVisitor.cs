using AbstractSyntaxTree;
using AbstractSyntaxTree.Nodes;
using CSharpLanguageModel.Models;

namespace CSharpLanguageModel;

public class CodeModelAstVisitor : AbstractAstVisitor<ICodeModel> {
    protected override void Enter(IAstNode node) { }

    protected override void Exit(IAstNode node) { }

    private FileCodeModel BuildFileModel(FileNode fileNode) {
        var main = Visit(fileNode.MainNode);
        var globals = fileNode.GlobalNodes.Select(Visit);
        return new FileCodeModel(globals, main);
    }

    private MainCodeModel BuildMainModel(MainNode mainNode) => new(Visit(mainNode.StatementBlock));

    private StatementBlockModel BuildStatementBlockModel(StatementBlockNode statementBlock) => new(statementBlock.Statements.Select(Visit));

    public override ICodeModel Visit(IAstNode astNode) {
        return astNode switch {
            FileNode n => HandleScope(BuildFileModel, n),
            MainNode n => HandleScope(BuildMainModel, n),
            SystemCallNode n => HandleScope(BuildSystemCallModel, n),
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
            IndexedExpressionNode n => HandleScope(BuildIndexedExpressionModel, n),
            RangeExpressionNode n => HandleScope(BuildRangeExpressionModel, n),
            NewInstanceNode n => HandleScope(BuildNewInstanceModel, n),
            DataStructureTypeNode n => HandleScope(BuildDataStructureTypeModel, n),
            ValueTypeNode n => HandleScope(BuildValueTypeModel, n),
            ForStatementNode n => HandleScope(BuildForStatementModel, n),
            ForInStatementNode n => HandleScope(BuildForInStatementModel, n),
            TwoDIndexExpressionNode n => HandleScope(Build2DIndexModel, n),
            ProcedureDefNode n => HandleScope(BuildProcedureDefModel, n),
            FunctionDefNode n => HandleScope(BuildFunctionDefModel, n),
            MethodSignatureNode n => HandleScope(BuildMethodSignatureModel, n),
            ParameterNode n => HandleScope(BuildParameterModel, n),
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private VarDefModel BuildVarDefModel(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Expression));

    private ConstantDefModel BuildConstantDefModel(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToPrefixedCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel BuildAssignmentModel(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Expression));

    private ProcedureCallModel BuildProcedureCallModel(ProcedureCallNode procedureCallNode) {
        var parameters = procedureCallNode.Parameters.Select(Visit);
        var passByRef = procedureCallNode.Parameters.Select(p => p is IdentifierNode);

        var zip = parameters.Zip(passByRef);


        return new ProcedureCallModel(Visit(procedureCallNode.Id), zip);
    }

    private MethodCallModel BuildSystemCallModel(SystemCallNode systemCallNode) => new(CodeHelpers.MethodType.SystemCall, Visit(systemCallNode.Id), systemCallNode.Parameters.Select(Visit));

    private MethodCallModel BuildFunctionCallModel(FunctionCallNode functionCallNode) => new(CodeHelpers.MethodType.Function, Visit(functionCallNode.Id), functionCallNode.Parameters.Select(Visit));

    private ScalarValueModel BuildScalarValueModel(IScalarValueNode scalarValueNode) => new(scalarValueNode.Value);

    private IdentifierModel BuildIdentifierModel(IdentifierNode identifierNode) => new(identifierNode.Id);

    private IfStatementModel BuildIfStatementModel(IfStatementNode ifStatementNode) {
        var expressions = ifStatementNode.Expressions.Select(Visit);
        var statementBlocks = ifStatementNode.StatementBlocks.Select(Visit);

        return new IfStatementModel(expressions, statementBlocks);
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

    private RangeExpressionModel BuildRangeExpressionModel(RangeExpressionNode rangeExpressionNode) {
        var expression1 = Visit(rangeExpressionNode.Expression1);
        var expression2 = rangeExpressionNode.Expression2 is { } e ? Visit(e) : null;

        return new RangeExpressionModel(rangeExpressionNode.Prefix, expression1, expression2);
    }

    private NewInstanceModel BuildNewInstanceModel(NewInstanceNode newInstanceNode) {
        var type = Visit(newInstanceNode.Type);
        var args = newInstanceNode.Arguments.Select(Visit);
        var init = newInstanceNode.Init.Select(Visit);

        return new NewInstanceModel(type, args, init);
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

        return new ProcedureDefModel(signature, statementBlock);
    }

    private ProcedureDefModel BuildFunctionDefModel(FunctionDefNode functionDefNode) {
        var signature = Visit(functionDefNode.Signature);
        var statementBlock = Visit(functionDefNode.StatementBlock);
        var ret = Visit(functionDefNode.Return);

        return new ProcedureDefModel(signature, statementBlock);
    }

    private MethodSignatureModel BuildMethodSignatureModel(MethodSignatureNode procedureDefNode) {
        var id = Visit(procedureDefNode.Id);
        var parameters = procedureDefNode.Parameters.Select(Visit);
        return new MethodSignatureModel(id, parameters);
    }

    private ParameterModel BuildParameterModel(ParameterNode parameterNode) {
        var id = Visit(parameterNode.Id);
        var type = Visit(parameterNode.TypeNode);

        return new ParameterModel(id, type);
    }
}