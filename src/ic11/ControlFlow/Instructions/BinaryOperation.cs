using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class BinaryOperation : Instruction
{
    public Variable Destination;
    public IExpression Left;
    public IExpression Right;
    string Operation;

    public BinaryOperation(Variable destination, IExpression left, IExpression right, string operation)
    {
        Destination = destination;
        Left = left;
        Right = right;
        Operation = operation;
    }

    public override string Render() => $"{Operation} {Destination.Register} {Left.Render()} {Right.Render()}";
}
