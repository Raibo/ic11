using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class If : Node, IStatement, IStatementsContainer, IExpressionContainer
{
    public INodeExpression Expression;
    public List<IStatement> IfStatements = new();
    public List<IStatement> ElseStatements = new();

    public IfStatementsContainer CurrentStatementsContainer = IfStatementsContainer.If;

    public List<IStatement> Statements =>
        CurrentStatementsContainer == IfStatementsContainer.If
            ? IfStatements
            : ElseStatements;

    public If(INodeExpression expression)
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
