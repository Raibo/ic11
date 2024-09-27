using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class UnaryOperation : Node, IBinaryOperation, IExpression
{
    public IExpression Operand;
    public UnaryOperationType Type;

    public UnaryOperation(IExpression operand, UnaryOperationType type)
    {
        Operand = operand;
        ((Node)operand).Parent = this;
        Type = type;
    }
}
