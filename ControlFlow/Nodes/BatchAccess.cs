using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BatchAccess : Node, IExpression, IExpressionContainer
{
    public IExpression DeviceTypeHashExpr;
    public IExpression? NameHashExpr;
    public IExpression? TargetIndexExpr;
    public DeviceTarget Target;
    public string MemberName;
    public string BatchMode;

    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;
    public override int IndexSize => 2;

    public BatchAccess(IExpression deviceTypeHashExpr, IExpression? nameHashExpr, IExpression? targetIndexExpr,
        DeviceTarget target, string memberName, string batchMode)
    {
        DeviceTypeHashExpr = deviceTypeHashExpr;
        NameHashExpr = nameHashExpr;
        TargetIndexExpr = targetIndexExpr;
        
        ((Node)deviceTypeHashExpr).Parent = this;

        if (nameHashExpr is not null)
            ((Node)nameHashExpr).Parent = this;

        if (targetIndexExpr is not null)
            ((Node)targetIndexExpr).Parent = this;

        Target = target;
        MemberName = memberName;
        BatchMode = batchMode;
    }

    public IEnumerable<IExpression> Expressions
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
