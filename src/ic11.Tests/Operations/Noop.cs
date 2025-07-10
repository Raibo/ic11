namespace ic11.Tests.Operations;

public class Noop: Operation
{
    public static string[] Opcodes = ["noop"];
    public static int ArgumentCount = 0;
    
    public override void Execute(Emulator emulator)
    {
        emulator.ProgramCounter++;
    }
}
