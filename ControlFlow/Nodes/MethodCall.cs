using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MethodCall : Node, IStatement, IExpression
{
    public string Name;
    public List<IExpression> ArgumentExpressions;

    public MethodCall(string name, List<IExpression> argumentExpressions)
    {
        Name = name;
        ArgumentExpressions = argumentExpressions;

        foreach (var item in argumentExpressions)
            ((Node)item).Parent = this;
    }
}
