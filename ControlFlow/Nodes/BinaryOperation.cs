using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BinaryOperation : Node, IExpression, IExpressionContainer
{
    public IExpression Left;
    public IExpression Right;
    public BinaryOperationType Type;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue
    {
        get
        {
            if (!Left.CtKnownValue.HasValue || !Right.CtKnownValue.HasValue)
                return null;

            return CtCalculate(Left.CtKnownValue.Value, Right.CtKnownValue.Value);
        }
    }

    public BinaryOperation(IExpression left, IExpression right, BinaryOperationType type)
    {
        Left = left;
        Right = right;
        Type = type;
        ((Node)left).Parent = this;
        ((Node)right).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Left;
            yield return Right;
        }
    }

    private decimal CtCalculate(decimal left, decimal right)
    {
        return Type switch
        {
            BinaryOperationType.Add => left + right,
            BinaryOperationType.Sub => left - right,
            BinaryOperationType.Mul => left * right,
            BinaryOperationType.Div when right != 0m => left / right,
            BinaryOperationType.Div when right == 0m => throw new DivideByZeroException(),
            BinaryOperationType.Mod => Modulo(left, right),
            BinaryOperationType.Lt => left < right ? 1 : 0,
            BinaryOperationType.GT => left > right ? 1 : 0,
            BinaryOperationType.Le => left <= right ? 1 : 0,
            BinaryOperationType.Ge => left >= right ? 1 : 0,
            BinaryOperationType.Eq => left == right ? 1 : 0,
            BinaryOperationType.Ne => left != right ? 1 : 0,
            BinaryOperationType.And => (long)decimal.Truncate(left) & (long)decimal.Truncate(right),
            BinaryOperationType.Or => (long)decimal.Truncate(left) | (long)decimal.Truncate(right),
            _ => throw new Exception("Unexpected binary operation type"),
        };
    }

    private decimal Modulo(decimal x, decimal m)
    {
        // IC10 returns 0 for mod, e.g. "mod r0 12345 0" will put 0 into r0
        if (m == 0)
            return 0m;

        var r = x % m;
        return r < 0 ? r + m : r;
    }
}
