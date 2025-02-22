//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./grammars/Ic11.g4 by ANTLR 4.13.2

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
/// <see cref="Ic11Parser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IIc11Listener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] Ic11Parser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] Ic11Parser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeclaration([NotNull] Ic11Parser.DeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeclaration([NotNull] Ic11Parser.DeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunction([NotNull] Ic11Parser.FunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunction([NotNull] Ic11Parser.FunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] Ic11Parser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] Ic11Parser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] Ic11Parser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] Ic11Parser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.delimetedStatmentWithDelimiter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDelimetedStatmentWithDelimiter([NotNull] Ic11Parser.DelimetedStatmentWithDelimiterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.delimetedStatmentWithDelimiter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDelimetedStatmentWithDelimiter([NotNull] Ic11Parser.DelimetedStatmentWithDelimiterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.delimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.delimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.yieldStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterYieldStatement([NotNull] Ic11Parser.YieldStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.yieldStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.hcfStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterHcfStatement([NotNull] Ic11Parser.HcfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.hcfStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitHcfStatement([NotNull] Ic11Parser.HcfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.sleepStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSleepStatement([NotNull] Ic11Parser.SleepStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.sleepStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSleepStatement([NotNull] Ic11Parser.SleepStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturnStatement([NotNull] Ic11Parser.ReturnStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturnStatement([NotNull] Ic11Parser.ReturnStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.returnValueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturnValueStatement([NotNull] Ic11Parser.ReturnValueStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.returnValueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturnValueStatement([NotNull] Ic11Parser.ReturnValueStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.breakStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBreakStatement([NotNull] Ic11Parser.BreakStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.breakStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBreakStatement([NotNull] Ic11Parser.BreakStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCallStatement([NotNull] Ic11Parser.FunctionCallStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCallStatement([NotNull] Ic11Parser.FunctionCallStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.undelimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.undelimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhileStatement([NotNull] Ic11Parser.WhileStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForStatement([NotNull] Ic11Parser.ForStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForStatement([NotNull] Ic11Parser.ForStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] Ic11Parser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] Ic11Parser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIdAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceWithIdAssignment([NotNull] Ic11Parser.DeviceWithIdAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIdAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceWithIdAssignment([NotNull] Ic11Parser.DeviceWithIdAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIdExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceWithIdExtendedAssignment([NotNull] Ic11Parser.DeviceWithIdExtendedAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIdExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceWithIdExtendedAssignment([NotNull] Ic11Parser.DeviceWithIdExtendedAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.batchAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBatchAssignment([NotNull] Ic11Parser.BatchAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.batchAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBatchAssignment([NotNull] Ic11Parser.BatchAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.memberExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMemberExtendedAssignment([NotNull] Ic11Parser.MemberExtendedAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.memberExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMemberExtendedAssignment([NotNull] Ic11Parser.MemberExtendedAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.memberAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMemberAssignment([NotNull] Ic11Parser.MemberAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.memberAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMemberAssignment([NotNull] Ic11Parser.MemberAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIndexExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceWithIndexExtendedAssignment([NotNull] Ic11Parser.DeviceWithIndexExtendedAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceWithIndexExtendedAssignment([NotNull] Ic11Parser.DeviceWithIndexExtendedAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIndexAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceWithIndexAssignment([NotNull] Ic11Parser.DeviceWithIndexAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceWithIndexAssignment([NotNull] Ic11Parser.DeviceWithIndexAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceStackClear"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceStackClear([NotNull] Ic11Parser.DeviceStackClearContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceStackClear"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceStackClear([NotNull] Ic11Parser.DeviceStackClearContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIdStackClear"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceWithIdStackClear([NotNull] Ic11Parser.DeviceWithIdStackClearContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIdStackClear"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceWithIdStackClear([NotNull] Ic11Parser.DeviceWithIdStackClearContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] Ic11Parser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] Ic11Parser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.constantDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstantDeclaration([NotNull] Ic11Parser.ConstantDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.constantDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstantDeclaration([NotNull] Ic11Parser.ConstantDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>arraySizeDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArraySizeDeclaration([NotNull] Ic11Parser.ArraySizeDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>arraySizeDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArraySizeDeclaration([NotNull] Ic11Parser.ArraySizeDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>arrayListDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayListDeclaration([NotNull] Ic11Parser.ArrayListDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>arrayListDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayListDeclaration([NotNull] Ic11Parser.ArrayListDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.arrayAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayAssignment([NotNull] Ic11Parser.ArrayAssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.arrayAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayAssignment([NotNull] Ic11Parser.ArrayAssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>UnaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryOp([NotNull] Ic11Parser.UnaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>UnaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryOp([NotNull] Ic11Parser.UnaryOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>BatchAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBatchAccess([NotNull] Ic11Parser.BatchAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>BatchAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBatchAccess([NotNull] Ic11Parser.BatchAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>TernaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTernaryOp([NotNull] Ic11Parser.TernaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>TernaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTernaryOp([NotNull] Ic11Parser.TernaryOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedMemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExtendedMemberAccess([NotNull] Ic11Parser.ExtendedMemberAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedMemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExtendedMemberAccess([NotNull] Ic11Parser.ExtendedMemberAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesis([NotNull] Ic11Parser.ParenthesisContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesis([NotNull] Ic11Parser.ParenthesisContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifier([NotNull] Ic11Parser.IdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifier([NotNull] Ic11Parser.IdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMemberAccess([NotNull] Ic11Parser.MemberAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMemberAccess([NotNull] Ic11Parser.MemberAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedDeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExtendedDeviceIdAccess([NotNull] Ic11Parser.ExtendedDeviceIdAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedDeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExtendedDeviceIdAccess([NotNull] Ic11Parser.ExtendedDeviceIdAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Literal</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] Ic11Parser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Literal</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] Ic11Parser.LiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>DeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceIndexAccess([NotNull] Ic11Parser.DeviceIndexAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>DeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceIndexAccess([NotNull] Ic11Parser.DeviceIndexAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>DeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeviceIdAccess([NotNull] Ic11Parser.DeviceIdAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>DeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeviceIdAccess([NotNull] Ic11Parser.DeviceIdAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ArrayElementAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArrayElementAccess([NotNull] Ic11Parser.ArrayElementAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ArrayElementAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArrayElementAccess([NotNull] Ic11Parser.ArrayElementAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCall([NotNull] Ic11Parser.FunctionCallContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>NullaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNullaryOp([NotNull] Ic11Parser.NullaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>NullaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNullaryOp([NotNull] Ic11Parser.NullaryOpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedDeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExtendedDeviceIndexAccess([NotNull] Ic11Parser.ExtendedDeviceIndexAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedDeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExtendedDeviceIndexAccess([NotNull] Ic11Parser.ExtendedDeviceIndexAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBinaryOp([NotNull] Ic11Parser.BinaryOpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context);
}
