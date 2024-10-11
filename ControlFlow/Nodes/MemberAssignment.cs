using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAssignment : Node, IStatement, IExpressionContainer
{
    public string Name;
    public string MemberName;
    public IExpression? SlotIndexExpr;
    public IExpression ValueExpression;

    public MemberAssignment(string name, string memberName, IExpression valueExpression)
    {
        Name = name;
        MemberName = memberName;
        ValueExpression = valueExpression;
        ((Node)valueExpression).Parent = this;
    }

    public MemberAssignment(string name, string memberName, IExpression slotIndexExpr, IExpression valueExpression)
    {
        Name = name;
        MemberName = memberName;
        SlotIndexExpr = slotIndexExpr;
        ValueExpression = valueExpression;
        ((Node)slotIndexExpr).Parent = this;
        ((Node)valueExpression).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return ValueExpression;

            if (SlotIndexExpr is not null)
                yield return SlotIndexExpr;
        }
    }
}
