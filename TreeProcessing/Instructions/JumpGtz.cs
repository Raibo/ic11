using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class JumpGtz : InstructionBase
{
    public string Destination;
    public IValue Param;

    public JumpGtz(Scope scope, string destination, IValue param) : base(scope)
    {
        Destination = destination;
        Param = param;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"bgtz {Param.Render()} {Destination}";
}
