using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class DeviceWithIndexAssignment : Node, IStatement, IExpressionContainer
{
    public IExpression IndexExpr;
    public IExpression ValueExpr;
    public string Member;

    public DeviceWithIndexAssignment(IExpression indexExpr, IExpression valueExpr, string member)
    {
        IndexExpr = indexExpr;
        ValueExpr = valueExpr;
        ((Node)indexExpr).Parent = this;
        ((Node)valueExpr).Parent = this;
        Member = member;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return IndexExpr;
            yield return ValueExpr;
        }
    }
}
