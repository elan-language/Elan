//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c://Elan//Repository//Parser//Elan.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="ElanParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IElanListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFile([NotNull] ElanParser.FileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFile([NotNull] ElanParser.FileContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.main"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMain([NotNull] ElanParser.MainContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.main"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMain([NotNull] ElanParser.MainContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.statementBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatementBlock([NotNull] ElanParser.StatementBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.statementBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatementBlock([NotNull] ElanParser.StatementBlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.callStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCallStatement([NotNull] ElanParser.CallStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.callStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCallStatement([NotNull] ElanParser.CallStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.varDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarDef([NotNull] ElanParser.VarDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.varDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarDef([NotNull] ElanParser.VarDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] ElanParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] ElanParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.assignableValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignableValue([NotNull] ElanParser.AssignableValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.assignableValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignableValue([NotNull] ElanParser.AssignableValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCall([NotNull] ElanParser.MethodCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCall([NotNull] ElanParser.MethodCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgumentList([NotNull] ElanParser.ArgumentListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgumentList([NotNull] ElanParser.ArgumentListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.procedureDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProcedureDef([NotNull] ElanParser.ProcedureDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.procedureDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProcedureDef([NotNull] ElanParser.ProcedureDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.procedureSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProcedureSignature([NotNull] ElanParser.ProcedureSignatureContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.procedureSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProcedureSignature([NotNull] ElanParser.ProcedureSignatureContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.parameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParameterList([NotNull] ElanParser.ParameterListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.parameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParameterList([NotNull] ElanParser.ParameterListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParameter([NotNull] ElanParser.ParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParameter([NotNull] ElanParser.ParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.functionDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionDef([NotNull] ElanParser.FunctionDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.functionDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionDef([NotNull] ElanParser.FunctionDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.functionWithBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionWithBody([NotNull] ElanParser.FunctionWithBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.functionWithBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionWithBody([NotNull] ElanParser.FunctionWithBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.expressionFunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionFunction([NotNull] ElanParser.ExpressionFunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.expressionFunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionFunction([NotNull] ElanParser.ExpressionFunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.letIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLetIn([NotNull] ElanParser.LetInContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.letIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLetIn([NotNull] ElanParser.LetInContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.functionSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionSignature([NotNull] ElanParser.FunctionSignatureContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.functionSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionSignature([NotNull] ElanParser.FunctionSignatureContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.constantDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstantDef([NotNull] ElanParser.ConstantDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.constantDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstantDef([NotNull] ElanParser.ConstantDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.enumDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumDef([NotNull] ElanParser.EnumDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.enumDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumDef([NotNull] ElanParser.EnumDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.enumType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumType([NotNull] ElanParser.EnumTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.enumType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumType([NotNull] ElanParser.EnumTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.enumValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumValue([NotNull] ElanParser.EnumValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.enumValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumValue([NotNull] ElanParser.EnumValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassDef([NotNull] ElanParser.ClassDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassDef([NotNull] ElanParser.ClassDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.mutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMutableClass([NotNull] ElanParser.MutableClassContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.mutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMutableClass([NotNull] ElanParser.MutableClassContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.abstractClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAbstractClass([NotNull] ElanParser.AbstractClassContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.abstractClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAbstractClass([NotNull] ElanParser.AbstractClassContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.immutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterImmutableClass([NotNull] ElanParser.ImmutableClassContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.immutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitImmutableClass([NotNull] ElanParser.ImmutableClassContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.abstractImmutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAbstractImmutableClass([NotNull] ElanParser.AbstractImmutableClassContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.abstractImmutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAbstractImmutableClass([NotNull] ElanParser.AbstractImmutableClassContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.inherits"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInherits([NotNull] ElanParser.InheritsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.inherits"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInherits([NotNull] ElanParser.InheritsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProperty([NotNull] ElanParser.PropertyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProperty([NotNull] ElanParser.PropertyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.constructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstructor([NotNull] ElanParser.ConstructorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.constructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstructor([NotNull] ElanParser.ConstructorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.newInstance"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNewInstance([NotNull] ElanParser.NewInstanceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.newInstance"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNewInstance([NotNull] ElanParser.NewInstanceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.withClause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWithClause([NotNull] ElanParser.WithClauseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.withClause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWithClause([NotNull] ElanParser.WithClauseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.proceduralControlFlow"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProceduralControlFlow([NotNull] ElanParser.ProceduralControlFlowContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.proceduralControlFlow"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProceduralControlFlow([NotNull] ElanParser.ProceduralControlFlowContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.if"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIf([NotNull] ElanParser.IfContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.if"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIf([NotNull] ElanParser.IfContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.for"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFor([NotNull] ElanParser.ForContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.for"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFor([NotNull] ElanParser.ForContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.forIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForIn([NotNull] ElanParser.ForInContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.forIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForIn([NotNull] ElanParser.ForInContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.while"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhile([NotNull] ElanParser.WhileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.while"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhile([NotNull] ElanParser.WhileContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.repeat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRepeat([NotNull] ElanParser.RepeatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.repeat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRepeat([NotNull] ElanParser.RepeatContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.try"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTry([NotNull] ElanParser.TryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.try"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTry([NotNull] ElanParser.TryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.switch"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSwitch([NotNull] ElanParser.SwitchContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.switch"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSwitch([NotNull] ElanParser.SwitchContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.case"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCase([NotNull] ElanParser.CaseContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.case"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCase([NotNull] ElanParser.CaseContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.caseDefault"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCaseDefault([NotNull] ElanParser.CaseDefaultContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.caseDefault"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCaseDefault([NotNull] ElanParser.CaseDefaultContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] ElanParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] ElanParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.bracketedExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBracketedExpression([NotNull] ElanParser.BracketedExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.bracketedExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBracketedExpression([NotNull] ElanParser.BracketedExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.ifExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfExpression([NotNull] ElanParser.IfExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.ifExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfExpression([NotNull] ElanParser.IfExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.lambda"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLambda([NotNull] ElanParser.LambdaContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.lambda"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLambda([NotNull] ElanParser.LambdaContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.index"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIndex([NotNull] ElanParser.IndexContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.index"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIndex([NotNull] ElanParser.IndexContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRange([NotNull] ElanParser.RangeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRange([NotNull] ElanParser.RangeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterValue([NotNull] ElanParser.ValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitValue([NotNull] ElanParser.ValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.nameQualifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNameQualifier([NotNull] ElanParser.NameQualifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.nameQualifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNameQualifier([NotNull] ElanParser.NameQualifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.namespace"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNamespace([NotNull] ElanParser.NamespaceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.namespace"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNamespace([NotNull] ElanParser.NamespaceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] ElanParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] ElanParser.LiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralValue([NotNull] ElanParser.LiteralValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralValue([NotNull] ElanParser.LiteralValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.dataStructureDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDataStructureDefinition([NotNull] ElanParser.DataStructureDefinitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.dataStructureDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDataStructureDefinition([NotNull] ElanParser.DataStructureDefinitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalDataStructure"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralDataStructure([NotNull] ElanParser.LiteralDataStructureContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalDataStructure"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralDataStructure([NotNull] ElanParser.LiteralDataStructureContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.tupleDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTupleDefinition([NotNull] ElanParser.TupleDefinitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.tupleDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTupleDefinition([NotNull] ElanParser.TupleDefinitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalTuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralTuple([NotNull] ElanParser.LiteralTupleContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalTuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralTuple([NotNull] ElanParser.LiteralTupleContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.tupleDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTupleDecomp([NotNull] ElanParser.TupleDecompContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.tupleDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTupleDecomp([NotNull] ElanParser.TupleDecompContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.listDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterListDefinition([NotNull] ElanParser.ListDefinitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.listDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitListDefinition([NotNull] ElanParser.ListDefinitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralList([NotNull] ElanParser.LiteralListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralList([NotNull] ElanParser.LiteralListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.listDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterListDecomp([NotNull] ElanParser.ListDecompContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.listDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitListDecomp([NotNull] ElanParser.ListDecompContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.arrayDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayDefinition([NotNull] ElanParser.ArrayDefinitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.arrayDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayDefinition([NotNull] ElanParser.ArrayDefinitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.dictionaryDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDictionaryDefinition([NotNull] ElanParser.DictionaryDefinitionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.dictionaryDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDictionaryDefinition([NotNull] ElanParser.DictionaryDefinitionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalDictionary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralDictionary([NotNull] ElanParser.LiteralDictionaryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalDictionary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralDictionary([NotNull] ElanParser.LiteralDictionaryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.kvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKvp([NotNull] ElanParser.KvpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.kvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKvp([NotNull] ElanParser.KvpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.literalKvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralKvp([NotNull] ElanParser.LiteralKvpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.literalKvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralKvp([NotNull] ElanParser.LiteralKvpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.unaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryOp([NotNull] ElanParser.UnaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.unaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryOp([NotNull] ElanParser.UnaryOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.binaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBinaryOp([NotNull] ElanParser.BinaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.binaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBinaryOp([NotNull] ElanParser.BinaryOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.arithmeticOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArithmeticOp([NotNull] ElanParser.ArithmeticOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.arithmeticOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArithmeticOp([NotNull] ElanParser.ArithmeticOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.logicalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLogicalOp([NotNull] ElanParser.LogicalOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.logicalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLogicalOp([NotNull] ElanParser.LogicalOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.conditionalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConditionalOp([NotNull] ElanParser.ConditionalOpContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.conditionalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConditionalOp([NotNull] ElanParser.ConditionalOpContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] ElanParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] ElanParser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.dataStructureType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDataStructureType([NotNull] ElanParser.DataStructureTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.dataStructureType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDataStructureType([NotNull] ElanParser.DataStructureTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.genericSpecifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGenericSpecifier([NotNull] ElanParser.GenericSpecifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.genericSpecifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGenericSpecifier([NotNull] ElanParser.GenericSpecifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.tupleType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTupleType([NotNull] ElanParser.TupleTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.tupleType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTupleType([NotNull] ElanParser.TupleTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ElanParser.funcType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFuncType([NotNull] ElanParser.FuncTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ElanParser.funcType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFuncType([NotNull] ElanParser.FuncTypeContext context);
}
