using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAccess : Instruction
{
    public Variable Destination;
    public string Device;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string? MemberName;

    public MemberAccess(Variable destination, string device, string member)
    {
        Destination = destination;
        Device = device;
        Target = DeviceTarget.Device;
        MemberName = member;
    }

    public MemberAccess(Variable destination, string device, IExpression slotIndexExpr, DeviceTarget target, string? memberName)
    {
        Destination = destination;
        Device = device;
        TargetIndexExpr = slotIndexExpr;
        Target = target;
        MemberName = memberName;
    }

    public override string Render()
    {
        return Target switch
        {
            DeviceTarget.Device => RenderDevice(),
            DeviceTarget.Slots => RenderSlot(),
            DeviceTarget.Reagents => RenderReagent(),
            DeviceTarget.Stack => RenderStack(),
            _ => throw new Exception($"Unexpected device target"),
        };
    }

    private string RenderDevice()
    {
        if (MemberName == Consts.PinSetProperty)
            return $"sdse {Destination.Register} {Device}";

        return $"l {Destination.Register} {Device} {MemberName}";
    }

    private string RenderSlot()
    {
        return $"ls {Destination.Register} {Device} {TargetIndexExpr!.Render()} {MemberName}";
    }

    private string RenderReagent()
    {
        if (MemberName == Consts.RmapProperty)
            return $"rmap {Destination.Register} {Device} {TargetIndexExpr!.Render()}";

        return $"lr {Destination.Register} {Device} {MemberName} {TargetIndexExpr!.Render()}";
    }

    private string RenderStack()
    {
        return $"get {Destination.Register} {Device} {TargetIndexExpr!.Render()}";
    }
}
