using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayDeclaration : Node, IStatement, IExpressionContainer
{
    public string Name;
    public INodeExpression SizeExpression;
    public Variable? AddressVariable;
    public ArrayDeclarationType DeclarationType;

    public List<INodeExpression>? InitialElementExpressions;

    public ArrayDeclaration(string name, INodeExpression sizeExpression)
    {
        Name = name;
        SizeExpression = sizeExpression;
        sizeExpression.Parent = this;

        DeclarationType = ArrayDeclarationType.Size;
    }

    public ArrayDeclaration(string name, List<INodeExpression> initialElementExpressions)
    {
        Name = name;
        SizeExpression = new Literal(initialElementExpressions.Count);
        InitialElementExpressions = initialElementExpressions;

        foreach (var item in initialElementExpressions)
            item.Parent = this;

        DeclarationType = ArrayDeclarationType.List;
    }

    public IEnumerable<INodeExpression> Expressions => DeclarationType switch
    {
        ArrayDeclarationType.Size => Enumerable.Repeat(SizeExpression, 1),
        ArrayDeclarationType.List => InitialElementExpressions!,
        _ => throw new Exception($"Unexpected array declaration type {DeclarationType}"),
    };
}
