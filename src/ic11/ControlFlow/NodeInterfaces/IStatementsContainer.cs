using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.NodeInterfaces;
public interface IStatementsContainer: INode
{
    public List<IStatement> Statements { get; }
    public void AddToStatements(IStatement statement)
    {
        Statements.Add(statement);
        statement.Parent = this;
    }
}
