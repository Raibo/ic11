namespace ic11.Tests.Operations;

public class Pins : Operation
{
    public static string[] Opcodes = ["sdse"];
    public static int ArgumentCount = 2;
    
    // sdse r1 dr0 -- check if device is connected 
    public override void Execute(Emulator emulator)
    {
        var targetRegister = Args[0];
        var deviceRegister = Args[1];

        var device = emulator.InferDevice(deviceRegister);
        emulator.SetRegister(targetRegister, device == null ? 0 : 1);
        
        emulator.ProgramCounter++;
    }
}