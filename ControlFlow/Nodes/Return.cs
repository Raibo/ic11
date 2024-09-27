using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Return : Node, IStatement
{
    public bool HasValue;
    public IExpression? Expression;

    public Return()
    {
        HasValue = false;
    }

    public Return(IExpression expression)
    {
        HasValue = true;
        Expression = expression;
        ((Node)expression).Parent = this;
    }
}
