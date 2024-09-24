using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class JumpLez : IInstruction
{
    public string Destination;
    public IValue Param;

    public JumpLez(string destination, IValue param)
    {
        Destination = destination;
        Param = param;
    }

    public InstructionType Type => InstructionType.Jump;
    public string Render() => $"blez {Param.Render()} {Destination}";
}
