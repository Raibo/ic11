using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BatchAccess : Node, INodeExpression, IExpressionContainer
{
    public INodeExpression DeviceTypeHashExpr;
    public INodeExpression? NameHashExpr;
    public INodeExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string MemberName;
    public string BatchMode;

    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public override int IndexSize => 2;

    public BatchAccess(INodeExpression deviceTypeHashExpr, INodeExpression? nameHashExpr, INodeExpression? targetIndexExpr,
        DeviceTarget target, string memberName, string batchMode)
    {
        DeviceTypeHashExpr = deviceTypeHashExpr;
        NameHashExpr = nameHashExpr;
        TargetIndexExpr = targetIndexExpr;
        
        deviceTypeHashExpr.Parent = this;

        if (nameHashExpr is not null)
            nameHashExpr.Parent = this;

        if (targetIndexExpr is not null)
            targetIndexExpr.Parent = this;

        Target = target;
        MemberName = memberName;
        BatchMode = batchMode;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return DeviceTypeHashExpr;

            if (NameHashExpr is not null)
                yield return NameHashExpr;

            if (TargetIndexExpr is not null)
                yield return TargetIndexExpr;
        }
    }
}
