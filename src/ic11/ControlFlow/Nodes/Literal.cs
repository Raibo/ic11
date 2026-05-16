using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Literal : Node, INodeExpression
{
    public Variable? Variable { get => null; set { } }
    public decimal? CtKnownValue { get; init; }

    public Literal(decimal value)
    {
        CtKnownValue = value;
    }
}
