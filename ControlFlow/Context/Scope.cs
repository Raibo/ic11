using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class Scope
{
    public readonly int Id = _staticIndex++;
    public Scope? Parent;
    public MethodDeclaration? Method;

    public int CurrentNodeOrder = 0;
    public Dictionary<string, Variable> UserDefinedVariables = new();
    public List<Variable> Variables = new();

    private static int _staticIndex = 0;

    public Variable ClaimNewVariable()
    {
        var newVar = new Variable();
        newVar.DeclareIndex = CurrentNodeOrder - 1;
        Variables.Add(newVar);
        return newVar;
    }

    public Scope CreateChildScope(MethodDeclaration? method = null)
    {
        var childScope = new Scope();
        childScope.Parent = this;

        if (method is not null)
        {
            childScope.Method = method;
            return childScope;
        }

        childScope.UserDefinedVariables = new(UserDefinedVariables);
        childScope.Variables = new(Variables);
        childScope.CurrentNodeOrder = CurrentNodeOrder;
        return childScope;
    }

    public string GetAvailableRegister(int nodeIndex)
    {
        var usedRegisters = Variables
            .Where(v => v.DeclareIndex < nodeIndex)
            .Where(v => v.LastUseIndex > nodeIndex)
            .Select(v => v.Register)
            .ToHashSet();

        var availableRegisters = Enumerable.Range(0, 15)
            .OrderBy(r => r)
            .Select(r => $"r{r}")
            .Except(usedRegisters);

        return availableRegisters.First();
    }
}
