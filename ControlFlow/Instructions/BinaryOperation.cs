using ic11.ControlFlow.Context;
using ic11.ControlFlow.DataHolders;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class BinaryOperation : Instruction
{
    public Variable Destination;
    public IExpression Left;
    public IExpression Right;
    public BinaryOperationType Type;

    public BinaryOperation(Variable destination, IExpression left, IExpression right, BinaryOperationType type)
    {
        Destination = destination;
        Left = left;
        Right = right;
        Type = type;
    }

    private string GetCommand() =>
        Type switch
            {
                BinaryOperationType.Add => "add",
                BinaryOperationType.Sub => "sub",
                BinaryOperationType.Mul => "mul",
                BinaryOperationType.Div => "div",
                BinaryOperationType.Mod => "mod",
                BinaryOperationType.Lt => "slt",
                BinaryOperationType.GT => "sgt",
                BinaryOperationType.Le => "sle",
                BinaryOperationType.Ge => "sge",
                BinaryOperationType.Eq => "seq",
                BinaryOperationType.Ne => "sne",
                BinaryOperationType.And => "and",
                BinaryOperationType.Or => "or",
                _ => throw new Exception("Unexpected binary operation type"),
            };


    public override string Render() => $"{GetCommand()} {Destination.Register} {Left.Render()} {Right.Render()}";
}
