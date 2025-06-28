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

    public static bool IsCallerSavedRegister(string register) =>
        register switch
        {
            "r0" => true,
            "r1" => true,
            "r2" => true,
            "r3" => true,
            "r4" => true,
            "r5" => true,
            "r6" => true,
            "r7" => true,
            _ => false
        };

    public string GetAvailableRegister(int fromNodeIndex, int toNodeIndex)
    {
        /* Optimization for fewer lines of generated code:
         *
         * If this method is the main method, it probably has a lot of variables
         * and calls other small methods. In this case, we want to avoid pushing 
         * all the variables to the stack and popping them back. So we use the
         * original mips calling convention of having some registers be caller-saved
         * and some be callee-saved. We use the caller-saved registers for the main method
         * and callee-saved for the other methods. This way, we can minimize pushing and popping
         * registers to the stack.
         *
         * We will assign registers 0 to 7 to be caller-saved and 8 to 14 to be callee-saved.
         * This is configured in the IsCallerSavedRegister method above.
         * This way, the main method can use registers 0 to 7 and the other methods can use 8 to 14.
         * The limit is just a suggestion and won't limit any method from using more registers.
         * The ratio might not be optimal and may be improved in the future.
         *
         * Idealy we would like to have a way to determone exactly what registers are changed in the
         * called method and only save those registers. But this is a good enough solution for now.
         */

        toNodeIndex = Math.Max(fromNodeIndex, toNodeIndex);

        var usedRegisters = GetUsedRegisters(fromNodeIndex, toNodeIndex);
        IEnumerable<string> availableRegisters;

        // Main method should prioritize callee-saved registers
        if (Method!.Name == "Main")
        {
            availableRegisters = Enumerable.Range(0, 14)
           .OrderBy(r => r)
           .Select(r => $"r{r}")
           .Except(usedRegisters);
            // reverse the order to prioritize caller-saved registers
            availableRegisters = availableRegisters.Reverse();
        }
        else
        {
            availableRegisters = Enumerable.Range(0, 7)
             .OrderBy(r => r)
             .Select(r => $"r{r}")
             .Except(usedRegisters);
        }

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
