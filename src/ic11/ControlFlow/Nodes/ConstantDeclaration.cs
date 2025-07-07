using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ConstantDeclaration : Node, IStatement, IExpressionContainer
{
    public string Name;
    public IExpression Expression;

    public ConstantDeclaration(string name, IExpression expression)
    {
        Name = name;
        Expression = expression;
        ((Node)expression).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Expression;
        }
    }
}
