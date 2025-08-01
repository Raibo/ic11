using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BatchAssignment : Node, IStatement, IExpressionContainer
{
    public IExpression DeviceTypeHashExpr;
    public IExpression? NameHashExpr;
    public IExpression? TargetIndexExpr;
    public IExpression ValueExpr;
    public DeviceTarget Target;
    public string MemberName;
    public override int IndexSize => 2;

    public BatchAssignment(IExpression deviceTypeHashExpr, IExpression? nameHashExpr, IExpression? targetIndexExpression,
        IExpression valueExpr, string memberName, DeviceTarget target)
    {
        DeviceTypeHashExpr = deviceTypeHashExpr;
        NameHashExpr = nameHashExpr;
        ValueExpr = valueExpr;
        TargetIndexExpr = targetIndexExpression;
        ((Node)deviceTypeHashExpr).Parent = this;
        ((Node)valueExpr).Parent = this;

        if (nameHashExpr is not null)
            ((Node)nameHashExpr).Parent = this;
            
        if (targetIndexExpression is not null)
            ((Node)targetIndexExpression).Parent = this;

        Target = target;

        MemberName = memberName;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return ValueExpr;
            yield return DeviceTypeHashExpr;

            if (NameHashExpr is not null)
                yield return NameHashExpr;

            if (TargetIndexExpr is not null)
                yield return TargetIndexExpr;
        }
    }
}
