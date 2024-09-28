using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIdAccess : Node, IExpression, IExpressionContainer
{
    public IExpression RefIdExpr;
    public string Member;
    public Variable? Variable { get; set; }

    public DeviceWithIdAccess(IExpression refIdExpr, string member)
    {
        RefIdExpr = refIdExpr;
        ((Node)refIdExpr).Parent = this;
        Member = member;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return RefIdExpr;
        }
    }
}
