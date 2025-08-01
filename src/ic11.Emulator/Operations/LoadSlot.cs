namespace ic11.Emulator.Operations;

public class LoadSlot : Operation
{
    public static string[] Opcodes = ["ls"];
    public static int ArgumentCount = 4;

    // ls r0 CanisterSlot 0 PrefabHash
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var register = Args[0];
        var device = Args[1];
        var slot = Args[2];
        var propertyName = Args[3];
        
        var d = emulator.InferDevice(device);
        var value = d.GetSlotProperty((int)emulator.InferValue(slot), propertyName);
        
        emulator.SetRegister(register, value);
        
        emulator.ProgramCounter++;
    }
}