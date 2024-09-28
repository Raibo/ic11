using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Literal : Node, IExpression
{
    public double Value;
    public Variable? Variable { get => null; set { } }

    public Literal(double value)
    {
        Value = value;
    }
}
