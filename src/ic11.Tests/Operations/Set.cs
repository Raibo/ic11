namespace ic11.Tests.Operations;

public class Set : Operation
{
    public static string[] Opcodes = ["s"];
    public static int ArgumentCount = 3;

    // s BypassDoor Lock 1
    public override void Execute(Emulator emulator)
    {
        var device = Args[0];
        var propertyName = Args[1];
        var value = Args[2];
        
        // var d = emulator.DeviceAliases[device];
        var d = emulator.InferDevice(device);
        d.SetProperty(propertyName, emulator.InferValue(value));
        
        emulator.ProgramCounter++;
    }
}