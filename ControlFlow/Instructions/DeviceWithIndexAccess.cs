using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAccess : Instruction
{
    public Variable Destination;
    public IExpression PinIndexExpr;
    public IExpression? SlotIndexExpr;
    public string Member;

    private const string PinSetProperty = "IsSet";

    public DeviceWithIndexAccess(Variable destination, IExpression pinIndexExpr, string member)
    {
        Destination = destination;
        PinIndexExpr = pinIndexExpr;
        Member = member;
    }

    public DeviceWithIndexAccess(Variable destination, IExpression pinIndexExpr, IExpression slotIndexExpr, string member)
    {
        Destination = destination;
        PinIndexExpr = pinIndexExpr;
        SlotIndexExpr = slotIndexExpr;
        Member = member;
    }

    public override string Render()
    {
        return (isPinSet: Member == PinSetProperty, isSlotDefined: SlotIndexExpr is not null) switch
        {
            (isPinSet: true, isSlotDefined: true) => throw new Exception($"IsSet property is not relevant for slot access"),
            (isPinSet: true, isSlotDefined: false) => $"sdse {Destination.Register} d{PinIndexExpr.Render()}",
            (isPinSet: false, isSlotDefined: true) => $"ls {Destination.Register} d{PinIndexExpr.Render()} {SlotIndexExpr!.Render()} {Member}",
            (isPinSet: false, isSlotDefined: false) => $"l {Destination.Register} d{PinIndexExpr.Render()} {Member}",
        };
    }
}
