namespace ic11.Emulator.Operations;

public class Load : Operation
{
    public static string[] Opcodes = ["l", "ld"];
    public static int ArgumentCount = 3;

    // l r4 Sensor RatioPollutant
    // ld r2 360735 On
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var register = Args[0];
        var device = Args[1];
        var propertyName = Args[2];

        double value;
        
        switch (Opcode)
        {
            case "l":
                value = emulator.InferDevice(device).GetProperty(propertyName);
                break;
            
            case "ld":
                var d = emulator.GetOrCreateDeviceById(device);
                value = d.GetProperty(propertyName);
                break;
            
            default:
                throw emulator.GenerateEmulatorException("Unexpected operation type: " + Opcode);
        }
        

        emulator.SetRegister(register, value);
        
        emulator.ProgramCounter++;
    }
}