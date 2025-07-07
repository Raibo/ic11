using ic11.ControlFlow.Instructions;

namespace ic11.ControlFlow.InstructionsProcessing;
public static class UselessMoveRemover
{
    public static void Remove(List<Instruction> instructions)
    {
        for (int i = instructions.Count - 1; i >= 0; i--)
        {
            if (instructions[i] is Move mv && mv.Expression.Render() == (mv.Register ?? mv.Destination!.Register))
                instructions.RemoveAt(i);
        }
    }
}
