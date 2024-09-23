using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Text;

namespace ic11.TreeProcessing;
public class Ic11Listener : IIc11Listener
{
    public readonly TreeContext TreeContext = new();
    //public readonly StringBuilder Writer = new();

    public void EnterDeclaration([NotNull] Ic11Parser.DeclarationContext context)
    { }
    
    public void ExitDeclaration([NotNull] Ic11Parser.DeclarationContext context)
    {
        var aliasName = context.IDENTIFIER().Symbol.Text;
        var aliasId = context.PINID().Symbol.Text;
        TreeContext.PinAliases[aliasName] = aliasId;

        //Writer.AppendLine($"alias {aliasName} {aliasId}");
    }



    public void EnterVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context)
    {
        // Console.WriteLine($"Declaration: {context.GetText()}");
    }

    public void ExitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context)
    {
        TreeContext.Variables.Add(context.IDENTIFIER().GetText(), "");
        //Console.WriteLine($"LEft Declaration: {context.GetText()}");
    }

    public void EnterWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        TreeContext.WhileLabels.Push(TreeContext.WhileCount++);
        var startLabel = $"StartWhile{TreeContext.WhileLabels.Peek()}";
        //Writer.AppendLine($"{startLabel}:");
    }

    public void ExitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        var startLabel = $"StartWhile{TreeContext.WhileLabels.Pop()}";
        //Writer.AppendLine($"j {startLabel}");
    }

    public void EnterProgram([NotNull] Ic11Parser.ProgramContext context) { }
    public void ExitProgram([NotNull] Ic11Parser.ProgramContext context) { }

    public void EnterFunction([NotNull] Ic11Parser.FunctionContext context) { Console.WriteLine("Not Implemented! EnterFunction"); }
    public void ExitFunction([NotNull] Ic11Parser.FunctionContext context) { }
    
    public void EnterBlock([NotNull] Ic11Parser.BlockContext context) { Console.WriteLine("Not Implemented! EnterBlock"); }
    public void ExitBlock([NotNull] Ic11Parser.BlockContext context) { }
    
    public void EnterStatement([NotNull] Ic11Parser.StatementContext context) { Console.WriteLine("Not Implemented! EnterStatement"); }
    public void ExitStatement([NotNull] Ic11Parser.StatementContext context) { }
    
    public void EnterDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) { Console.WriteLine("Not Implemented! EnterDelimitedStatement"); }
    public void ExitDelimitedStatement([NotNull] Ic11Parser.DelimitedStatementContext context) { }
    
    public void EnterUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) { Console.WriteLine("Not Implemented! EnterUndelimitedStatement"); }
    public void ExitUndelimitedStatement([NotNull] Ic11Parser.UndelimitedStatementContext context) { }

    public void EnterIfStatement([NotNull] Ic11Parser.IfStatementContext context) { Console.WriteLine("Not Implemented! EnterIfStatement"); }
    public void ExitIfStatement([NotNull] Ic11Parser.IfStatementContext context) { }
    
    public void EnterExpression([NotNull] Ic11Parser.ExpressionContext context)
    {
        Console.WriteLine("=======================================");
        foreach(var child in context.children)
        {

            var operation = child.GetText();
            var childType = child.GetType();

            Console.WriteLine($"Operation: {operation} [{childType.Name}]");
        }

        //HashSet<string> binaries = new() { "&&", "||", "+", "-", "" };

        //context.children.Where(c => c.GetText());
    }   
    public void ExitExpression([NotNull] Ic11Parser.ExpressionContext context) { }
    
    public void EnterPrimaryExpression([NotNull] Ic11Parser.PrimaryExpressionContext context) { Console.WriteLine("Not Implemented! EnterPrimaryExpression"); }
    public void ExitPrimaryExpression([NotNull] Ic11Parser.PrimaryExpressionContext context) { }
    
    public void EnterUnaryOperator([NotNull] Ic11Parser.UnaryOperatorContext context) { Console.WriteLine("Not Implemented! EnterUnaryOperator"); }
    public void ExitUnaryOperator([NotNull] Ic11Parser.UnaryOperatorContext context) { }
    
    public void EnterLiteral([NotNull] Ic11Parser.LiteralContext context) { Console.WriteLine("Not Implemented! EnterLiteral"); }
    public void ExitLiteral([NotNull] Ic11Parser.LiteralContext context) { }
    
    public void EnterAssignment([NotNull] Ic11Parser.AssignmentContext context) { Console.WriteLine("Not Implemented! EnterAssignment"); }
    public void ExitAssignment([NotNull] Ic11Parser.AssignmentContext context) { }
    
    public void VisitTerminal(ITerminalNode node) {  }
    public void VisitErrorNode(IErrorNode node) {  }
    
    public void EnterEveryRule(ParserRuleContext ctx) {  }
    public void ExitEveryRule(ParserRuleContext ctx) { }

    public void EnterYieldStatement([NotNull] Ic11Parser.YieldStatementContext context) { }
    public void ExitYieldStatement([NotNull] Ic11Parser.YieldStatementContext context)
    {
        //Writer.AppendLine("yield");
    }
}
