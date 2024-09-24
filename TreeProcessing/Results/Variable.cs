using ic11.TreeProcessing.Context;

namespace ic11.TreeProcessing.Results;

public class Variable : IValue
{
    public Variable(string name, int firstInstructionIndex, Scope scope)
    {
        Name = name;
        FirstInstructionIndex = firstInstructionIndex;
        Scope = scope;
    }

    public string Name;
    public string? Register;

    public int FirstInstructionIndex;
    public int LastInstructionIndex = -1;

    public Scope Scope;

    public void UpdateUsage(int instructionIndex)
    {
        LastInstructionIndex = instructionIndex;
    }

    public override string ToString() => "VAR " + Name;
    public string Render() => Register ?? $"Unallocated_{Name}";
}