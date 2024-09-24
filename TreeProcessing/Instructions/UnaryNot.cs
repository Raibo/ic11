using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class UnaryNot : IInstruction
{
    Variable Destination;
    IValue Operand;

    public UnaryNot(Variable destination, IValue operand)
    {
        Destination = destination;
        Operand = operand;
    }

    public InstructionType Type => InstructionType.OperationUnary;

    public string Render() => $"not {Destination.Render()} {Operand.Render()}";
}
