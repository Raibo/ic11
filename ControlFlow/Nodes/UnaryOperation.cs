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
            "not" => ~((long)value),
            _ => throw new Exception("Unexpected unary operation type"),
        };
    }
}
