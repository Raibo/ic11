using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class Scope
{
    public readonly int Id = _staticIndex++;
    public Scope? Parent;
    public List<Scope> Children = new();
    public MethodDeclaration? Method;

    public int CurrentNodeOrder = 0;
    public List<Variable> Variables = new();

    private static int _staticIndex = 0;

    public Dictionary<string, UserDefinedVariable> UserDefinedVariables = new();
    public Dictionary<string, UserDefinedConstant> UserDefinedConstants = new();
    public Dictionary<string, UserDefinedConstant> GlobalUserDefinedConstants = new();

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
        Children.Add(childScope);
        childScope.Parent = this;
        childScope.GlobalUserDefinedConstants = GlobalUserDefinedConstants;

        if (method is not null)
        {
            childScope.Method = method;
            return childScope;
        }

        childScope.UserDefinedVariables = new(UserDefinedVariables);
        childScope.Variables = new(Variables);
        childScope.CurrentNodeOrder = CurrentNodeOrder;
        childScope.Method = Method;
        return childScope;
    }

    public string GetAvailableRegister(int nodeIndex)
    {
        var usedRegisters = GetUsedRegisters(nodeIndex);

        var availableRegisters = Enumerable.Range(0, 15)
            .OrderBy(r => r)
            .Select(r => $"r{r}")
            .Except(usedRegisters);

        return availableRegisters.First();
    }

    public List<string> GetUsedRegisters(int nodeIndex) =>
        Variables
            .Where(v => v.DeclareIndex < nodeIndex)
            .Where(v => v.LastReferencedIndex > nodeIndex)
            .Select(v => v.Register)
            .ToList();

    public void AddUserVariable(UserDefinedVariable variable)
    {
        if (GlobalUserDefinedConstants.ContainsKey(variable.Name) || UserDefinedConstants.ContainsKey(variable.Name)
            || UserDefinedVariables.ContainsKey(variable.Name))
        {
            throw new Exception($"'{variable.Name}' already exists");
        }

        UserDefinedVariables[variable.Name] = variable;

        foreach (var item in Children)
            item.AddUserVariable(variable);
    }

    public bool TryGetUserVariable(string name, out UserDefinedVariable variable) =>
        UserDefinedVariables.TryGetValue(name, out variable!);

    public void AddUserConstant(UserDefinedConstant constant)
    {
        var placeToAdd = Parent is null
            ? GlobalUserDefinedConstants
            : UserDefinedConstants;

        if (GlobalUserDefinedConstants.ContainsKey(constant.Name) || UserDefinedConstants.ContainsKey(constant.Name)
            || UserDefinedVariables.ContainsKey(constant.Name))
        {
            throw new Exception($"'{constant.Name}' already exists");
        }

        placeToAdd[constant.Name] = constant;

        foreach (var item in Children)
            item.AddUserConstant(constant);
    }

    public bool TryGetUserConstant(string name, out UserDefinedConstant constant)
    {
        if (GlobalUserDefinedConstants.TryGetValue(name, out constant!))
            return true;

        if (UserDefinedConstants.TryGetValue(name, out constant!))
            return true;

        return false;
    }
}
