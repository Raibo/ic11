namespace ic11.Emulator.Operations;

public class Arrays : Operation
{
    public static string[] Opcodes = ["get", "put"];

    public static int ArgumentCount = 3;

    // get r? d? address(r?|num)
    // put d? address(r?|num) value(r?|num)

    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        switch (Opcode)
        {  
            case "get":
                var targetRegister = Args[0];
                var device = Args[1];
                var address = Args[2];
        
                var d = emulator.InferDevice(device)!;
                var addressValue = emulator.InferValue(address);
                var value = d.Stack[(int)addressValue];
        
                emulator.SetRegister(targetRegister, value);
        
                emulator.ProgramCounter++;
                
                break;
            
            case "put":
                var deviceToWrite = Args[0];
                var addressToWrite = Args[1];
                var valueToWrite = Args[2];
                
                var targetDevice = emulator.InferDevice(deviceToWrite)!;
                var addressValueToWrite = emulator.InferValue(addressToWrite);
                var valueToWriteValue = emulator.InferValue(valueToWrite);
                
                targetDevice.Stack[(int)addressValueToWrite] = valueToWriteValue;
                
                emulator.ProgramCounter++;
                
                break;
            default:
                throw emulator.GenerateEmulatorException("Unexpected operation type: " + Opcode);
        }
        
    }

   
}