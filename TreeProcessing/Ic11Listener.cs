using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.Operations;
using ic11.TreeProcessing.Operations;
using System.Text;

namespace ic11.TreeProcessing;
public class Ic11Listener : Ic11BaseListener
{
    public readonly TreeContext TreeContext = new();
    //public readonly StringBuilder Writer = new();


    
    public override void ExitDeclaration([NotNull] Ic11Parser.DeclarationContext context)
    {
        var aliasName = context.IDENTIFIER().Symbol.Text;
        var aliasId = context.PINID().Symbol.Text;
        TreeContext.PinAliases[aliasName] = aliasId;

        //Writer.AppendLine($"alias {aliasName} {aliasId}");
    }





    public override void ExitVariableDeclaration([NotNull] Ic11Parser.VariableDeclarationContext context)
    {
        TreeContext.Variables.Add(context.IDENTIFIER().GetText(), "");
        //Console.WriteLine($"LEft Declaration: {context.GetText()}");
    }

    public override void EnterWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        TreeContext.WhileLabels.Push(TreeContext.WhileCount++);
        var startLabel = $"StartWhile{TreeContext.WhileLabels.Peek()}";
        //Writer.AppendLine($"{startLabel}:");
    }

    public override void ExitWhileStatement([NotNull] Ic11Parser.WhileStatementContext context)
    {
        var startLabel = $"StartWhile{TreeContext.WhileLabels.Pop()}";
        //Writer.AppendLine($"j {startLabel}");
    }

    //public override void EnterExpression([NotNull] Ic11Parser.ExpressionContext context)
    //{
    //    Console.WriteLine("=======================================");
    //    foreach(var child in context.children)
    //    {
    //
    //        var operation = child.GetText();
    //        var childType = child.GetType();
    //
    //        Console.WriteLine($"Operation: {operation} [{childType.Name}]");
    //    }
    //
    //    //HashSet<string> binaries = new() { "&&", "||", "+", "-", "" };
    //
    //    //context.children.Where(c => c.GetText());
    //}   


    public override void EnterTheLiteral([NotNull] Ic11Parser.TheLiteralContext context)
    {
        Console.WriteLine($"Literal: {context.GetText()}");
    }

    public override void EnterAddSub([NotNull] Ic11Parser.AddSubContext context)
    {
        var operand1 = context.operand1.GetText();
        var operand2 = context.operand2.GetText();
        var newVar = GenerateNewTempVar();

        Operation operation = context.op.Text switch
        {
            "+" => new Operations.Add(newVar, operand1, operand2),
            "-" => new Operations.Sub(newVar, operand1, operand2),
            _ => throw new Exception()
        };

        TreeContext.Operations.Add(operation);
    }

    private string GenerateNewTempVar()
    {
        var newVar = $"TempVar{TreeContext.TempVarIndex++}";
        TreeContext.Variables.Add(newVar, "");
        return newVar;
    }
}
