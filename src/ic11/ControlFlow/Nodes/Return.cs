using System.Diagnostics.CodeAnalysis;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Return : Node, IStatement, IExpressionContainer
{
    [MemberNotNullWhen(true, nameof(Expression))]
    public bool HasValue => Expression != null;
    public IExpression? Expression { get; }

    public Return()
    {
        Expression = null;
    }

    public Return(IExpression expression)
    {
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
