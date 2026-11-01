namespace ic11.Emulator.Operations;

public sealed class Sleep : Operation
{
    public static string[] Opcodes = ["sleep"];
    public static int ArgumentCount = 1;

    public override void Execute(Emulator emulator)
    {
        double seconds = emulator.InferValue(Args[0]);
        if (!double.IsFinite(seconds) || seconds < 0)
            throw emulator.GenerateEmulatorException("Sleep requires finite, non-negative seconds");
        emulator.SleepingUntil = emulator.TimeSeconds + seconds;
        emulator.Yielding = true;
        emulator.ProgramCounter++;
    }
}

public sealed class Halt : Operation
{
    public static string[] Opcodes = ["hcf"];
    public static int ArgumentCount = 0;

    public override void Execute(Emulator emulator)
    {
        emulator.Stopped = true;
        emulator.ProgramCounter++;
    }
}
