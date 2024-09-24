namespace ic11.TreeProcessing.Results;

public interface IValue
{
    string Render();
    void UpdateUsage(int instructionIndex) { }
}