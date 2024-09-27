using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class VariableDeclaration : Node, IStatement
{
    public string Name;
    public IExpression Expression;

    public VariableDeclaration(string name, IExpression expression)
    {
        Name = name;
        Expression = expression;
        ((Node)expression).Parent = this;
    }
}
