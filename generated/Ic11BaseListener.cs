//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Ic11.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IIc11Listener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class Ic11BaseListener : IIc11Listener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] Ic11Parser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] Ic11Parser.ProgramContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.declaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeclaration([NotNull] Ic11Parser.DeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.declaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeclaration([NotNull] Ic11Parser.DeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunction([NotNull] Ic11Parser.FunctionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.function"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunction([NotNull] Ic11Parser.FunctionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] Ic11Parser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] Ic11Parser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] Ic11Parser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] Ic11Parser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.delimitedStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.delimitedStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.yieldStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterYieldStatement([NotNull] Ic11Parser.YieldStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.yieldStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnStatement([NotNull] Ic11Parser.ReturnStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnStatement([NotNull] Ic11Parser.ReturnStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.returnValueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnValueStatement([NotNull] Ic11Parser.ReturnValueStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.returnValueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnValueStatement([NotNull] Ic11Parser.ReturnValueStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.breakStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBreakStatement([NotNull] Ic11Parser.BreakStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.breakStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBreakStatement([NotNull] Ic11Parser.BreakStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.functionCallStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionCallStatement([NotNull] Ic11Parser.FunctionCallStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.functionCallStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionCallStatement([NotNull] Ic11Parser.FunctionCallStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.undelimitedStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.undelimitedStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileStatement([NotNull] Ic11Parser.WhileStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] Ic11Parser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] Ic11Parser.IfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIdAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceWithIdAssignment([NotNull] Ic11Parser.DeviceWithIdAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIdAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceWithIdAssignment([NotNull] Ic11Parser.DeviceWithIdAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIdExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceWithIdExtendedAssignment([NotNull] Ic11Parser.DeviceWithIdExtendedAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIdExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceWithIdExtendedAssignment([NotNull] Ic11Parser.DeviceWithIdExtendedAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.memberExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMemberExtendedAssignment([NotNull] Ic11Parser.MemberExtendedAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.memberExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMemberExtendedAssignment([NotNull] Ic11Parser.MemberExtendedAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.memberAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMemberAssignment([NotNull] Ic11Parser.MemberAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.memberAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMemberAssignment([NotNull] Ic11Parser.MemberAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIndexExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceWithIndexExtendedAssignment([NotNull] Ic11Parser.DeviceWithIndexExtendedAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexExtendedAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceWithIndexExtendedAssignment([NotNull] Ic11Parser.DeviceWithIndexExtendedAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.deviceWithIndexAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceWithIndexAssignment([NotNull] Ic11Parser.DeviceWithIndexAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceWithIndexAssignment([NotNull] Ic11Parser.DeviceWithIndexAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAssignment([NotNull] Ic11Parser.AssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.assignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAssignment([NotNull] Ic11Parser.AssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.constantDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConstantDeclaration([NotNull] Ic11Parser.ConstantDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.constantDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConstantDeclaration([NotNull] Ic11Parser.ConstantDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>arraySizeDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArraySizeDeclaration([NotNull] Ic11Parser.ArraySizeDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>arraySizeDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArraySizeDeclaration([NotNull] Ic11Parser.ArraySizeDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>arrayListDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayListDeclaration([NotNull] Ic11Parser.ArrayListDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>arrayListDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayListDeclaration([NotNull] Ic11Parser.ArrayListDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.arrayAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayAssignment([NotNull] Ic11Parser.ArrayAssignmentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.arrayAssignment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayAssignment([NotNull] Ic11Parser.ArrayAssignmentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>UnaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnaryOp([NotNull] Ic11Parser.UnaryOpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>UnaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnaryOp([NotNull] Ic11Parser.UnaryOpContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedMemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExtendedMemberAccess([NotNull] Ic11Parser.ExtendedMemberAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedMemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExtendedMemberAccess([NotNull] Ic11Parser.ExtendedMemberAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesis([NotNull] Ic11Parser.ParenthesisContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesis([NotNull] Ic11Parser.ParenthesisContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifier([NotNull] Ic11Parser.IdentifierContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifier([NotNull] Ic11Parser.IdentifierContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMemberAccess([NotNull] Ic11Parser.MemberAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMemberAccess([NotNull] Ic11Parser.MemberAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedDeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExtendedDeviceIdAccess([NotNull] Ic11Parser.ExtendedDeviceIdAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedDeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExtendedDeviceIdAccess([NotNull] Ic11Parser.ExtendedDeviceIdAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>Literal</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] Ic11Parser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>Literal</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] Ic11Parser.LiteralContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>DeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceIndexAccess([NotNull] Ic11Parser.DeviceIndexAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>DeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceIndexAccess([NotNull] Ic11Parser.DeviceIndexAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>DeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDeviceIdAccess([NotNull] Ic11Parser.DeviceIdAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>DeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDeviceIdAccess([NotNull] Ic11Parser.DeviceIdAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ArrayElementAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArrayElementAccess([NotNull] Ic11Parser.ArrayElementAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ArrayElementAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArrayElementAccess([NotNull] Ic11Parser.ArrayElementAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionCall([NotNull] Ic11Parser.FunctionCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ExtendedDeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExtendedDeviceIndexAccess([NotNull] Ic11Parser.ExtendedDeviceIndexAccessContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ExtendedDeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExtendedDeviceIndexAccess([NotNull] Ic11Parser.ExtendedDeviceIndexAccessContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBinaryOp([NotNull] Ic11Parser.BinaryOpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
