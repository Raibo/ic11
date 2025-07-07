using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAccess : Node, IExpression, IExpressionContainer
{
    public string Name;
    public string? MemberName;
    public DeviceTarget Target;
    public IExpression? TargetIndexExpr;

    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public MemberAccess(string name, string memberName)
    {
        Name = name;
        MemberName = memberName;
        Target = DeviceTarget.Device;
        Validate();
    }

    public MemberAccess(string name, DeviceTarget target, IExpression targetIndexExpr, string? memberName)
    {
        Name = name;
        TargetIndexExpr = targetIndexExpr;
        MemberName = memberName;
        Target = target;
        ((Node)targetIndexExpr).Parent = this;
        Validate();
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
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
