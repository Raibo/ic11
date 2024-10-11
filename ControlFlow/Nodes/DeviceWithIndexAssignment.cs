using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAssignment : Node, IStatement, IExpressionContainer
{
    public IExpression PinIndexExpr;
    public IExpression? SlotIndexExpr;
    public IExpression ValueExpr;
    public string Member;
    public override int IndexSize => 2;

    public DeviceWithIndexAssignment(IExpression pinIndexExpr, IExpression valueExpr, string member)
    {
        PinIndexExpr = pinIndexExpr;
        ValueExpr = valueExpr;
        ((Node)pinIndexExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Member = member;
    }

    public DeviceWithIndexAssignment(IExpression pinIndexExpr, IExpression slotIndexExpr, IExpression valueExpr, string member)
    {
        PinIndexExpr = pinIndexExpr;
        SlotIndexExpr = slotIndexExpr;
        ValueExpr = valueExpr;
        ((Node)pinIndexExpr).Parent = this;
        ((Node)slotIndexExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Member = member;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return ValueExpr;
            yield return PinIndexExpr;

            if (SlotIndexExpr is not null)
                yield return SlotIndexExpr;
        }
    }
}
