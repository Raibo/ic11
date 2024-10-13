using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAssignment : Instruction
{
    public IExpression PinIndexExpr;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string? MemberName;
    public IExpression ValueExpr;

    public DeviceWithIndexAssignment(IExpression pinIndexExpression, string memberName, IExpression expression)
    {
        PinIndexExpr = pinIndexExpression;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        ValueExpr = expression;
    }

    public DeviceWithIndexAssignment(IExpression pinIndexExpression, IExpression targetIndexExpr, DeviceTarget target,
        string? memberName, IExpression expression)
    {
        PinIndexExpr = pinIndexExpression;
        TargetIndexExpr = targetIndexExpr;
        Target = target;
        MemberName = memberName;
        ValueExpr = expression;
    }

    public override string Render()
    {
        return Target switch
        {
            DeviceTarget.Device => $"s d{PinIndexExpr.Render()} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Slots => $"ss d{PinIndexExpr.Render()} {TargetIndexExpr!.Render()} {MemberName} {ValueExpr.Render()}",
            DeviceTarget.Stack => $"put d{PinIndexExpr.Render()} {TargetIndexExpr!.Render()} {ValueExpr.Render()}",
            _ => throw new Exception($"Unexpected device target"),
        };
    }
}
