using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class BinaryOperation : Node, IBinaryOperation, IExpression
{
    public IExpression Left;
    public IExpression Right;
    public BinaryOperationType Type;

    public BinaryOperation(IExpression left, IExpression right, BinaryOperationType type)
    {
        Left = left;
        Right = right;
        Type = type;
        ((Node)left).Parent = this;
        ((Node)right).Parent = this;
    }
}
