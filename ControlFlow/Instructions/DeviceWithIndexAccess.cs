using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIndexAccess : Instruction
{
    public Variable Destination;
    public IExpression IndexExpression;
    public string Member;

    private const string PinSetProperty = "IsSet";

    public DeviceWithIndexAccess(Variable destination, IExpression indexExpression, string member)
    {
        Destination = destination;
        IndexExpression = indexExpression;
        Member = member;
    }

    public override string Render()
    {
        if (Member == PinSetProperty)
            return $"sdse {Destination.Register} d{IndexExpression.Render()}";

        return $"l {Destination.Register} d{IndexExpression.Render()} {Member}";
    }
}
