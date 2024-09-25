using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public abstract class BinaryBase : InstructionBase
{
    IValue Operand1;
    IValue Operand2;
    Variable Destination;

    public BinaryBase(Scope scope, Variable destination, IValue operand1, IValue operand2) : base(scope)
    {
        Operand1 = operand1;
        Operand2 = operand2;
        Destination = destination;
    }

    public override InstructionType Type => InstructionType.OperationBinary;
    public string Render(string operation) => $"{operation} {Destination.Render()} {Operand1.Render()} {Operand2.Render()}";
}
