using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class BinaryAdd : BinaryBase
{
    public BinaryAdd(Variable destination, IValue operand1, IValue operand2) : base(destination, operand1, operand2)
    { }

    public override string Render() => Render("add");
}
