namespace ic11.Tests.Operations;

public class Yield: Operation
{
    public static string[] Opcodes = ["yield"];
    public static int ArgumentCount = 0;
    
    public override void Execute(Emulator emulator)
    {
        emulator.Yielding = true;
        emulator.ProgramCounter++;
    }
}
