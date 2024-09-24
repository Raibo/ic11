using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Operations;
public class Minus : Operation
{
    public Minus(Variable destination, IValue operand1)
    {
        Destination = destination;
        Operand1 = operand1;
    }

    public override string OpCode => "MINUS";

    public Variable Destination;
    public IValue Operand1;

    public override string ToString() => $"{Destination} = -{Operand1}";
}
