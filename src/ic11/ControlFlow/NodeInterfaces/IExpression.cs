using ic11.ControlFlow.Context;
using System.Globalization;

namespace ic11.ControlFlow.NodeInterfaces;
public interface IExpression
{
    Variable? Variable { get; set; }
    decimal? CtKnownValue { get; }
    string Render() => CtKnownValue?.ToString(CultureInfo.InvariantCulture) ?? Variable!.Register;
}
