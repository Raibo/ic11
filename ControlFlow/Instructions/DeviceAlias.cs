namespace ic11.ControlFlow.Instructions;
public class DeviceAlias : Instruction
{
    public string Alias;
    public string Device;

    public DeviceAlias(string alias, string device)
    {
        Alias = alias;
        Device = device;
    }

    public override string Render() => $"alias {Alias} {Device}";
}
