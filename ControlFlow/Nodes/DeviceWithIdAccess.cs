using ic11.ControlFlow.NodeInterfaces;
using System.Linq.Expressions;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIdAccess : Node, IExpression
{
    public IExpression RefIdExpr;
    public string Member;

    public DeviceWithIdAccess(IExpression refIdExpr, string member)
    {
        RefIdExpr = refIdExpr;
        ((Node)refIdExpr).Parent = this;
        Member = member;
    }
}
