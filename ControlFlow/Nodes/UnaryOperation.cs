using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Nodes;
public class UnaryOperation : Node, IBinaryOperation, IExpression, IExpressionContainer
{
    public IExpression Operand;
    public UnaryOperationType Type;
    public Variable? Variable { get; set; }

    public UnaryOperation(IExpression operand, UnaryOperationType type)
    {
        Operand = operand;
        ((Node)operand).Parent = this;
        Type = type;
    }

    public IEnumerable<IExpression> Expressions
    {
        get
        {
            yield return Operand;
        }
    }
}
