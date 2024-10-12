using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class UnaryOperation : Instruction
{
    public Variable Destination;
    public IExpression Operand;
    public string Operation;

    public UnaryOperation(Variable destination, IExpression operand, string operation)
    {
        Destination = destination;
        Operand = operand;
        Operation = operation;
    }

    public override string Render() =>
        Operation switch
            {
                "_not" => $"seqz {Destination.Register} {Operand.Render()}",
                "_neg" => $"mul {Destination.Register} {Operand.Render()} -1",
                _ => $"{Operation} {Destination.Register} {Operand.Render()}",
            };
}
