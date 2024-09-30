using ic11.ControlFlow.Context;

namespace ic11.ControlFlow.Instructions;
public class MemberAccess : Instruction
{
    public Variable Destination;
    public string Device;
    public string Member;

    private const string PinSetProperty = "IsSet";

    public MemberAccess(Variable destination, string device, string member)
    {
        Destination = destination;
        Device = device;
        Member = member;
    }

    public override string Render()
    {
        if (Member == PinSetProperty)
            return $"sdse {Destination.Register} {Device}";

        return $"l {Destination.Register} {Device} {Member}";
    }
}
