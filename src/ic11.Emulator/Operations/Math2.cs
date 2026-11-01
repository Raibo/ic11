namespace ic11.Emulator.Operations;

public class Math2 : Operation
{
    public static string[] Opcodes =
    [
        "abs", "sqrt", "exp", "log", "sin", "cos", "tan", "asin", "acos", "atan",
        "round", "ceil", "floor", "trunc", "not", "seqz", "snez", "sgez", "sgtz",
        "slez", "sltz", "snan", "snanz",
    ];
    public static int ArgumentCount = 2;

    // seqz r0 r2
    
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var register = Args[0];
        var value = emulator.InferValue(Args[1]);
        
        double result = Opcode switch
        {
            "abs" => Math.Abs(value),
            "sqrt" => Math.Sqrt(value),
            "exp" => Math.Exp(value),
            "log" => Math.Log(value),
            "sin" => Math.Sin(value),
            "cos" => Math.Cos(value),
            "tan" => Math.Tan(value),
            "asin" => Math.Asin(value),
            "acos" => Math.Acos(value),
            "atan" => Math.Atan(value),
            "round" => Math.Round(value, MidpointRounding.ToEven),
            "ceil" => Math.Ceiling(value),
            "floor" => Math.Floor(value),
            "trunc" => Math.Truncate(value),
            "not" => ~(long)Math.Truncate(value),
            "seqz" => value == 0 ? 1 : 0,
            "snez" => value != 0 ? 1 : 0,
            "sgez" => value >= 0 ? 1 : 0,
            "sgtz" => value > 0 ? 1 : 0,
            "slez" => value <= 0 ? 1 : 0,
            "sltz" => value < 0 ? 1 : 0,
            "snan" => double.IsNaN(value) ? 1 : 0,
            "snanz" => double.IsNaN(value) ? 0 : 1,
            _ => throw emulator.GenerateEmulatorException($"Unknown unary instruction: {Opcode}"),
        };
        emulator.SetRegister(register, result);
        
        emulator.ProgramCounter++;
    }
}
