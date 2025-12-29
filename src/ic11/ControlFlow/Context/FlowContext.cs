using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class FlowContext
{
    public readonly Root Root;
    public Node CurrentNode;
    public Dictionary<string, MethodDeclaration> DeclaredMethods = new();
    public List<IStatement>? CurrentStatementList;
    public List<UserDefinedVariable> AllUserDefinedVariables = new();
    public List<UserDefinedConstant> AllUserDefinedConstants = new();

    public FlowContext()
    {
        var root = new Root();
        CurrentNode = root;
        Root = root;
        CurrentStatementList = root.Statements;
    }
}
