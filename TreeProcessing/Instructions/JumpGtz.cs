using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class JumpGtz : IInstruction
{
    public string Destination;
    public IValue Param;

    public JumpGtz(string destination, IValue param)
    {
        Destination = destination;
        Param = param;
    }

    public InstructionType Type => InstructionType.Jump;
    public string Render() => $"bgtz {Param.Render()} {Destination}";
}
