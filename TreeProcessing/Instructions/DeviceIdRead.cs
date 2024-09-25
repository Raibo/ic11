using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceIdRead : InstructionBase
{
    public Variable Destination;
    public IValue IdValue;
    public string DeviceProperty;

    private const string PinSetProperty = "IsSet";

    public DeviceIdRead(Scope scope, Variable destination, IValue idValue, string deviceProperty) : base(scope)
    {
        Destination = destination;
        IdValue = idValue;
        DeviceProperty = deviceProperty;
    }

    public override InstructionType Type => InstructionType.DeviceRead;

    public override string Render()
    {
        if (DeviceProperty == PinSetProperty)
            throw new Exception("Impossible to check that a device ID exists");

        return $"ld {Destination.Render()} {IdValue.Render()} {DeviceProperty}";
    }
}
