namespace ic11.Tests.Operations;

public class Math2 : Operation
{
    public static string[] Opcodes = ["seqz"];
    public static int ArgumentCount = 2;

    // seqz r0 r2
    
    public override void Execute(Emulator emulator)
    {
        //Device = args[0];
        //PropertyName = args[1];
        //Value = Convert.ToDouble(args[2]);
        
        //var d = emulator.deviceAliases[Device];
        //d.SetProperty(PropertyName, Value);
        
        throw new NotImplementedException();
    }
}