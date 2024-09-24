using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class BinarySub : BinaryBase
{
    public BinarySub(Variable destination, IValue operand1, IValue operand2) : base(destination, operand1, operand2)
    { }

    public override string Render() => Render("sub");
}
