using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAssignment : Instruction
{
    public readonly DeviceAddress Device;
    public readonly DeviceTarget Target;
    public readonly string? MemberName;
    public readonly IExpression? TargetIndexExpr;
    public readonly IExpression ValueExpr;

    public MemberAssignment(DeviceAddress device, string memberName, IExpression valueExpr)
    {
        Device = device;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        ValueExpr = valueExpr;
    }

    public MemberAssignment(DeviceAddress device, DeviceTarget target, string? memberName, IExpression slotIndexExpr, IExpression valueExpr)
    {
        Device = device;
        Target = target;
        MemberName = memberName;
        TargetIndexExpr = slotIndexExpr;
        ValueExpr = valueExpr;
    }

    public override string Render()
    {
        return Target switch
        {
            DeviceTarget.Device => $"s {Device.Render()} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Slots => $"ss {Device.Render()} {TargetIndexExpr!.Render()} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Stack => $"put {Device.Render()} {TargetIndexExpr!.Render()} {ValueExpr.Render()}",
            _ => throw new Exception($"Unexpected device target"),
        };
    }
}
