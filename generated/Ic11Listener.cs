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
	/// Enter a parse tree produced by <see cref="Ic11Parser.negation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNegation([NotNull] Ic11Parser.NegationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.negation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNegation([NotNull] Ic11Parser.NegationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] Ic11Parser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] Ic11Parser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Ic11Parser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] Ic11Parser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Ic11Parser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] Ic11Parser.LiteralContext context);
}
