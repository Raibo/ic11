using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAccess : Node, IExpression, IExpressionContainer
{
    public IExpression IndexExpr;
    public string Member;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public DeviceWithIndexAccess(IExpression indexExpr, string member)
    {
        IndexExpr = indexExpr;
        ((Node)indexExpr).Parent = this;
        Member = member;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return IndexExpr;
        }
    }
}
