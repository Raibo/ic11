namespace ic11.Emulator.Operations;

public class Noop: Operation
{
    public static string[] Opcodes = ["noop"];
    public static int ArgumentCount = 0;
    
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        emulator.ProgramCounter++;
    }
}
