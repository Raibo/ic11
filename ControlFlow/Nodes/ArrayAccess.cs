using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayAccess : Node, IExpression, IExpressionContainer
{
    public string Name;
    public IExpression IndexExpression;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public ArrayDeclaration Array;

    public ArrayAccess(string name, IExpression indexExpression)
    {
        Name = name;
        IndexExpression = indexExpression;
        ((Node)indexExpression).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return IndexExpression;
        }
    }
}
