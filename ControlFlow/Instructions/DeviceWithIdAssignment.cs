using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIdAssignment : Instruction
{
    public IExpression IdExpression;
    public string Member;
    public IExpression Expression;

    public DeviceWithIdAssignment(IExpression idExpression, string member, IExpression expression)
    {
        IdExpression = idExpression;
        Member = member;
        Expression = expression;
    }

    public override string Render() => $"sd {IdExpression.Render()} {Member} {Expression.Render()}";
}
