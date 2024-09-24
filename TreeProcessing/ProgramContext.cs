using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing;
public class ProgramContext
{
    public List<Variable> Variables = new();
    public int TempVarIndex = 0;

    // test stuff
    public int IfCount = 0;
    public int WhileCount = 0;
    public Dictionary<string, IValue> UserValuesMap = new();
    public Stack<string> CycleContinueLabels = new();

    public List<IInstruction> Instructions = new();

    public Variable ClaimTempVar()
    {
        var tempVar = new Variable($"t{TempVarIndex++}");
        Variables.Add(tempVar);
        return tempVar;
    }
}
