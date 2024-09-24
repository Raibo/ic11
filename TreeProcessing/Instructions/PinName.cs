namespace ic11.TreeProcessing.Instructions;
public class PinName : IInstruction
{
    public string Name;
    public string Pin;

    public PinName(string name, string pin)
    {
        Name = name;
        Pin = pin;
    }

    public InstructionType Type => InstructionType.PinName;
    public string Render() => $"alias {Name} {Pin}";
}
