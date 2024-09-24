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
	/// Visit a parse tree produced by <see cref="Ic11Parser.continueStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitContinueStatement([NotNull] Ic11Parser.ContinueStatementContext context);
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
	/// Visit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCall([NotNull] Ic11Parser.FunctionCallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>BinaryOp</c>
	/// labeled alternative in <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinaryOp([NotNull] Ic11Parser.BinaryOpContext context);
}
