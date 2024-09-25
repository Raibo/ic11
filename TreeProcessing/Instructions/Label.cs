using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class Label : InstructionBase
{
    public string Name;

    public Label(Scope scope, string name) : base(scope)
    {
        Name = name;
    }

    public override InstructionType Type => InstructionType.Label;
    public override string Render() => $"{Name}:";
}
