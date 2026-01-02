using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayAssignment : Node, IStatement, IExpressionContainer
{
    public string Name;
    public INodeExpression IndexExpression;
    public INodeExpression ValueExpression;
    public Variable? Variable;
    public UserDefinedVariable? ArrayAddressVariable;

    public override int IndexSize => 2;

    public ArrayAssignment(string name, INodeExpression indexExpression, INodeExpression valueExpression)
    {
        Name = name;

        IndexExpression = indexExpression;
        ValueExpression = valueExpression;

        indexExpression.Parent = this;
        valueExpression.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return IndexExpression;
            yield return ValueExpression;
        }
    }
}
