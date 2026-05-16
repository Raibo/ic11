using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ConstantDeclaration : Node, IStatement, IExpressionContainer
{
    public string Name;
    public INodeExpression Expression;

    public ConstantDeclaration(string name, INodeExpression expression)
    {
        Name = name;
        Expression = expression;
        expression.Parent = this;
    }

    public IEnumerable<INodeExpression> Expressions
    {
        get
        {
            yield return Expression;
        }
    }
}
