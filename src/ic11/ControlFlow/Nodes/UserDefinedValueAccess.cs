using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class UserDefinedValueAccess : Node, INodeExpression
{
    public string Name;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue { get; set; }

    public UserDefinedValueAccess(string name)
    {
        Name = name;
    }
}
