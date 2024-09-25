using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class Jump : InstructionBase
{
    public string Destination;

    public Jump(Scope scope, string destination) : base(scope)
    {
        Destination = destination;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"j {Destination}";
}
