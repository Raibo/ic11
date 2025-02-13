using ic11.ControlFlow.Nodes;
using System.Collections.Generic;

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
        toNodeIndex = Math.Max(fromNodeIndex, toNodeIndex);

        var usedRegisters = GetUsedRegisters(fromNodeIndex, toNodeIndex);

        var availableRegisters = Enumerable.Range(0, 15)
            .OrderBy(r => r)
            .Select(r => $"r{r}")
            .Except(usedRegisters);

        var register = availableRegisters.First();
        return register;
    }

    public List<string> GetUsedRegisters(int fromNodeIndex, int toNodeIndex) =>
        GetVariablesForWholeMethodAndChildren()
            .Where(v => v.DeclareIndex <= toNodeIndex)
            .Where(v => fromNodeIndex < v.LastReferencedIndex)
            .Select(v => v.Register)
            .ToList();

    private HashSet<Variable> GetVariablesForWholeMethodAndChildren()
    {
        var variables = new HashSet<Variable>();
        TraverseScope(this.Method!.InnerScope!);

        return variables;

        void TraverseScope(Scope sc)
        {
            foreach (var item in sc.Variables)
                variables.Add(item);

            foreach (var child in sc.Children)
                TraverseScope(child);
        }
    }

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

    public bool IsNameAlreadyTaken(string name) =>
        UserDefinedConstants.ContainsKey(name)
        || UserDefinedVariables.ContainsKey(name);
}
