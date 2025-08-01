namespace ic11.Emulator.Operations;

public class Yield: Operation
{
    public static string[] Opcodes = ["yield"];
    public static int ArgumentCount = 0;
    
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        emulator.Yielding = true;
        emulator.ProgramCounter++;
    }
}
