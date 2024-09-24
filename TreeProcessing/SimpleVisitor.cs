using Antlr4.Runtime.Misc;
using ic11.TreeProcessing.Operations;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing;
public class CompileVisitor : Ic11BaseVisitor<IValue> // TODO more complex reference? 
{   
    public ProgramContext ProgramContext;

    public CompileVisitor(ProgramContext programContext)
    {
        ProgramContext = programContext;
    }

    public override IValue VisitUnaryOp(Ic11Parser.UnaryOpContext context)
    {
        //Console.WriteLine("Visited UnaryOP: " + context.GetText());
        var operand = Visit(context.operand);
        if (operand == null || operand is None){
            Console.WriteLine("Operand of an Unary operation cannot return None: " + context.operand.GetText() + " [" + context.operand.GetType() + "]");
            operand = new Unknown(context.operand.GetText() + " [" + context.operand.GetType() + "]");
        }            

        var tempVar = new Variable(ProgramContext.ClaimTempVar());
        Operation? operation = null;
        switch (context.op.Type)
        {
            case Ic11Parser.NEGATION:
                operation = new Negation(tempVar, operand);
                break;
            case Ic11Parser.SUB:
                operation = new Minus(tempVar, operand);
                break;
            default:
                throw new NotImplementedException();
        }

        ProgramContext.InsertOperation(operation);

        return tempVar;
    }

    public override IValue VisitBinaryOp(Ic11Parser.BinaryOpContext context)
    {
        //Console.WriteLine("Visited BinaryOP: " + context.GetText());
        var operand1 = Visit(context.left);
        var operand2 = Visit(context.right);

        if ( operand1 == null || operand1 is None){
            Console.WriteLine("Operands of a Binary operation cannot return None: " + context.left + " [" + context.left.GetType() + "]");
            operand1 = new Unknown(context.left.GetText() + " [" + context.left.GetType() + "]");
        }

        if ( operand2 == null || operand2 is None){
            Console.WriteLine("Operands of a Binary operation cannot return None: " + context.right + " [" + context.right.GetType() + "]");
            operand2 = new Unknown(context.right.GetText() + " [" + context.right.GetType() + "]");
        }

        var tempVar = new Variable(ProgramContext.ClaimTempVar());
        Operation? operation = null;
        switch (context.op.Type)
        {
            case Ic11Parser.ADD:
                operation = new Add(tempVar, operand1, operand2);
                break;
            case Ic11Parser.SUB:
                operation = new Sub(tempVar, operand1, operand2);
                break;
            case Ic11Parser.MUL:
                operation = new Mul(tempVar, operand1, operand2);
                break;
            case Ic11Parser.DIV:
                operation = new Div(tempVar, operand1, operand2);
                break;
            //case Ic11Parser.MOD:
            //    operation = new Mod(tempVar, operand1, operand2);
            //    break;
            case Ic11Parser.AND:
                operation = new And(tempVar, operand1, operand2);
                break;
            case Ic11Parser.OR:
                operation = new Or(tempVar, operand1, operand2);
                break;
            //case Ic11Parser.EQ:
            //    operation = new Eq(tempVar, operand1, operand2);
            //    break;
            //case Ic11Parser.NEQ:
            //    operation = new Neq(tempVar, operand1, operand2);
            //    break;
            case Ic11Parser.GT:
                operation = new Gt(tempVar, operand1, operand2);
                break;
            case Ic11Parser.GE:
                operation = new Ge(tempVar, operand1, operand2);
                break;
            case Ic11Parser.LT:
                operation = new Lt(tempVar, operand1, operand2);
                break;
            case Ic11Parser.LE:
                operation = new Le(tempVar, operand1, operand2);
                break;
            default:
                throw new NotImplementedException();
        }

        ProgramContext.InsertOperation(operation);

        return tempVar;
    }

    public override IValue VisitLiteral(Ic11Parser.LiteralContext context)
    {
        switch (context.type.Type)
        {
            case Ic11Parser.INTEGER:
                return new Constant(int.Parse(context.GetText()));
            case Ic11Parser.REAL:
                return new Constant(double.Parse(context.GetText()));
            case Ic11Parser.BOOLEAN:
                return new Constant(context.GetText().ToLower() == "true" ? 1 : 0);
            default:
                throw new NotImplementedException();
        }
    }
}