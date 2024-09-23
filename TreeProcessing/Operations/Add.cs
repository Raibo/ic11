namespace ic11.TreeProcessing.Operations;
public class Add : Operation
{
    public Add(string destination, string operand1, string operand2)
    {
        Destination = destination;
        Operand1 = operand1;
        Operand2 = operand2;
    }

    public override string OpCode => "Add";

    public string Destination;
    public string Operand1;
    public string Operand2;
}
