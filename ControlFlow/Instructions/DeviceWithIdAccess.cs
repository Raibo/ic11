using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class DeviceWithIdAccess : Instruction
{
    public Variable Destination;
    public IExpression IdExpression;
    public string Member;

    private const string PinSetProperty = "IsSet";

    public DeviceWithIdAccess(Variable destination, IExpression idExpression, string member)
    {
        Destination = destination;
        IdExpression = idExpression;
        Member = member;
    }

    public override string Render()
    {
        if (Member == PinSetProperty)
            throw new Exception("Impossible to check that a device ID exists");

        return $"ld {Destination.Register} {IdExpression.Render()} {Member}";
    }
}
