using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAccess : Node, IExpression, IExpressionContainer
{
    public IExpression DeviceIndexExpr;
    public DeviceIndexType IndexType;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string? MemberName;

    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public override int IndexSize => 2;

    public DeviceWithIndexAccess(IExpression deviceIndexExpr, DeviceIndexType indexType, string memberName)
    {
        DeviceIndexExpr = deviceIndexExpr;
        ((Node)deviceIndexExpr).Parent = this;
        IndexType = indexType;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        Validate();
    }

    public DeviceWithIndexAccess(IExpression deviceIndexExpr, DeviceIndexType indexType, IExpression targetIndexExpr,
        DeviceTarget target, string? memberName)
    {
        DeviceIndexExpr = deviceIndexExpr;
        TargetIndexExpr = targetIndexExpr;
        ((Node)deviceIndexExpr).Parent = this;
        ((Node)targetIndexExpr).Parent = this;
        IndexType = indexType;
        Target = target;
        MemberName = memberName;
        Validate();
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return DeviceIndexExpr;

            if (TargetIndexExpr is not null)
                yield return TargetIndexExpr;
        }
    }

    private void Validate()
    {
        if (Target != DeviceTarget.Stack && string.IsNullOrWhiteSpace(MemberName))
            throw new Exception($"Expected member name for device interaction");

        if (Target == DeviceTarget.Stack && MemberName is not null)
            throw new Exception($"Unexpected member name for device stack interaction");

        if (Target != DeviceTarget.Device && TargetIndexExpr is null)
            throw new Exception($"Expected target index expression");

        if (Target == DeviceTarget.Device && TargetIndexExpr is not null)
            throw new Exception($"Unexpected target index expression");

        // Magic properties
        if (Target != DeviceTarget.Device && MemberName == Consts.PinSetProperty)
            throw new Exception($"Property {Consts.PinSetProperty} is only relevant for device itself");

        if (Target != DeviceTarget.Reagents && MemberName == Consts.RmapProperty)
            throw new Exception($"Property {Consts.RmapProperty} is only relevant for reagents");
    }
}
