using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceWrite : IInstruction
{
    public string Device;
    public string DeviceProperty;
    public IValue Param;

    public DeviceWrite(string device, string deviceProperty, IValue param)
    {
        Device = device;
        DeviceProperty = deviceProperty;
        Param = param;
    }

    public InstructionType Type => InstructionType.DeviceWrite;
    public string Render() => $"s {Device} {DeviceProperty} {Param.Render()}";
}
