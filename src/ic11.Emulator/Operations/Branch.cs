namespace ic11.Emulator.Operations;

public class Branch : Operation
{
    public static string[] Opcodes = ["beqz"];
    public static int ArgumentCount = 2;

    // beqz r3 34
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var value = Args[0];
        var address = Args[1];

        switch (Opcode)
        {
            case "beqz":
            {
                var registerValue = emulator.InferValue(value);

                if (registerValue == 0)
                    emulator.ProgramCounter = (int)emulator.InferValue(address);
                else
                    emulator.ProgramCounter++;
            
                return;
            }
            default:
                throw new NotImplementedException();
        }
    }
}