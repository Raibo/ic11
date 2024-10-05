using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAccess : Instruction
{
    public Variable Destination;
    public string Device;
    public IExpression? SlotIndexExpr;
    public string Member;

    private const string PinSetProperty = "IsSet";

    public MemberAccess(Variable destination, string device, string member)
    {
        Destination = destination;
        Device = device;
        Member = member;
    }

    public MemberAccess(Variable destination, string device, IExpression slotIndexExpr, string member)
    {
        Destination = destination;
        Device = device;
        SlotIndexExpr = slotIndexExpr;
        Member = member;
    }

    public override string Render()
    {
        return (isPinSet: Member == PinSetProperty, isSlotDefined: SlotIndexExpr is not null) switch
        {
            (isPinSet: true, isSlotDefined: true) => throw new Exception($"IsSet property is not relevant for slot access"),
            (isPinSet: true, isSlotDefined: false) => $"sdse {Destination.Register} {Device}",
            (isPinSet: false, isSlotDefined: true) => $"ls {Destination.Register} {Device} {SlotIndexExpr!.Render()} {Member}",
            (isPinSet: false, isSlotDefined: false) => $"l {Destination.Register} {Device} {Member}",
        };
    }
}
