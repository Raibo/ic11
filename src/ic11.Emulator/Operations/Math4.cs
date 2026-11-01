namespace ic11.Emulator.Operations;

public sealed class Math4 : Operation
{
    public static string[] Opcodes = ["select", "sap", "sna"];
    public static int ArgumentCount = 4;

    public override void Execute(Emulator emulator)
    {
        double a = emulator.InferValue(Args[1]);
        double b = emulator.InferValue(Args[2]);
        double c = emulator.InferValue(Args[3]);
        double result = Opcode switch
        {
            "select" => a != 0 ? b : c,
            "sap" => Math.Abs(a - b) <= Math.Max(c * Math.Max(Math.Abs(a), Math.Abs(b)), float.Epsilon * 8d) ? 1 : 0,
            "sna" => Math.Abs(a - b) > Math.Max(c * Math.Max(Math.Abs(a), Math.Abs(b)), float.Epsilon * 8d) ? 1 : 0,
            _ => throw emulator.GenerateEmulatorException($"Unknown ternary instruction: {Opcode}"),
        };
        emulator.SetRegister(Args[0], result);
        emulator.ProgramCounter++;
    }
}
