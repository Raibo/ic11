namespace ic11.Tests.Operations;

public class SetBatchNamed : Operation
{
    public static string[] Opcodes = ["sbn"];
    public static int ArgumentCount = 4;
    
    // sbn -785498334 -2030210773 Lock 1
    public override void Execute(Emulator emulator)
    {
        var deviceTypeHash = Args[0];
        var nameHash = Args[1];
        var propertyName = Args[2];
        var value = Args[3];


        var key = (int)emulator.InferValue(deviceTypeHash);
        if (!emulator.BatchDevices.ContainsKey(key))
        {
            emulator.BatchDevices[key] = new Device();
        }
        
        emulator.BatchDevices[key].SetProperty(propertyName, emulator.InferValue(value));

        emulator.ProgramCounter++;
    }
}