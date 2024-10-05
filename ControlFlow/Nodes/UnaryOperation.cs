using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class UnaryOperation : Node, IExpression, IExpressionContainer
{
    public IExpression Operand;
    public UnaryOperationType Type;
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

    public UnaryOperation(IExpression operand, UnaryOperationType type)
    {
        Operand = operand;
        ((Node)operand).Parent = this;
        Type = type;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Operand;
        }
    }

    private decimal CtCalculate(decimal value)
    {
        return Type switch
        {
            UnaryOperationType.Not => value == 0m ? 1m : 0m,
            UnaryOperationType.Minus => value * -1m,
            UnaryOperationType.Abs => Math.Abs(value),
            _ => throw new Exception("Unexpected unary operation type"),
        };
    }
}
