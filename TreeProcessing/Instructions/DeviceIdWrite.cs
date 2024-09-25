using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;

namespace ic11.TreeProcessing.Instructions;
public class DeviceIdWrite : InstructionBase
{
    public IValue IdValue;
    public IValue Value;
    public string DeviceProperty;

    public DeviceIdWrite(Scope scope, IValue idValue, IValue value, string deviceProperty) : base(scope)
    {
        IdValue = idValue;
        Value = value;
        DeviceProperty = deviceProperty;
    }

    public override InstructionType Type => InstructionType.DeviceRead;

    public override string Render()
    {
        return $"sd {IdValue.Render()} {DeviceProperty} {Value.Render()}";
    }
}
