namespace ic11.Tests.Operations;

public class Stack: Operation
{
    public static string[] Opcodes = ["push", "pop"];
    public static int ArgumentCount = 1;
    
    // pop r2
    public override void Execute(Emulator emulator)
    {
        switch (Opcode)
        {
            case "push":
                var value = emulator.InferValue(Args[0]);
                emulator.Stack[(int)emulator.Registers[emulator.StackPointerRegister]] = value;
                emulator.Registers[emulator.StackPointerRegister] += 1; 
                break;
                
            case "pop":
                emulator.Registers[emulator.StackPointerRegister] -= 1; 
                var value2 = emulator.Stack[(int)emulator.Registers[emulator.StackPointerRegister]];
                emulator.SetRegister(Args[0], value2);
                
                break;
                
            default:
                throw new InvalidOperationException($"Unknown opcode: {Opcode}");
        }  
        
        emulator.ProgramCounter++;
    }
}
