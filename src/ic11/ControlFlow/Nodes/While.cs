using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class While : Node, IStatement, IStatementsContainer, IExpressionContainer
{
    public INodeExpression Expression;
    public List<IStatement> Statements { get; set; } = new();

    public While(INodeExpression expression)
    {
        Expression = expression;
        expression.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return Expression;
        }
    }
}
