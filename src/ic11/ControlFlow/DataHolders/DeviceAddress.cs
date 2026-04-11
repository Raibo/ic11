using ic11.ControlFlow.NodeInterfaces;

namespace ic11.ControlFlow.DataHolders;

public readonly struct DeviceAddress
{
    private DeviceAddress(string? name, IExpression? deviceId, IExpression? pinsIndex)
    {
        Name = name;
        DeviceId = deviceId;
        PinsIndex = pinsIndex;
    }

    public static DeviceAddress FromName(string name) => new(name, null, null);
    public static DeviceAddress FromDeviceId(IExpression expression) => new(null, expression, null);
    public static DeviceAddress FromPinsIndex(IExpression expression) => new(null, null, expression);

    public string? Name { get; }
    public IExpression? DeviceId { get; }
    public IExpression? PinsIndex { get; }

    public string Render()
    {
        if (Name is not null)
            return Name;
        if (DeviceId is not null)
            return DeviceId.Render();
        if (PinsIndex is not null)
            return $"d{PinsIndex.Render()}";
        
        throw new Exception("No device address is set");
    }

    public IExpression? Expression => DeviceId ?? PinsIndex;

    public void Validate()
    {
        int setValues = (Name is not null ? 1 : 0) + (DeviceId is not null ? 1 : 0) + (PinsIndex is not null ? 1 : 0);
        if (setValues != 1)
            throw new Exception("Not exactly one entry is set");
    }
}