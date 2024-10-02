using ic11.ControlFlow.Context;
using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.Instructions;
public class StackGet : Instruction
{
    string Device;
    public Variable Destination;
    public IExpression AddressExpression;

    public StackGet(string device, Variable destination, IExpression addressExpression)
    {
        Device = device;
        Destination = destination;
        AddressExpression = addressExpression;
    }

    public override string Render() => $"get {Destination.Register} {Device} {AddressExpression.Render()}";
}
