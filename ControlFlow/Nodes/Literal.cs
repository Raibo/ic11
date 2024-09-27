namespace ic11.ControlFlow.Nodes;
public class Literal : Node, IExpression
{
    public double Value;

    public Literal(double value)
    {
        Value = value;
    }
}
