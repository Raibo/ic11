using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAssignment : Node, IStatement, IExpressionContainer
{
    public INodeExpression DeviceIndexExpr;
    public DeviceIndexType IndexType;
    public INodeExpression? TargetIndexExpr;
    public INodeExpression ValueExpr;
    public DeviceTarget Target;
    public string? MemberName;
    public override int IndexSize => 2;

    public DeviceWithIndexAssignment(INodeExpression deviceIndexExpr, DeviceIndexType indexType, INodeExpression valueExpr, string memberName)
    {
        DeviceIndexExpr = deviceIndexExpr;
        IndexType = indexType;
        ValueExpr = valueExpr;
        deviceIndexExpr.Parent = this;
        valueExpr.Parent = this;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        Validate();
    }

    public DeviceWithIndexAssignment(INodeExpression deviceIndexExpr, DeviceIndexType indexType, INodeExpression slotIndexExpr, INodeExpression valueExpr,
        DeviceTarget target, string? memberName)
    {
        DeviceIndexExpr = deviceIndexExpr;
        IndexType = indexType;
        TargetIndexExpr = slotIndexExpr;
        ValueExpr = valueExpr;
        deviceIndexExpr.Parent = this;
        slotIndexExpr.Parent = this;
        valueExpr.Parent = this;
        Target = target;
        MemberName = memberName;
        Validate();
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return ValueExpr;
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

        if (Target == DeviceTarget.Reagents)
            throw new Exception($"Reagents are read-only");

        // Magic properties
        if (MemberName == Consts.PinSetProperty)
            throw new Exception($"Property {Consts.PinSetProperty} is read-only");

        if (MemberName == Consts.RmapProperty)
            throw new Exception($"Property {Consts.RmapProperty} is read-only");
    }
}
