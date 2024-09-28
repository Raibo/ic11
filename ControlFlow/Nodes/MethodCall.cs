using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MethodCall : Node, IStatement, IExpression, IExpressionContainer
{
    public string Name;
    public List<IExpression> ArgumentExpressions;
    public Variable? Variable { get; set; }

    public MethodCall(string name, List<IExpression> argumentExpressions)
    {
        Name = name;
        ArgumentExpressions = argumentExpressions;

        foreach (var item in argumentExpressions)
            ((Node)item).Parent = this;
    }

    public IEnumerable<IExpression> Expressions => ArgumentExpressions;
}
