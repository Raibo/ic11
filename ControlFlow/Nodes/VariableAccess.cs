using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class VariableAccess : Node, IExpression
{
    public string Name;
    public Variable? Variable { get; set; }

    public VariableAccess(string name)
    {
        Name = name;
    }
}
