using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class StatementParam1 : Node, IStatement, IExpressionContainer
{
    public string Operation;
    public IExpression Parameter;

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Parameter;
        }
    }

    public StatementParam1(string operation, IExpression parameter)
    {
        Operation = operation;
        Parameter = parameter;
    }
}
