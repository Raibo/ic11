using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class MethodDeclaration : Node, IStatement, IStatementsContainer
{
    public string Name;
    public MethodReturnType ReturnType;
    public List<string> Parameters;
    public List<IStatement> Statements { get; init; } = new();
    public List<Variable> ParameterVariables = new();

    public bool NotAllPathsReturnValue = false;
    public bool ContainsArrays = false;
    public Scope? InnerScope;

    public List<MethodDeclaration> InvokedMethods = [];
    public HashSet<MethodDeclaration> InvokedFrom = [];
    public List<MethodCall> MethodCalls = [];
    public int UsedRegistersCount;
    public HashSet<string> AssignedRegisters = [];
    public List<Variable> AllVariables = [];


    public MethodDeclaration(string name, MethodReturnType returnType, List<string> parameters)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
    }

    public override string ToString() => $"MethodDeclaration[{Name}]";

    public override int GetHashCode() => Name.GetHashCode();
    public override bool Equals(object? obj) => obj is MethodDeclaration md && md.Name == Name;
}
