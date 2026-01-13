using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Return : Node, IStatement, IExpressionContainer
{
    public bool HasValue;
    public INodeExpression? Expression;

    public Return()
    {
        HasValue = false;
    }

    public Return(INodeExpression expression)
    {
        HasValue = true;
        Expression = expression;
        expression.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            if (Expression is not null)
                yield return Expression;
        }
    }
}
