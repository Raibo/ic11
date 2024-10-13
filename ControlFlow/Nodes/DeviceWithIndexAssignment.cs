using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAssignment : Node, IStatement, IExpressionContainer
{
    public IExpression PinIndexExpr;
    public IExpression? TargetIndexExpr;
    public IExpression ValueExpr;
    public DeviceTarget Target;
    public string? MemberName;
    public override int IndexSize => 2;

    public DeviceWithIndexAssignment(IExpression pinIndexExpr, IExpression valueExpr, string memberName)
    {
        PinIndexExpr = pinIndexExpr;
        ValueExpr = valueExpr;
        ((Node)pinIndexExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Target = DeviceTarget.Device;
        MemberName = memberName;
        Validate();
    }

    public DeviceWithIndexAssignment(IExpression pinIndexExpr, IExpression slotIndexExpr, IExpression valueExpr,
        DeviceTarget target, string? memberName)
    {
        PinIndexExpr = pinIndexExpr;
        TargetIndexExpr = slotIndexExpr;
        ValueExpr = valueExpr;
        ((Node)pinIndexExpr).Parent = this;
        ((Node)slotIndexExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Target = target;
        MemberName = memberName;
        Validate();
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return ValueExpr;
            yield return PinIndexExpr;

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
