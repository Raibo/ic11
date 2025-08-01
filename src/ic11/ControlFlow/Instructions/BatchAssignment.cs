using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class BatchAssignment : Instruction
{
    public IExpression DeviceTypeHashExpr;
    public IExpression? NameHashExpr;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string MemberName;
    public IExpression ValueExpr;

    public BatchAssignment(IExpression deviceTypeHashExpr, IExpression? nameHashExpr, IExpression? targetIndexExpression,
        IExpression valueExpr, string memberName, DeviceTarget target)
    {
        DeviceTypeHashExpr = deviceTypeHashExpr;
        TargetIndexExpr = targetIndexExpression;
        Target = target;
        MemberName = memberName;
        ValueExpr = valueExpr;
        NameHashExpr = nameHashExpr;
    }

    public override string Render()
    {
        return (Target, NameHashExpr) switch
        {
            (DeviceTarget.Device, null) => $"sb {DeviceTypeHashExpr.Render()} {MemberName} {ValueExpr.Render()}",
            (DeviceTarget.Device, not null) => $"sbn {DeviceTypeHashExpr.Render()} {NameHashExpr!.Render()} {MemberName} {ValueExpr.Render()}",

            (DeviceTarget.Slots, null) => $"sbs {DeviceTypeHashExpr.Render()} {TargetIndexExpr!.Render()} {MemberName} {ValueExpr.Render()}",

            _ => throw new Exception($"Batch write operation to devices of type{(NameHashExpr is null ? "" : " and name")} targeting {Target} is not supported"),
        };
    }
}
