namespace ic11.Emulator.Operations;

public class Alias : Operation
{
    public static string[] Opcodes = ["alias"];
    public static int ArgumentCount = 2;
    
    // alias BypassDoor d2
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var aliasName = Args[0];
        var device = Args[1];
        
        emulator.DeviceAliases[aliasName] = emulator.DeviceAliases[device];
        emulator.ProgramCounter++;
    }
}