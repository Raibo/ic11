using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Operations;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing;
public class ProgramContext
{
    public Dictionary<string, string> PinAliases = new();
    public List<Variable> Variables = new();
    public int TempVarIndex = 0;

    public List<Operation> Operations = new();

    public Stack<int> WhileLabels = new();
    public int WhileCount = 0;

    // test stuff
    public int TestDepth = 0;
    public int TestIfCount = 0;
    public int TestWhileCount = 0;
    public List<UserValue> UserValues = new();

    public List<IInstruction> Instructions = new();

    public Variable ClaimTempVar()
    {
        var tempVar = new Variable($"t{TempVarIndex++}");
        Variables.Add(tempVar);
        return tempVar;
    }

    public void InsertOperation(Operation operation)
    {
        Operations.Add(operation);
    }
}
