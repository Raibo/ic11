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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="Ic11Parser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IIc11Visitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] Ic11Parser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclaration([NotNull] Ic11Parser.DeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunction([NotNull] Ic11Parser.FunctionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] Ic11Parser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] Ic11Parser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.delimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.yieldStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnStatement([NotNull] Ic11Parser.ReturnStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.returnValueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnValueStatement([NotNull] Ic11Parser.ReturnValueStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.breakStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBreakStatement([NotNull] Ic11Parser.BreakStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCallStatement([NotNull] Ic11Parser.FunctionCallStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.undelimitedStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStatement([NotNull] Ic11Parser.IfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.deviceWithIdAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeviceWithIdAssignment([NotNull] Ic11Parser.DeviceWithIdAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.memberExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMemberExtendedAssignment([NotNull] Ic11Parser.MemberExtendedAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.memberAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMemberAssignment([NotNull] Ic11Parser.MemberAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexExtendedAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeviceWithIndexExtendedAssignment([NotNull] Ic11Parser.DeviceWithIndexExtendedAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.deviceWithIndexAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeviceWithIndexAssignment([NotNull] Ic11Parser.DeviceWithIndexAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] Ic11Parser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.constantDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstantDeclaration([NotNull] Ic11Parser.ConstantDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arraySizeDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArraySizeDeclaration([NotNull] Ic11Parser.ArraySizeDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arrayListDeclaration</c>
	/// labeled alternative in <see cref="Ic11Parser.arrayDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayListDeclaration([NotNull] Ic11Parser.ArrayListDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Ic11Parser.arrayAssignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayAssignment([NotNull] Ic11Parser.ArrayAssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Parenthesis</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesis([NotNull] Ic11Parser.ParenthesisContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UnaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryOp([NotNull] Ic11Parser.UnaryOpContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Identifier</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIdentifier([NotNull] Ic11Parser.IdentifierContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMemberAccess([NotNull] Ic11Parser.MemberAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Literal</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] Ic11Parser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeviceIndexAccess([NotNull] Ic11Parser.DeviceIndexAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>DeviceIdAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeviceIdAccess([NotNull] Ic11Parser.DeviceIdAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ArrayElementAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayElementAccess([NotNull] Ic11Parser.ArrayElementAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ExtendedDeviceIndexAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExtendedDeviceIndexAccess([NotNull] Ic11Parser.ExtendedDeviceIndexAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ExtendedMemberAccess</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExtendedMemberAccess([NotNull] Ic11Parser.ExtendedMemberAccessContext context);
}
