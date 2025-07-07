using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAccess : Instruction
{
    public Variable Destination;
    public IExpression DeviceIndexExpr;
    public DeviceIndexType IndexType;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string? MemberName;

    public DeviceWithIndexAccess(Variable destination, IExpression deviceIndexExpr, DeviceIndexType indexType, string memberName)
    {
        Destination = destination;
        Target = DeviceTarget.Device;
        DeviceIndexExpr = deviceIndexExpr;
        IndexType = indexType;
        MemberName = memberName;
    }

    public DeviceWithIndexAccess(Variable destination, IExpression deviceIndexExpr, DeviceIndexType indexType, IExpression targetIndexExpr,
        DeviceTarget target, string? memberName)
    {
        Destination = destination;
        DeviceIndexExpr = deviceIndexExpr;
        TargetIndexExpr = targetIndexExpr;
        IndexType = indexType;
        Target = target;
        MemberName = memberName;
    }

    public override string Render()
    {
        return (IndexType, Target) switch
        {
            (DeviceIndexType.Pin, DeviceTarget.Device) => RenderDevice(),
            (DeviceIndexType.Pin, DeviceTarget.Slots) => $"ls {Destination.Register} d{DeviceIndexExpr.Render()} {TargetIndexExpr!.Render()} {MemberName}",
            (DeviceIndexType.Pin, DeviceTarget.Reagents) => RenderReagent(),
            (DeviceIndexType.Pin, DeviceTarget.Stack) => $"get {Destination.Register} d{DeviceIndexExpr.Render()} {TargetIndexExpr!.Render()}",

            (DeviceIndexType.Id, DeviceTarget.Device) => $"ld {Destination.Register} {DeviceIndexExpr.Render()} {MemberName}",
            (DeviceIndexType.Id, DeviceTarget.Stack) => $"getd {Destination.Register} {DeviceIndexExpr.Render()} {TargetIndexExpr!.Render()}",

            _ => throw new Exception($"Read operation for device access by {IndexType} and target {Target} is not supported"),
        };
    }

    private string RenderDevice()
    {
        if (MemberName == Consts.PinSetProperty)
            return $"sdse {Destination.Register} d{DeviceIndexExpr.Render()}";

        return $"l {Destination.Register} d{DeviceIndexExpr.Render()} {MemberName}";
    }

    private string RenderReagent()
    {
        if (MemberName == Consts.RmapProperty)
            return $"rmap {Destination.Register} d{DeviceIndexExpr.Render()} {TargetIndexExpr!.Render()}";

        return $"lr {Destination.Register} d{DeviceIndexExpr.Render()} {MemberName} {TargetIndexExpr!.Render()}";
    }
}
