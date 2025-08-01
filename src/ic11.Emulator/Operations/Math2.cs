namespace ic11.Emulator.Operations;

public class Math2 : Operation
{
    public static string[] Opcodes = ["seqz"];
    public static int ArgumentCount = 2;

    // seqz r0 r2
    
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var register = Args[0];
        var value = emulator.InferValue(Args[1]);
        
        var result = value == 0 ? 1 : 0;
        emulator.SetRegister(register, result);
        
        emulator.ProgramCounter++;
    }
}