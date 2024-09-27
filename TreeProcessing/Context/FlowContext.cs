using ic11.ControlFlow.NodeInterfaces;
using ic11.ControlFlow.Nodes;

namespace ic11.TreeProcessing.Context;
public class FlowContext
{
    public FlowContext()
    {
        var root = new Root();
        CurrentNode = root;
        Root = root;
        CurrentStatementList = root.Statements;
    }

    public readonly Node Root;
    public Node CurrentNode;
    public List<IStatement>? CurrentStatementList;
}
