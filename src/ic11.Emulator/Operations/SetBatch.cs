namespace ic11.Emulator.Operations;

public class SetBatch : Operation
{
    public static string[] Opcodes = ["sb"];
    public static int ArgumentCount = 3;
    
    // sb -785498334 Lock 1
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var deviceTypeHash = Args[0];
        var propertyName = Args[1];
        var value = Args[2];
        
        var key = (int)emulator.InferValue(deviceTypeHash);
        if (!emulator.BatchDevices.ContainsKey(key))
        {
            emulator.BatchDevices[key] = new Device();
        }
        
        emulator.BatchDevices[key].SetProperty(propertyName, emulator.InferValue(value));

        emulator.ProgramCounter++;
    }
}