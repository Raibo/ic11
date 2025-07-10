namespace ic11.Tests.Operations;

public class Move : Operation
{
    public static string[] Opcodes = ["move"];
    public static int ArgumentCount = 2;
    
    // move r0 30
    public override void Execute(Emulator emulator)
    {
        var register = Args[0]; // e.g. "r0"
        var value = Args[1];

        emulator.SetRegister(register, emulator.InferValue(value));
        
        emulator.ProgramCounter++;
    }
}