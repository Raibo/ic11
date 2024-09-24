namespace ic11.TreeProcessing.Instructions;
public class Label : IInstruction
{
    public string Name;

    public Label(string name)
    {
        Name = name;
    }

    public InstructionType Type => InstructionType.Label;
    public string Render() => $"{Name}:";
}
