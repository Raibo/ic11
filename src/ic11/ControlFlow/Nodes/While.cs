using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class While : Node, IStatement, IStatementsContainer, IExpressionContainer
{
    public IExpression Expression;
    public List<IStatement> Statements { get; set; } = new();

    public While(IExpression expression)
    {
        Expression = expression;
        ((Node)expression).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Expression;
        }
    }
}
