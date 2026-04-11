using ic11.ControlFlow.DataHolders;

namespace ic11.ControlFlow.Instructions;

public class DeviceStackClear : Instruction
{
    public readonly DeviceAddress Device;

    public DeviceStackClear(DeviceAddress device)
    {
        Device = device;
    }

    public override string Render()
    {
        if (Device.DeviceId is not null)
            return $"clrd {Device.DeviceId.Render()}";
        return $"clr {Device.Render()}";
    }
}