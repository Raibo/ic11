using ic11.ControlFlow.Nodes;

namespace ic11.ControlFlow.NodeInterfaces;
public interface IStatementsContainer
{
    public List<IStatement> Statements { get; }
    public void AddToStatements(IStatement statement)
    {
        Statements.Add(statement);
        ((Node)statement).Parent = (Node)this;
    }
}
