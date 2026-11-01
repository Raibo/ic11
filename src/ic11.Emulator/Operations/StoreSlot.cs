namespace ic11.Emulator.Operations;

public sealed class StoreSlot : Operation
{
    public static string[] Opcodes = ["ss", "sbs"];
    public static int ArgumentCount = 4;

    public override void Execute(Emulator emulator)
    {
        int slot = (int)emulator.InferValue(Args[1]);
        double value = emulator.InferValue(Args[3]);
        IEnumerable<Device> devices = Opcode == "ss"
            ? [emulator.RequireDevice(Args[0])]
            : emulator.SelectDevices((int)emulator.InferValue(Args[0]));
        foreach (var device in devices)
            emulator.WriteSlot(device, slot, Args[2], value);
        emulator.ProgramCounter++;
    }
}
