using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class Root : Node, IStatementsContainer
{
    public List<IStatement> Statements { get; init; } = new();
}
