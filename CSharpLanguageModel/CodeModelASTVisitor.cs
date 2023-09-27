﻿using AbstractSyntaxTree;
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
            MethodCallNode n => HandleScope(BuildMethodCallModel, n),
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
            null => throw new NotImplementedException("null"),
            _ => throw new NotImplementedException(astNode.GetType().ToString() ?? "null")
        };
    }

    private VarDefModel BuildVarDefModel(VarDefNode varDefNode) => new(Visit(varDefNode.Id), Visit(varDefNode.Expression));

    private ConstantDefModel BuildConstantDefModel(ConstantDefNode constantDefNode) => new(Visit(constantDefNode.Id), CodeHelpers.NodeToPrefixedCSharpType(constantDefNode.Expression), Visit(constantDefNode.Expression));

    private AssignmentModel BuildAssignmentModel(AssignmentNode assignmentNode) => new(Visit(assignmentNode.Id), Visit(assignmentNode.Expression));

    private MethodCallModel BuildMethodCallModel(MethodCallNode methodCallNode) => new(Visit(methodCallNode.Id), methodCallNode.Parameters.Select(Visit));

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
        var step = forStatementNode.Step is {} i ? Visit(i) : null;
        var neg = forStatementNode.Neg;

        return new ForStatementModel(id, expressions, step, neg, statementBlock);
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

    private NewInstanceModel BuildNewInstanceModel(NewInstanceNode  newInstanceNode) {
        var type = Visit(newInstanceNode.Type);

        return new NewInstanceModel(type);
    }

    private DataStructureTypeModel BuildDataStructureTypeModel(DataStructureTypeNode  dataStructureTypeNode) {
        var types = dataStructureTypeNode.GenericTypes.Select(Visit);

        return new DataStructureTypeModel(types, CodeHelpers.DataStructureTypeToCSharpType(dataStructureTypeNode.Type));
    }

    private ScalarValueModel BuildValueTypeModel(ValueTypeNode  valueTypeNode) {
        return new ScalarValueModel(CodeHelpers.ValueTypeToCSharpType(valueTypeNode.Type));
    }
}