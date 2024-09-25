using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class StackPush : InstructionBase
{
    public IValue Param;

    public StackPush(Scope scope, IValue param) : base(scope)
    {
        Param = param;
    }

    public override InstructionType Type => InstructionType.Jump;
    public override string Render() => $"push {Param.Render()}";
}
