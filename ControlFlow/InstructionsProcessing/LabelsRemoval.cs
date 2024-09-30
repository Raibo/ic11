using ic11.ControlFlow.Instructions;

namespace ic11.ControlFlow.InstructionsProcessing;
public static class LabelsRemoval
{
    public static void RemoveLabels(List<Instruction> instructions)
    {
        for (int i = 0; i < instructions.Count;)
        {
            if (instructions[i] is not Label label)
            {
                i++;
                continue;
            }

            var referencingJumps = instructions.Where(n => n is Jump j && j.Destination == label.Name);

            foreach (Jump jump in referencingJumps)
                jump.Destination = i.ToString();

            instructions.RemoveAt(i);
        }
    }
}
