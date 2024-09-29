using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.Context;
public class FlowContext
{
    public readonly Node Root;
    public Node CurrentNode;
    public List<IStatement>? CurrentStatementList;
    public List<UserDefinedVariable> AllUserDefinedVariables = new();

    public FlowContext()
    {
        var root = new Root();
        CurrentNode = root;
        Root = root;
        CurrentStatementList = root.Statements;
    }
}
