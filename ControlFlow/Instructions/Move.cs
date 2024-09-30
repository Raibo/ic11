using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class Move : Instruction
{
    public Variable? Destination;
    public string? Register;
    public IExpression Expression;

    public Move(Variable destination, IExpression expression)
    {
        Destination = destination;
        Expression = expression;
    }

    public Move(string register, IExpression expression)
    {
        Register = register;
        Expression = expression;
    }

    public override string Render() => $"move {Register ?? Destination!.Register} {Expression.Render()}";
}
