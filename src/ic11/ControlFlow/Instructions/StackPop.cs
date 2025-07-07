using ic11.ControlFlow.Context;

namespace ic11.ControlFlow.Instructions;
public class StackPop : Instruction
{
    public Variable? Destination;
    public string? Register;

    public StackPop(Variable destination)
    {
        Destination = destination;
    }

    public StackPop(string? register)
    {
        Register = register;
    }

    public override string Render() => $"pop {Register ?? Destination!.Register}";
}
