using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.TreeProcessing;

namespace ic11.ControlFlow.Nodes;
public class BinaryOperation : Node, IExpression, IExpressionContainer
{
    public IExpression Left;
    public IExpression Right;
    public string Operation;
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

    public BinaryOperation(IExpression left, IExpression right, string operation)
    {
        Left = left;
        Right = right;
        Operation = GetOperation(operation ?? throw new ArgumentNullException(nameof(operation)));
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

    private string GetOperation(string input)
    {
        return OperationHelper.SymbolsBinaryOpMap.TryGetValue(input, out var symbolOp)
            ? symbolOp
            : input;
    }

    private decimal CtCalculate(decimal left, decimal right)
    {
        return Operation switch
        {
            "add" => left + right,
            "sub" => left - right,
            "mul" => left * right,
            "div" when right != 0m => left / right,
            "div" when right == 0m => throw new DivideByZeroException(),
            "mod" => Modulo(left, right),
            "lt" => left < right ? 1 : 0,
            "gt" => left > right ? 1 : 0,
            "le" => left <= right ? 1 : 0,
            "ge" => left >= right ? 1 : 0,
            "eq" => left == right ? 1 : 0,
            "ne" => left != right ? 1 : 0,
            "and" => (long)decimal.Truncate(left) & (long)decimal.Truncate(right),
            "or" => (long)decimal.Truncate(left) | (long)decimal.Truncate(right),
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
