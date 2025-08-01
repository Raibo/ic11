using ic11.ControlFlow.DataHolders;
using System.Text;

namespace ic11.ControlFlow.Instructions;
public class Jump : Instruction
{
    public JumpType Type;
    public string Destination;
    public string? Argument1;
    public string? Argument2;

    public Jump(JumpType type, string destination, string? argument1 = null, string? argument2 = null)
    {
        Type = type;
        Destination = destination;
        Argument1 = argument1;
        Argument2 = argument2;
    }

    public override string Render()
    {
        var sb = new StringBuilder();
        sb.Append(Type.ToString().ToLower());

        if (Argument1 is not null)
            sb.Append($" {Argument1}");

        if (Argument2 is not null)
            sb.Append($" {Argument2}");

        sb.Append($" {Destination}");

        return sb.ToString();
    }
}
