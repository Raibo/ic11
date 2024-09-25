using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceRead : InstructionBase
{
    public Variable Destination;
    public string Device;
    public string DeviceProperty;

    private const string PinSetProperty = "IsSet";

    public DeviceRead(Scope scope, Variable destination, string device, string deviceProperty) : base(scope)
    {
        Destination = destination;
        Device = device;
        DeviceProperty = deviceProperty;
    }

    public override InstructionType Type => InstructionType.DeviceRead;

    public override string Render()
    {
        if (DeviceProperty == PinSetProperty)
            return $"sdse {Destination.Render()} {Device}";

        return $"l {Destination.Render()} {Device} {DeviceProperty}";
    }
}
