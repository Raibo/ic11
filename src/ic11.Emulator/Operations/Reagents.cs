namespace ic11.Emulator.Operations;

public sealed class LoadReagent : Operation
{
    public static string[] Opcodes = ["lr"];
    public static int ArgumentCount = 4;

    public override void Execute(Emulator emulator)
    {
        var device = emulator.RequireDevice(Args[1]);
        int reagent = (int)emulator.InferValue(Args[3]);
        var value = device.Reagents.GetValueOrDefault((Args[2], reagent));
        emulator.RecordAccess(device, DeviceAccessKind.ReadReagent, value, Args[2], reagent);
        emulator.SetRegister(Args[0], value);
        emulator.ProgramCounter++;
    }
}

public sealed class MapReagent : Operation
{
    public static string[] Opcodes = ["rmap"];
    public static int ArgumentCount = 3;

    public override void Execute(Emulator emulator)
    {
        var device = emulator.RequireDevice(Args[1]);
        int reagent = (int)emulator.InferValue(Args[2]);
        var value = device.ReagentSources.GetValueOrDefault(reagent);
        emulator.RecordAccess(device, DeviceAccessKind.MapReagent, value, index: reagent);
        emulator.SetRegister(Args[0], value);
        emulator.ProgramCounter++;
    }
}
