using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MemberAssignment : Node, IStatement
{
    public string Name;
    public string MemberName;
    public IExpression Expression;

    public MemberAssignment(string name, string memberName, IExpression expression)
    {
        Name = name;
        MemberName = memberName;
        Expression = expression;
        ((Node)expression).Parent = this;
    }
}
