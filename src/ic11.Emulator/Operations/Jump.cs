namespace ic11.Emulator.Operations;

public class Jump: Operation
{
    public static string[] Opcodes = ["j", "jal"];
    public static int ArgumentCount = 1;
    
    // j 34
    // jal 34
    
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var address = (int)emulator.InferValue(Args[0]);

        switch (Opcode)
        {
            case "j":
            {
                emulator.ProgramCounter = address;
                return;
            }
            case "jal":
            {
                emulator.SetRegister("ra", emulator.ProgramCounter + 1);
                emulator.ProgramCounter = address;
                return;
            }
            default:
                throw new NotImplementedException();
        }
    }
}
