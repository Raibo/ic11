using ic11.ControlFlow.NodeInterfaces;
using System.Linq.Expressions;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIdAssignment : Node, IStatement
{
    public IExpression RefIdExpr;
    public IExpression ValueExpr;
    public string Member;

    public DeviceWithIdAssignment(IExpression refIdExpr, IExpression valueExpr, string member)
    {
        RefIdExpr = refIdExpr;
        ValueExpr = valueExpr;
        ((Node)refIdExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Member = member;
    }
}
