using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BinaryOperation : Node, IBinaryOperation, IExpression, IExpressionContainer
{
    public IExpression Left;
    public IExpression Right;
    public BinaryOperationType Type;
    public Variable? Variable { get; set; }

    public BinaryOperation(IExpression left, IExpression right, BinaryOperationType type)
    {
        Left = left;
        Right = right;
        Type = type;
        ((Node)left).Parent = this;
        ((Node)right).Parent = this;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Left;
            yield return Right;
        }
    }
}
