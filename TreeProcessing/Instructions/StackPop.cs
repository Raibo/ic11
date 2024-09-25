using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class StackPop : InstructionBase
{
    public Variable? Destination;
    public DirectRegister? DirectRegister;

    public StackPop(Scope scope, Variable destination) : base(scope)
    {
        Destination = destination;
    }

    public StackPop(Scope scope, DirectRegister destination) : base(scope)
    {
        DirectRegister = destination;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"pop {Destination?.Render() ?? DirectRegister.Render()}";
}
