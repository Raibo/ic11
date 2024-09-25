using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class Yield : InstructionBase
{
    public Yield(Scope scope) : base(scope)
    { }

    public override InstructionType Type => InstructionType.Yield;
    public override string Render() => $"yield";
}
