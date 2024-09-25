using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class JumpLez : InstructionBase
{
    public string Destination;
    public IValue Param;

    public JumpLez(Scope scope, string destination, IValue param) : base(scope)
    {
        Destination = destination;
        Param = param;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"blez {Param.Render()} {Destination}";
}
