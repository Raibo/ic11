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
    public Dictionary<string, UserDefinedArray> UserDefinedArrays = new();

    public Variable ClaimNewVariable(int declareIndex)
    {
        var newVar = new Variable();
        newVar.DeclareScope = this;
        newVar.DeclareIndex = declareIndex;
        AddVariable(newVar);

        return newVar;
    }

    private void AddVariable(Variable variable)
    {
        Variables.Add(variable);

        foreach (var item in Children)
            item.AddVariable(variable);
    }

    public Scope CreateChildScope(MethodDeclaration? method = null)
    {
        var childScope = new Scope();
        Children.Add(childScope);
        childScope.Parent = this;

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

    public string GetAvailableRegister(int fromNodeIndex, int toNodeIndex)
    {
        var usedRegisters = GetUsedRegisters(fromNodeIndex, toNodeIndex);

        var availableRegisters = Enumerable.Range(0, 15)
            .OrderBy(r => r)
            .Select(r => $"r{r}")
            .Except(usedRegisters);

        var register = availableRegisters.First();
        return register;
    }

    public List<string> GetUsedRegisters(int fromNodeIndex, int toNodeIndex) =>
        Variables
            .Where(v => v.DeclareIndex <= toNodeIndex)
            .Where(v => v.LastReferencedIndex >= fromNodeIndex)
            .Select(v => v.Register)
            .ToList();

    public void AddUserVariable(UserDefinedVariable variable)
    {
        if (IsNameAlreadyTaken(variable.Name))
            throw new Exception($"'{variable.Name}' already exists");

        UserDefinedVariables[variable.Name] = variable;

        foreach (var item in Children)
            item.AddUserVariable(variable);
    }

    public bool TryGetUserVariable(string name, out UserDefinedVariable variable) =>
        UserDefinedVariables.TryGetValue(name, out variable!);

    public void AddUserConstant(UserDefinedConstant constant)
    {
        if (IsNameAlreadyTaken(constant.Name))
            throw new Exception($"'{constant.Name}' already exists");

        UserDefinedConstants[constant.Name] = constant;

        foreach (var item in Children)
            item.AddUserConstant(constant);
    }

    public bool TryGetUserConstant(string name, out UserDefinedConstant constant) =>
        UserDefinedConstants.TryGetValue(name, out constant!);

    public void AddUserArray(UserDefinedArray array)
    {
        if (IsNameAlreadyTaken(array.Name))
            throw new Exception($"'{array.Name}' already exists");

        UserDefinedArrays[array.Name] = array;

        foreach (var item in Children)
            item.AddUserArray(array);
    }

    public bool TryGetUserArray(string name, out UserDefinedArray array) =>
        UserDefinedArrays.TryGetValue(name, out array!);

    public bool IsNameAlreadyTaken(string name) =>
        UserDefinedConstants.ContainsKey(name)
        || UserDefinedVariables.ContainsKey(name)
        || UserDefinedArrays.ContainsKey(name);
}
