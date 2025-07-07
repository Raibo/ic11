using ic11.ControlFlow.Context;
using ic11.ControlFlow.Nodes;
using System.Globalization;

namespace ic11.ControlFlow.NodeInterfaces;
public interface IExpression
{
    Variable? Variable { get; set; }
    decimal? CtKnownValue { get; }
    string Render() => CtKnownValue?.ToString(CultureInfo.InvariantCulture) ?? Variable!.Register;

    public int FirstIndexInTree => GetFirstExpressionIndex(this);

    private int GetFirstExpressionIndex(IExpression ex)
    {
        var index = ((Node)ex).IndexInScope;

        if (ex is IExpressionContainer ec)
        {
            foreach (var item in ec.Expressions)
                index = Math.Min(index, GetFirstExpressionIndex(item));
        }

        return index;
    }
}
