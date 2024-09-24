namespace ic11.TreeProcessing.Instructions;
public class Yield : IInstruction
{
    public Yield()
    { }

    public InstructionType Type => InstructionType.Yield;
    public string Render() => $"yield";
}
