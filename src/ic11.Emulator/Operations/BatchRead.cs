namespace ic11.Emulator.Operations;

public abstract class BatchRead : Operation
{
    protected void ExecuteBatch(Emulator emulator, bool named, bool slot)
    {
        int next = 1;
        int prefab = (int)emulator.InferValue(Args[next++]);
        int? name = named ? (int)emulator.InferValue(Args[next++]) : null;
        int index = slot ? (int)emulator.InferValue(Args[next++]) : 0;
        string property = Args[next++];
        string mode = Args[next];
        var values = emulator.SelectDevices(prefab, name)
            .Select(device => slot ? emulator.ReadSlot(device, index, property) : emulator.ReadProperty(device, property))
            .ToArray();
        double result = mode switch
        {
            "Average" or "0" => values.Length == 0 ? double.NaN : values.Average(),
            "Sum" or "1" => values.Sum(),
            "Minimum" or "2" => values.Length == 0 ? double.PositiveInfinity : values.Min(),
            "Maximum" or "3" => values.Length == 0 ? double.NegativeInfinity : values.Max(),
            _ => throw emulator.GenerateEmulatorException($"Unsupported batch mode: {mode}"),
        };
        emulator.SetRegister(Args[0], result);
        emulator.ProgramCounter++;
    }
}

public sealed class BatchLoad : BatchRead
{
    public static string[] Opcodes = ["lb"];
    public static int ArgumentCount = 4;
    public override void Execute(Emulator emulator) => ExecuteBatch(emulator, named: false, slot: false);
}

public sealed class BatchLoadNamed : BatchRead
{
    public static string[] Opcodes = ["lbn"];
    public static int ArgumentCount = 5;
    public override void Execute(Emulator emulator) => ExecuteBatch(emulator, named: true, slot: false);
}

public sealed class BatchLoadSlot : BatchRead
{
    public static string[] Opcodes = ["lbs"];
    public static int ArgumentCount = 5;
    public override void Execute(Emulator emulator) => ExecuteBatch(emulator, named: false, slot: true);
}

public sealed class BatchLoadSlotNamed : BatchRead
{
    public static string[] Opcodes = ["lbns"];
    public static int ArgumentCount = 6;
    public override void Execute(Emulator emulator) => ExecuteBatch(emulator, named: true, slot: true);
}
