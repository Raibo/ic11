using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Context;
public class Scope
{
    public readonly int Id = _staticIndex++;
    public static int TempVarIndex;
    public int InitialTempVarIndex;
    public Dictionary<string, IValue> UserValuesMap = new();
    public List<Variable> Variables = new();
    public List<Variable> LocalVariables = new();
    
    private static int _staticIndex;

    public Variable ClaimTempVar(int CurrentInstructionIndex)
    {
        var tempVar = new Variable($"t{TempVarIndex++}", CurrentInstructionIndex, this);

        Variables.Add(tempVar);
        LocalVariables.Add(tempVar);
        return tempVar;
    }

    public void BeforeExit()
    {
        
    }

    public Scope Clone()
    {
        var newScope = new Scope();

        newScope.UserValuesMap = new Dictionary<string, IValue>(UserValuesMap);
        newScope.Variables = new(Variables);

        newScope.InitialTempVarIndex = TempVarIndex;

        return newScope;
    }
}
