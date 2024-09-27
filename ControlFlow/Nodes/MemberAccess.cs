namespace ic11.ControlFlow.Nodes;
public class MemberAccess : Node, IExpression
{
    public string Name;
    public string MemberName;

    public MemberAccess(string name, string memberName)
    {
        Name = name;
        MemberName = memberName;
    }
}
