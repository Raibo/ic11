using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.TreeProcessing;

namespace ic11.ControlFlow.Nodes;
public class UnaryOperation : Node, IExpression, IExpressionContainer
{
    public IExpression Operand;
    public string Operation;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue
    {
        get
        {
            if (!Operand.CtKnownValue.HasValue)
                return null;

            return CtCalculate(Operand.CtKnownValue.Value);
        }
    }

    public UnaryOperation(IExpression operand, string operation)
    {
        Operand = operand;
        ((Node)operand).Parent = this;
        Operation = GetOperation(operation ?? throw new ArgumentNullException(nameof(operation)));
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Operand;
        }
    }

    private string GetOperation(string input)
    {
        return OperationHelper.SymbolsUnaryOpMap.TryGetValue(input, out var symbolOp)
            ? symbolOp
            : input;
    }

    private decimal CtCalculate(decimal value)
    {
        return Operation switch
        {
            "_not" => value == 0m ? 1m : 0m,
            "_neg" => value * -1m,
            "abs" => Math.Abs(value),
            "not" => ~(long)value,
            "round" => Math.Round(value),
            "ceil" => Math.Ceiling(value),
            "floor" => Math.Floor(value),
            "trunc" => Math.Truncate(value),
            "seqz" => value == 0m ? 1m : 0m,
            "snez" => value != 0m ? 1m : 0m,
            "sgez" => value >= 0m ? 1m : 0m,
            "sgtz" => value >  0m ? 1m : 0m,
            "slez" => value <= 0m ? 1m : 0m,
            "sltz" => value <  0m ? 1m : 0m,
            "sqrt" => (decimal)Math.Sqrt((double)value),
            "exp" => (decimal)Math.Exp((double)value),
            "log" => (decimal)Math.Log((double)value),
            "sin" => (decimal)Math.Sin((double)value),
            "asin" => (decimal)Math.Asin((double)value),
            "cos" => (decimal)Math.Cos((double)value),
            "acos" => (decimal)Math.Acos((double)value),
            "tan" => (decimal)Math.Tan((double)value),
            "atan" => (decimal)Math.Atan((double)value),
            "snan" => 0m, // It is not possible to have NaN value in compile time, decimal will throw exception
            "snanz" => 1m,
            _ => throw new Exception("Unexpected unary operation type"),
        };
    }
}
