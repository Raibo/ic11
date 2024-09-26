using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Context;
public class Scope
{
    public readonly int Id = _staticIndex++;
    public static int VarIndex;
    public int InitialTempVarIndex;
    public DeclaredMethod? Method;

    public Dictionary<string, IValue> UserValuesMap = new();
    public List<Variable> Variables = new();
    public List<Variable> LocalVariables = new();
    
    private static int _staticIndex;

    public Variable ClaimNewVar(int CurrentInstructionIndex)
    {
        var newVar = new Variable($"var{VarIndex++}", CurrentInstructionIndex, this);

        Variables.Add(newVar);
        LocalVariables.Add(newVar);
        return newVar;
    }

    public void BeforeExit()
    {
        
    }

    public Scope Clone()
    {
        var newScope = new Scope();

        newScope.UserValuesMap = new Dictionary<string, IValue>(UserValuesMap);
        newScope.Variables = new(Variables);

        newScope.InitialTempVarIndex = VarIndex;
        newScope.Method = Method;

        return newScope;
    }
}
