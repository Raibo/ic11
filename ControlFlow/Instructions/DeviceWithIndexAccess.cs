using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAccess : Instruction
{
    public Variable Destination;
    public IExpression PinIndexExpr;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string? MemberName;

    public DeviceWithIndexAccess(Variable destination, IExpression pinIndexExpr, string memberName)
    {
        Destination = destination;
        Target = DeviceTarget.Device;
        PinIndexExpr = pinIndexExpr;
        MemberName = memberName;
    }

    public DeviceWithIndexAccess(Variable destination, IExpression pinIndexExpr, IExpression slotIndexExpr,
        DeviceTarget target, string? memberName)
    {
        Destination = destination;
        PinIndexExpr = pinIndexExpr;
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
            return $"sdse {Destination.Register} d{PinIndexExpr.Render()}";

        return $"l {Destination.Register} d{PinIndexExpr.Render()} {MemberName}";
    }

    private string RenderSlot()
    {
        return $"ls {Destination.Register} d{PinIndexExpr.Render()} {TargetIndexExpr!.Render()} {MemberName}";
    }

    private string RenderReagent()
    {
        if (MemberName == Consts.RmapProperty)
            return $"rmap {Destination.Register} d{PinIndexExpr.Render()} {TargetIndexExpr!.Render()}";

        return $"lr {Destination.Register} d{PinIndexExpr.Render()} {MemberName} {TargetIndexExpr!.Render()}";
    }

    private string RenderStack()
    {
        return $"get {Destination.Register} d{PinIndexExpr.Render()} {TargetIndexExpr!.Render()}";
    }
}
