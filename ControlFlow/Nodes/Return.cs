using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Return : Node, IStatement, IExpressionContainer
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

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            if (Expression is not null)
                yield return Expression;
        }
    }
}
