using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class TernaryOperation : Instruction
{
    public Variable Destination;
    public IExpression OperandA;
    public IExpression OperandB;
    public IExpression OperandC;
    string Operation;

    public TernaryOperation(Variable destination, IExpression operandA, IExpression operandB, IExpression operandC, string operation)
    {
        Destination = destination;
        OperandA = operandA;
        OperandB = operandB;
        OperandC = operandC;
        Operation = operation;
    }

    public override string Render() => $"{Operation} {Destination.Register} {OperandA.Render()} {OperandB.Render()} {OperandC.Render()}";
}
