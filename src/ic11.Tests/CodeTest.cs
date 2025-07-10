namespace ic11.Tests;

[TestClass]
public sealed partial class CodeTest
{
    private readonly Emulator _emulator = new Emulator();
    
    [TestMethod]
    public void TestMethod1()
    {
        //var program = File.ReadAllLines("../../../../../examples/hangar/air_control.ic10");
        var program = File.ReadAllLines("../../../../../examples/rocket/rocket_pump_control.ic10");
        
        //Console.WriteLine(program.Aggregate("", (current, line) => current + line + "\n"));
        
        _emulator.LoadProgram(program);
        _emulator.Run();
        _emulator.PrintSummary();
        
        Assert.IsTrue(true);
    }
}