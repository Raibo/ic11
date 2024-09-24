using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Context;
public class Scope
{
    public readonly int ScopeIndex = _staticIndex++;
    public int TempVarIndex;
    public int InitialTempVarIndex;
    public Dictionary<string, IValue> UserValuesMap = new();
    public List<Variable> Variables = new();
    public Stack<string> AvailableRegisters = new(new[]  { "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7", "r8", "r9", "r10", "r11", "r12", "r13", "r14" });

    private static int _staticIndex;

    public Variable ClaimTempVar(int CurrentInstructionIndex)
    {
        var tempVar = new Variable($"t{TempVarIndex++}", CurrentInstructionIndex, this);

        Variables.Add(tempVar);
        return tempVar;
    }

    public void BeforeExit()
    {
        Console.WriteLine(" AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA ");

        for (int i = InitialTempVarIndex; i < TempVarIndex; i++)
        {
            Console.WriteLine($"{Variables[i].Name}\t{Variables[i].FirstInstructionIndex}\t{Variables[i].LastInstructionIndex}");
        }
    }

    public Scope Clone()
    {
        var newScope = new Scope();

        newScope.UserValuesMap = new Dictionary<string, IValue>(UserValuesMap);
        newScope.AvailableRegisters = new(AvailableRegisters);
        newScope.Variables = new(Variables);

        newScope.TempVarIndex = TempVarIndex;
        newScope.InitialTempVarIndex = TempVarIndex;

        return newScope;
    }
}
