namespace ic11.TreeProcessing.Instructions;
public interface IInstruction
{
    InstructionType Type { get; }
    string Render();
}
