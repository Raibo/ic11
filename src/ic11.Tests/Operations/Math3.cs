namespace ic11.Tests.Operations;

public class Math3 : Operation
{
    public static string[] Opcodes =
    [
        "add",
        "sub",
        "mul",
        "div",
        "div",
        "mod",
        "slt",
        "sgt",
        "sle",
        "sge",
        "seq",
        "sne",
        "and",
        "xor",
        "or",
        "nor",
        "max",
        "min",
        "sll",
        "srl",
        "sla",
        "sra",
        "atan2",
        "sapz",
        "snaz"
    ];

    public static int ArgumentCount = 3;

    // seq r3 r1 4
    // sgt r2 r2 0
    // or r2 r2 r3
    // sub r2 r0 1
    // slt r2 r0 1
    // sgt r2 r0 1

    public override void Execute(Emulator emulator)
    {
        var register = Args[0];
        var left = emulator.InferValue(Args[1]);
        var right = emulator.InferValue(Args[2]);

        var result = Opcode switch
        {
            "add" => left + right,
            "sub" => left - right,
            "mul" => left * right,
            "div" when right != 0 => left / right,
            "div" when right == 0 => throw new DivideByZeroException(),
            "mod" => Modulo(left, right),
            "slt" => left < right ? 1 : 0,
            "sgt" => left > right ? 1 : 0,
            "sle" => left <= right ? 1 : 0,
            "sge" => left >= right ? 1 : 0,
            "seq" => left == right ? 1 : 0,
            "sne" => left != right ? 1 : 0,
            "and" => (long)double.Floor(left) & (long)double.Floor(right),
            "xor" => (long)double.Floor(left) ^ (long)double.Floor(right),
            "or" => (long)double.Floor(left) | (long)double.Floor(right),
            "nor" => ~((long)double.Floor(left) | (long)double.Floor(right)),
            "max" => Math.Max(left, right),
            "min" => Math.Min(left, right),
            "sll" => (ulong)double.Floor(left) <<
                     (int)double.Floor(right), // C# makes logic on unsigned and arithmetic on signed
            "srl" => (ulong)double.Floor(left) >> (int)double.Floor(right),
            "sla" => (long)double.Floor(left) << (int)double.Floor(right),
            "sra" => (long)double.Floor(left) >> (int)double.Floor(right),
            "atan2" => Math.Atan2(left, right),
            "sapz" => Math.Abs(left) <= Math.Max(right * Math.Abs(left), float.Epsilon * 8d) ? 1 : 0,
            "snaz" => Math.Abs(left) > Math.Max(right * Math.Abs(left), float.Epsilon * 8d) ? 1 : 0,
            _ => throw new Exception("Unexpected binary operation type"),
        };

        emulator.SetRegister(register, result);
        
        emulator.ProgramCounter++;
    }

    private double Modulo(double x, double m)
    {
        // IC10 returns 0 for mod, e.g. "mod r0 12345 0" will put 0 into r0
        if (m == 0)
            return 0;

        var r = x % m;
        return r < 0 ? r + m : r;
    }
}