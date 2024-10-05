using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAccess : Node, IExpression, IExpressionContainer
{
    public IExpression DeviceIndexExpr;
    public IExpression? SlotIndexExpr;
    public string Member;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public DeviceWithIndexAccess(IExpression deviceIndexExpr, string member)
    {
        DeviceIndexExpr = deviceIndexExpr;
        ((Node)deviceIndexExpr).Parent = this;
        Member = member;
    }

    public DeviceWithIndexAccess(IExpression deviceIndexExpr, IExpression slotIndexExpr, string member)
    {
        DeviceIndexExpr = deviceIndexExpr;
        SlotIndexExpr = slotIndexExpr;
        ((Node)deviceIndexExpr).Parent = this;
        ((Node)slotIndexExpr).Parent = this;
        Member = member;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return DeviceIndexExpr;
            
            if (SlotIndexExpr is not null)
                yield return SlotIndexExpr;
        }
    }
}
