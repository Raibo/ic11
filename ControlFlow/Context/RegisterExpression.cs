using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Context;
public class DirectExpression : IExpression
{
    public Variable? Variable { get; set; }

    public decimal? CtKnownValue => null;

    public DirectExpression(string register)
    {
        Variable = new Variable { Register = register };
    }

    public DirectExpression(Variable variable)
    {
        Variable = variable;
    }
}
