using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceRead : IInstruction
{
    public Variable Destination;
    public string Device;
    public string DeviceProperty;

    private const string PinSetProperty = "IsSet";

    public DeviceRead(Variable destination, string device, string deviceProperty)
    {
        Destination = destination;
        Device = device;
        DeviceProperty = deviceProperty;
    }

    public InstructionType Type => InstructionType.DeviceRead;

    public string Render()
    {
        if (DeviceProperty == PinSetProperty)
            return $"sdse {Destination.Render()} {Device}";

        return $"l {Destination.Render()} {Device} {DeviceProperty}";
    }
}
