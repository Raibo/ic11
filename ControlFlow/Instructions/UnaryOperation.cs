using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class UnaryOperation : Instruction
{
    public Variable Destination;
    public IExpression Operand;
    public UnaryOperationType Type;

    public UnaryOperation(Variable destination, IExpression operand, UnaryOperationType type)
    {
        Destination = destination;
        Operand = operand;
        Type = type;
    }

    public override string Render() =>
        Type switch
            {
                UnaryOperationType.Not => $"seqz {Destination.Register} {Operand.Render()}",
                UnaryOperationType.Minus => $"mul {Destination.Register} {Operand.Render()} -1",
                UnaryOperationType.Abs => $"abs {Destination.Register} {Operand.Render()}",
                _ => throw new Exception("Unexpected binary operation type"),
            };
}
