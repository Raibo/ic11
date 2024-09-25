using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class PinName : InstructionBase
{
    public string Name;
    public string Pin;

    public PinName(Scope scope, string name, string pin) : base(scope)
    {
        Name = name;
        Pin = pin;
    }

    public override InstructionType Type => InstructionType.PinName;
    public override string Render() => $"alias {Name} {Pin}";
}
