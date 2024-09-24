using ic11.TreeProcessing.Instructions;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Context;
public class CompileContext
{
    public int TempVarIndex = 0;

    public int IfCount = 0;
    public int WhileCount = 0;
    public Stack<string> CycleContinueLabels = new();
    public Stack<Scope> Scopes = new();
    public List<Variable> GlobalVariables = new();

    public Dictionary<string, IValue> UserValuesMap => Scopes.Peek()!.UserValuesMap;
    public Stack<string> AvailableRegisters => Scopes.Peek()!.AvailableRegisters;
    public List<Variable> Variables => Scopes.Peek()!.Variables;

    public List<IInstruction> Instructions = new();

    public Variable ClaimTempVar()
    {
        var newVar = Scopes.Peek()!.ClaimTempVar(Instructions.Count);
        GlobalVariables.Add(newVar);
        return newVar;
    }

    public void EnterScope(bool isMethodScope = false)
    {
        var newScope = (Scopes.Count, isMethodScope) switch
        {
            (0, _) => new Scope(),
            (_, true) => new Scope(),
            _ => Scopes.Peek().Clone(),
        };

        Scopes.Push(newScope);
    }

    public void ExitScope()
    {
        Scopes.Pop().BeforeExit();
    }
}
