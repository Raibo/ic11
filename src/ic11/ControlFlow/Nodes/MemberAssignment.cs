using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAssignment : Node, IStatement, IExpressionContainer
{
    public string Name;
    public string? MemberName;
    public DeviceTarget Target;
    public INodeExpression? TargetIndexExpr;
    public INodeExpression ValueExpression;

    public override int IndexSize => 2;

    public MemberAssignment(string name, string memberName, INodeExpression valueExpression)
    {
        Name = name;
        MemberName = memberName;
        ValueExpression = valueExpression;
        valueExpression.Parent = this;
        Target = DeviceTarget.Device;
        Validate();
    }

    public MemberAssignment(string name, DeviceTarget target, string? memberName, INodeExpression targetIndexExpr, INodeExpression valueExpression)
    {
        Name = name;
        MemberName = memberName;
        TargetIndexExpr = targetIndexExpr;
        ValueExpression = valueExpression;
        targetIndexExpr.Parent = this;
        valueExpression.Parent = this;
        Target = target;
        Validate();
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return ValueExpression;

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
