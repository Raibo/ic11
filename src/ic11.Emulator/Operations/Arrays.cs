namespace ic11.Emulator.Operations;

public class Arrays : Operation
{
    public static string[] Opcodes = ["get", "put", "getd", "putd"];

    public static int ArgumentCount = 3;

    // get r? d? address(r?|num)
    // put d? address(r?|num) value(r?|num)

    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        switch (Opcode)
        {  
            case "get":
            case "getd":
                var targetRegister = Args[0];
                var device = Args[1];
                var address = Args[2];
        
                var d = emulator.RequireDevice(device);
                var addressValue = emulator.InferValue(address);
                var value = d.Stack[(int)addressValue];
                emulator.RecordAccess(d, DeviceAccessKind.ReadStack, value, index: (int)addressValue);
        
                emulator.SetRegister(targetRegister, value);
        
                emulator.ProgramCounter++;
                
                break;
            
            case "put":
            case "putd":
                var deviceToWrite = Args[0];
                var addressToWrite = Args[1];
                var valueToWrite = Args[2];
                
                var targetDevice = emulator.RequireDevice(deviceToWrite);
                var addressValueToWrite = emulator.InferValue(addressToWrite);
                var valueToWriteValue = emulator.InferValue(valueToWrite);
                
                targetDevice.Stack[(int)addressValueToWrite] = valueToWriteValue;
                emulator.RecordAccess(targetDevice, DeviceAccessKind.WriteStack, valueToWriteValue, index: (int)addressValueToWrite);
                
                emulator.ProgramCounter++;
                
                break;
            default:
                throw emulator.GenerateEmulatorException("Unexpected operation type: " + Opcode);
        }
        
    }

   
}
