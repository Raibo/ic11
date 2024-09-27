namespace ic11.ControlFlow.Nodes;
public class VariableAccess : Node, IExpression
{
    public string Name;

    public VariableAccess(string name)
    {
        Name = name;
    }
}
