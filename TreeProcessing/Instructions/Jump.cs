namespace ic11.TreeProcessing.Instructions;
public class Jump : IInstruction
{
    public string Destination;

    public Jump(string destination)
    {
        Destination = destination;
    }

    public InstructionType Type => InstructionType.Jump;
    public string Render() => $"j {Destination}";
}
