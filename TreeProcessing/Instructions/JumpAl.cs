using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public class JumpAl : InstructionBase
{
    public string Destination;

    public JumpAl(Scope scope, string destination) : base(scope)
    {
        Destination = destination;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"jal {Destination}";
}
