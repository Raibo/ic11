using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class NullaryOperation : Node, INodeExpression
{
    public string Operation;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public NullaryOperation(string operation)
    {
        Operation = operation ?? throw new ArgumentNullException(nameof(operation));
    }
}
