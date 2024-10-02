using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayAssignment : Node, IStatement, IExpressionContainer
{
    public string Name;
    public IExpression IndexExpression;
    public IExpression ValueExpression;
    public Variable? Variable;
    public ArrayDeclaration Array;

    public ArrayAssignment(string name, IExpression indexExpression, IExpression valueExpression)
    {
        Name = name;

        IndexExpression = indexExpression;
        ValueExpression = valueExpression;

        ((Node)indexExpression).Parent = this;
        ((Node)valueExpression).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return IndexExpression;
            yield return ValueExpression;
        }
    }
}
