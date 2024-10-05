using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAccess : Node, IExpression, IExpressionContainer
{
    public string Name;
    public string MemberName;
    public IExpression? SlotIndexExpr;

    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public MemberAccess(string name, string memberName)
    {
        Name = name;
        MemberName = memberName;
    }

    public MemberAccess(string name, IExpression slotIndexExpr, string memberName)
    {
        Name = name;
        SlotIndexExpr = slotIndexExpr;
        MemberName = memberName;
        ((Node)slotIndexExpr).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            if (SlotIndexExpr is not null)
                yield return SlotIndexExpr;
        }
    }
}
