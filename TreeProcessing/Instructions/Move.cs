using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class Move : InstructionBase
{
    public IValue Value;
    public DirectRegister DirectRegister;

    public Move(Scope scope, DirectRegister destination, IValue value) : base(scope)
    {
        DirectRegister = destination;
        Value = value;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"move {DirectRegister.Render()} {Value.Render()}";
}
