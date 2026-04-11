using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAccess : Instruction
{
    public readonly Variable Destination;
    public readonly DeviceAddress Device;
    public readonly IExpression? TargetIndexExpr;
    public readonly DeviceTarget Target;
    public readonly string? MemberName;

    public MemberAccess(Variable destination, DeviceAddress device, string member)
    {
        Destination = destination;
        Device = device;
        Target = DeviceTarget.Device;
        MemberName = member;
    }

    public MemberAccess(Variable destination, DeviceAddress device, IExpression slotIndexExpr, DeviceTarget target, string? memberName)
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
            return $"sdse {Destination.Register} {Device.Render()}";

        return $"l {Destination.Register} {Device.Render()} {MemberName}";
    }

    private string RenderSlot()
    {
        return $"ls {Destination.Register} {Device.Render()} {TargetIndexExpr!.Render()} {MemberName}";
    }

    private string RenderReagent()
    {
        if (MemberName == Consts.RmapProperty)
            return $"rmap {Destination.Register} {Device} {TargetIndexExpr!.Render()}";

        return $"lr {Destination.Register} {Device.Render()} {MemberName} {TargetIndexExpr!.Render()}";
    }

    private string RenderStack()
    {
        return $"get {Destination.Register} {Device.Render()} {TargetIndexExpr!.Render()}";
    }
}