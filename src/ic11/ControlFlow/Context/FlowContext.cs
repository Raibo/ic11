using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class FlowContext
{
    public readonly Root Root;
    public Node CurrentNode;
    public readonly Dictionary<string, MethodDeclaration> DeclaredMethods = new();
    public List<IStatement> CurrentStatementList => Root.Statements;
    public readonly List<UserDefinedVariable> AllUserDefinedVariables = new();
    public readonly List<UserDefinedConstant> AllUserDefinedConstants = new();

    public FlowContext()
    {
        var root = new Root();
        CurrentNode = root;
        Root = root;
    }
}
