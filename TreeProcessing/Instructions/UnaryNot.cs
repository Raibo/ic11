using ic11.TreeProcessing.Results;
using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class UnaryNot : InstructionBase
{
    Variable Destination;
    IValue Operand;

    public UnaryNot(Scope scope, Variable destination, IValue operand) : base(scope)
    {
        Destination = destination;
        Operand = operand;
    }

    public override InstructionType Type => InstructionType.OperationUnary;

    public override string Render() => $"not {Destination.Render()} {Operand.Render()}";
}
