namespace ic11.Tests;

using ic11.Emulator;

[TestClass]
public sealed class ProductionTest
{
    private readonly Emulator _emulator = new();

    [TestMethod]
    public void TestLatheControl()
    {
        var code = @"
            pin Fab1 d0;
            pin Dial1 d3;

            pin Fab2 d1;
            pin Dial2 d4;

            pin Fab3 d2;
            pin Dial3 d5;

            void Main()
            {
                while(true)
                {
                    yield;
                    
                    for(var i = 0; i < 3; i = i + 1)
                    {
                        if (Pins[i].IsSet & Pins[i + 3].IsSet)
                            ProcessMachine(i, i + 3);
                    }
                }
            }

            void ProcessMachine(machineIndex, dialIndex)
            {
                if (!Pins[machineIndex].Activate)
                {
                    Pins[machineIndex].ClearMemory = true;
                    return;
                }
                
                var craftsDone = Pins[machineIndex].ExportCount;
                var craftsTarget = Pins[dialIndex].Setting;
                
                var craftsLeft = craftsTarget - craftsDone;
                
                Pins[dialIndex].Setting = craftsLeft;
                Pins[machineIndex].ClearMemory = true;
                
                if (craftsLeft <= 0)
                {
                    Pins[dialIndex].Setting = 1;
                    Pins[machineIndex].Activate = false;
                }
            }
        ";

        var compileText = Program.CompileText(code);

        var program = compileText.Split("\n");

        _emulator.LoadProgram(program);

        var fab1 = _emulator.Devices[0];
        var dial1 = _emulator.Devices[3];
        _emulator.Devices[1] = null;
        _emulator.Devices[4] = null;
        _emulator.Devices[2] = null;
        _emulator.Devices[5] = null;

        fab1.SetProperty("Activate", 1);
        fab1.SetProperty("ExportCount", 1);

        dial1.SetProperty("Setting", 50);

        _emulator.Run(2);

        Assert.AreEqual(49, dial1.GetProperty("Setting"));
        Assert.AreEqual(1, fab1.GetProperty("ClearMemory"));

        fab1.SetProperty("ExportCount", 0);
        fab1.SetProperty("ClearMemory", 0);

        _emulator.Run(1);

        Assert.AreEqual(49, dial1.GetProperty("Setting"));

        _emulator.PrintSummary();
    }
}