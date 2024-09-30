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

    public bool NotAllPathsReturnValue = false;
    public Scope? InnerScope;

    public MethodDeclaration(string name, MethodReturnType returnType, List<string> parameters)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
    }
}
