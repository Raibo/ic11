namespace ic11.Emulator.Operations;

public class SetBatchNamed : Operation
{
    public static string[] Opcodes = ["sbn"];
    public static int ArgumentCount = 4;
    
    // sbn -785498334 -2030210773 Lock 1
    public override void Execute(ic11.Emulator.Emulator emulator)
    {
        var deviceTypeHash = Args[0];
        var nameHash = Args[1];
        var propertyName = Args[2];
        var value = Args[3];


        var key = (int)emulator.InferValue(deviceTypeHash);
        var name = (int)emulator.InferValue(nameHash);
        var writeValue = emulator.InferValue(value);
        foreach (var device in emulator.SelectDevices(key, name))
            emulator.WriteProperty(device, propertyName, writeValue);

        emulator.ProgramCounter++;
    }
}
