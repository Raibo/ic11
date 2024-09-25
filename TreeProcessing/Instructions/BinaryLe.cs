using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class BinaryLe : BinaryBase
{
    public BinaryLe(Scope scope, Variable destination, IValue operand1, IValue operand2) : base(scope, destination, operand1, operand2)
    { }

    public override string Render() => Render("sle");
}
