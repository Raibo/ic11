using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class ArrayDeclaration : Node, IStatement, IExpressionContainer
{
    public string Name;
    public IExpression SizeExpression;
    public Variable? AddressVariable;
    public ArrayDeclarationType DeclarationType;

    public List<IExpression>? InitialElementExpressions;

    public ArrayDeclaration(string name, IExpression sizeExpression)
    {
        Name = name;
        SizeExpression = sizeExpression;
        ((Node)sizeExpression).Parent = this;

        DeclarationType = ArrayDeclarationType.Size;
    }

    public ArrayDeclaration(string name, List<IExpression> initialElementExpressions)
    {
        Name = name;
        SizeExpression = new Literal(initialElementExpressions.Count);
        InitialElementExpressions = initialElementExpressions;

        foreach (var item in initialElementExpressions)
            ((Node)item).Parent = this;

        DeclarationType = ArrayDeclarationType.List;
    }

    public IEnumerable<IExpression> Expressions => DeclarationType switch
    {
        ArrayDeclarationType.Size => Enumerable.Repeat(SizeExpression, 1),
        ArrayDeclarationType.List => InitialElementExpressions!,
        _ => throw new Exception($"Unexpected array declaration type {DeclarationType}"),
    };
}
