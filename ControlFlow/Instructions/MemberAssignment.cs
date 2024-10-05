using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAssignment : Instruction
{
    public string Device;
    public string Member;
    public IExpression? SlotIndexExpr;
    public IExpression ValueExpr;

    private const string PinSetProperty = "IsSet";

    public MemberAssignment(string device, string member, IExpression valueExpr)
    {
        Device = device;
        Member = member;
        ValueExpr = valueExpr;
    }

    public MemberAssignment(string device, string member, IExpression slotIndexExpr, IExpression valueExpr)
    {
        Device = device;
        Member = member;
        SlotIndexExpr = slotIndexExpr;
        ValueExpr = valueExpr;
    }

    public override string Render()
    {
        return (isPinSet: Member == PinSetProperty, isSlotDefined: SlotIndexExpr is not null) switch
        {
            (isPinSet: true, isSlotDefined: true) => throw new Exception($"IsSet property is not relevant for slot access"),
            (isPinSet: true, isSlotDefined: false) => throw new Exception($"IsSet property cannot be written to"),
            (isPinSet: false, isSlotDefined: true) => $"ss {Device} {SlotIndexExpr!.Render()} {Member} {ValueExpr.Render()}",
            (isPinSet: false, isSlotDefined: false) => $"s {Device} {Member} {ValueExpr.Render()}",
        };
    }
}
