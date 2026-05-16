using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class StatementParam1 : Node, IStatement, IExpressionContainer
{
    public string Operation;
    public INodeExpression Parameter;

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return Parameter;
        }
    }

    public StatementParam1(string operation, INodeExpression parameter)
    {
        Operation = operation;
        Parameter = parameter;
    }
}
