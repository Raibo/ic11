using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.TreeProcessing;

namespace ic11.ControlFlow.Nodes;
public class BinaryOperation : Node, INodeExpression, IExpressionContainer
{
    public INodeExpression Left;
    public INodeExpression Right;
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

    public BinaryOperation(INodeExpression left, INodeExpression right, string operation)
    {
        Left = left;
        Right = right;
        Operation = GetOperation(operation ?? throw new ArgumentNullException(nameof(operation)));
        left.Parent = this;
        right.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
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
            "slt" => left < right ? 1 : 0,
            "sgt" => left > right ? 1 : 0,
            "sle" => left <= right ? 1 : 0,
            "sge" => left >= right ? 1 : 0,
            "seq" => left == right ? 1 : 0,
            "sne" => left != right ? 1 : 0,
            "and" => (long)decimal.Truncate(left) & (long)decimal.Truncate(right),
            "xor" => (long)decimal.Truncate(left) ^ (long)decimal.Truncate(right),
            "or"  => (long)decimal.Truncate(left) | (long)decimal.Truncate(right),
            "nor" => ~((long)decimal.Truncate(left) | (long)decimal.Truncate(right)),
            "max" => Math.Max(left, right),
            "min" => Math.Min(left, right),
            "sll" => (ulong)decimal.Truncate(left) << (int)decimal.Truncate(right), // C# makes logic on unsigned and arithmetic on signed
            "srl" => (ulong)decimal.Truncate(left) >> (int)decimal.Truncate(right),
            "sla" => (long)decimal.Truncate(left) << (int)decimal.Truncate(right),
            "sra" => (long)decimal.Truncate(left) >> (int)decimal.Truncate(right),
            "atan2" => (decimal)Math.Atan2((double)left, (double)right),
            "sapz" => (double)Math.Abs(left) <= Math.Max((double)right * (double)Math.Abs(left), float.Epsilon * 8d) ? 1 : 0,
            "snaz" => (double)Math.Abs(left) > Math.Max((double)right * (double)Math.Abs(left), float.Epsilon * 8d) ? 1 : 0,
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
