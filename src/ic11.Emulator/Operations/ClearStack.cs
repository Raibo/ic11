namespace ic11.Emulator.Operations;

public sealed class ClearStack : Operation
{
    public static string[] Opcodes = ["clr", "clrd"];
    public static int ArgumentCount = 1;

    public override void Execute(Emulator emulator)
    {
        var device = emulator.RequireDevice(Args[0]);
        Array.Clear(device.Stack);
        emulator.RecordAccess(device, DeviceAccessKind.ClearStack);
        emulator.ProgramCounter++;
    }
}
