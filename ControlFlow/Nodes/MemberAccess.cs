using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAccess : Node, IExpression
{
    public string Name;
    public string MemberName;
    public Variable? Variable { get; set; }
    public decimal? CtKnownValue => null;

    public MemberAccess(string name, string memberName)
    {
        Name = name;
        MemberName = memberName;
    }
}
