using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Operations;
public class Negation : Operation
{
    public Negation(Variable destination, IValue operand1)
    {
        Destination = destination;
        Operand1 = operand1;
    }

    public override string OpCode => "NEG";

    public Variable Destination { get; }
    public IValue Operand1 { get; }

    public override string ToString() => $"{Destination} = !{Operand1}";
}
