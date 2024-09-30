using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class MemberAssignment : Instruction
{
    public string Device;
    public string Member;
    public IExpression Expression;

    public MemberAssignment(string device, string member, IExpression expression)
    {
        Device = device;
        Member = member;
        Expression = expression;
    }

    public override string Render() => $"s {Device} {Member} {Expression.Render()}";
}
