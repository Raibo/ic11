namespace ic11.Tests;

using ic11.ControlFlow.TreeProcessing;

[TestClass]
public sealed partial class CodeTest
{
    private readonly Emulator _emulator = new Emulator();

    [TestMethod]
    public void TestMethod1()
    {
        //var program = File.ReadAllLines("../../../../../examples/hangar/air_control.ic10");
        //var program = File.ReadAllLines("../../../../../examples/rocket/rocket_pump_control.ic10");
        var program = File.ReadAllLines("../../../../../examples/base_air/propellant.ic10");

        //Console.WriteLine(program.Aggregate("", (current, line) => current + line + "\n"));

        _emulator.LoadProgram(program);
        
        /*
            pin Pump d0;
            pin PaInput d1;
            pin PaOutput d2;
            pin CanisterSlot d3;
         */
        
        var canisterSlot = _emulator.Devices[3];
        var paInput = _emulator.Devices[1];
        var paOutput = _emulator.Devices[2];
        
        canisterSlot.SetSlotProperty(0, "PrefabHash",
            (double)OperationHelper.Hash("ItemGasCanisterSmart")); // "ItemGasCanisterSmart"
        canisterSlot.SetProperty("RatioCarbonDioxide", 1);

        paInput.SetProperty("TotalMoles", 1);
        paInput.SetProperty("Volume", 1);
        paInput.SetProperty("Temperature", 275);
        
        paOutput.SetProperty("TotalMoles", 1);
        paOutput.SetProperty("Volume", 1);
        paOutput.SetProperty("Pressure", 275);
        
        _emulator.Run();
        _emulator.PrintSummary();

        Assert.IsTrue(true);
    }
}