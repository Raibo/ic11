using ic11.ControlFlow.Context;

namespace ic11.ControlFlow.NodeInterfaces;
public interface IExpression
{
    Variable? Variable { get; set; }
    decimal? CtKnownValue { get; }
}
