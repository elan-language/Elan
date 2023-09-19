//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\\Elan\\Repository\\Parser\\Elan.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ElanParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface IElanVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFile([NotNull] ElanParser.FileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.main"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMain([NotNull] ElanParser.MainContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.statementBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatementBlock([NotNull] ElanParser.StatementBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.callStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCallStatement([NotNull] ElanParser.CallStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.freestandingException"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFreestandingException([NotNull] ElanParser.FreestandingExceptionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.varDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVarDef([NotNull] ElanParser.VarDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] ElanParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.assignableValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignableValue([NotNull] ElanParser.AssignableValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCall([NotNull] ElanParser.MethodCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgumentList([NotNull] ElanParser.ArgumentListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.procedureDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProcedureDef([NotNull] ElanParser.ProcedureDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.procedureSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProcedureSignature([NotNull] ElanParser.ProcedureSignatureContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.parameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameterList([NotNull] ElanParser.ParameterListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameter([NotNull] ElanParser.ParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.functionDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionDef([NotNull] ElanParser.FunctionDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.functionWithBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionWithBody([NotNull] ElanParser.FunctionWithBodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.expressionFunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpressionFunction([NotNull] ElanParser.ExpressionFunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.letIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLetIn([NotNull] ElanParser.LetInContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.functionSignature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionSignature([NotNull] ElanParser.FunctionSignatureContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.constantDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstantDef([NotNull] ElanParser.ConstantDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.enumDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumDef([NotNull] ElanParser.EnumDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.enumValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEnumValue([NotNull] ElanParser.EnumValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassDef([NotNull] ElanParser.ClassDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.mutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMutableClass([NotNull] ElanParser.MutableClassContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.immutableClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitImmutableClass([NotNull] ElanParser.ImmutableClassContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.abstractClass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAbstractClass([NotNull] ElanParser.AbstractClassContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.inherits"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInherits([NotNull] ElanParser.InheritsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty([NotNull] ElanParser.PropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.constructor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstructor([NotNull] ElanParser.ConstructorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.newInstance"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNewInstance([NotNull] ElanParser.NewInstanceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.withClause"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWithClause([NotNull] ElanParser.WithClauseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.proceduralControlFlow"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProceduralControlFlow([NotNull] ElanParser.ProceduralControlFlowContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.if"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIf([NotNull] ElanParser.IfContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.for"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFor([NotNull] ElanParser.ForContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.forIn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForIn([NotNull] ElanParser.ForInContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.while"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhile([NotNull] ElanParser.WhileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.repeat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRepeat([NotNull] ElanParser.RepeatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.try"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTry([NotNull] ElanParser.TryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.switch"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSwitch([NotNull] ElanParser.SwitchContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.case"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCase([NotNull] ElanParser.CaseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.caseDefault"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCaseDefault([NotNull] ElanParser.CaseDefaultContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] ElanParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.bracketedExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBracketedExpression([NotNull] ElanParser.BracketedExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.ifExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfExpression([NotNull] ElanParser.IfExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.lambda"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambda([NotNull] ElanParser.LambdaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.throwException"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitThrowException([NotNull] ElanParser.ThrowExceptionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.index"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIndex([NotNull] ElanParser.IndexContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRange([NotNull] ElanParser.RangeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] ElanParser.ValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] ElanParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralValue([NotNull] ElanParser.LiteralValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.dataStructureDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDataStructureDefinition([NotNull] ElanParser.DataStructureDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalDataStructure"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralDataStructure([NotNull] ElanParser.LiteralDataStructureContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.tupleDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTupleDefinition([NotNull] ElanParser.TupleDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalTuple"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralTuple([NotNull] ElanParser.LiteralTupleContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.tupleDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTupleDecomp([NotNull] ElanParser.TupleDecompContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.listDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListDefinition([NotNull] ElanParser.ListDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralList([NotNull] ElanParser.LiteralListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.listDecomp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListDecomp([NotNull] ElanParser.ListDecompContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.dictionaryDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDictionaryDefinition([NotNull] ElanParser.DictionaryDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalDictionary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralDictionary([NotNull] ElanParser.LiteralDictionaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.kvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKvp([NotNull] ElanParser.KvpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.literalKvp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralKvp([NotNull] ElanParser.LiteralKvpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.assignmentOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignmentOp([NotNull] ElanParser.AssignmentOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.unaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryOp([NotNull] ElanParser.UnaryOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.binaryOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryOp([NotNull] ElanParser.BinaryOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.arithmeticOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmeticOp([NotNull] ElanParser.ArithmeticOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.logicalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogicalOp([NotNull] ElanParser.LogicalOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.conditionalOp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConditionalOp([NotNull] ElanParser.ConditionalOpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] ElanParser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.dataStructureType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDataStructureType([NotNull] ElanParser.DataStructureTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.genericSpecifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericSpecifier([NotNull] ElanParser.GenericSpecifierContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.tupleType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTupleType([NotNull] ElanParser.TupleTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ElanParser.funcType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncType([NotNull] ElanParser.FuncTypeContext context);
}
