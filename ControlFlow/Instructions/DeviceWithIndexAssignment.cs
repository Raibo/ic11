using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAssignment : Instruction
{
    public IExpression IndexExpression;
    public string Member;
    public IExpression Expression;

    public DeviceWithIndexAssignment(IExpression indexExpression, string member, IExpression expression)
    {
        IndexExpression = indexExpression;
        Member = member;
        Expression = expression;
    }

    public override string Render() => $"s d{IndexExpression.Render()} {Member} {Expression.Render()}";
}
