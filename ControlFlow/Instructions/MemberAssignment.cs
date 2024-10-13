using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAssignment : Instruction
{
    public string Device;
    public DeviceTarget Target;
    public string? MemberName;
    public IExpression? TargetIndexExpr;
    public IExpression ValueExpr;

    public MemberAssignment(string device, string memberName, IExpression valueExpr)
    {
        Device = device;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        ValueExpr = valueExpr;
    }

    public MemberAssignment(string device, DeviceTarget target, string? memberName, IExpression slotIndexExpr, IExpression valueExpr)
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
            DeviceTarget.Device => $"s {Device} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Slots => $"ss {Device} {TargetIndexExpr!.Render()} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Stack => $"put {Device} {TargetIndexExpr!.Render()} {ValueExpr.Render()}",
            _ => throw new Exception($"Unexpected device target"),
        };
    }
}
