using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class StackPush : Instruction
{
    public IExpression? Expression;
    public string? Register;

    public StackPush(IExpression expression)
    {
        Expression = expression;
    }

    public StackPush(string register)
    {
        Register = register;
    }

    public override string Render() => $"push {Register ?? Expression!.Render()}";
}
