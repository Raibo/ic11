using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceWrite : InstructionBase
{
    public string Device;
    public string DeviceProperty;
    public IValue Param;

    public DeviceWrite(Scope scope, string device, string deviceProperty, IValue param) : base(scope)
    {
        Device = device;
        DeviceProperty = deviceProperty;
        Param = param;
    }

    public override InstructionType Type => InstructionType.DeviceWrite;
    public override string Render() => $"s {Device} {DeviceProperty} {Param.Render()}";
}
