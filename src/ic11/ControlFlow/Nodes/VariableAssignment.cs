using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class VariableAssignment : Node, IStatement, IExpressionContainer
{
    public string Name;
    public INodeExpression Expression;
    public Variable? Variable;

    public VariableAssignment(string name, INodeExpression expression)
    {
        Name = name;
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
