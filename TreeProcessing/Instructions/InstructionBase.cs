using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Instructions;
public abstract class InstructionBase
{
    public Scope Scope;

    public InstructionBase(Scope scope)
    {
        Scope = scope;
    }

    public abstract InstructionType Type { get; }
    public abstract string Render();
}
