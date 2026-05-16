using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayAccess : Node, INodeExpression, IExpressionContainer
{
    public string Name;
    public INodeExpression IndexExpression;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public UserDefinedVariable? ArrayAddressVariable;

    public ArrayAccess(string name, INodeExpression indexExpression)
    {
        Name = name;
        IndexExpression = indexExpression;
        indexExpression.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return IndexExpression;
        }
    }
}
