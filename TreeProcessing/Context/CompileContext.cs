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
    public Scope CurrentScope => Scopes.Peek();
    public List<Variable> GlobalVariables = new();

    public Dictionary<string, DeclaredMethod> DeclaredMethods = new();

    public Dictionary<string, IValue> UserValuesMap => Scopes.Peek()!.UserValuesMap;
    public List<Variable> Variables => Scopes.Peek()!.Variables;

    public List<InstructionBase> Instructions = new();

    public Variable ClaimTempVar()
    {
        var newVar = Scopes.Peek()!.ClaimTempVar(Instructions.Count);
        GlobalVariables.Add(newVar);
        return newVar;
    }

    public void EnterScope(DeclaredMethod? enteredMethod = null)
    {
        var newScope = (Scopes.Count, enteredMethod) switch
        {
            (0, _) => new Scope(),
            (_, not null) => new Scope() { Method = enteredMethod },
            _ => CurrentScope.Clone(),
        };

        Scopes.Push(newScope);
    }

    public void ExitScope()
    {
        Scopes.Pop().BeforeExit();
    }
}
