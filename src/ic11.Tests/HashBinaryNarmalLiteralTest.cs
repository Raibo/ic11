namespace ic11.Tests;

using ic11.Emulator;

[TestClass]
public sealed class HashBinaryNarmalLiteralTest
{
    [TestMethod]
    public void HashBinaryNarmalLiteralTestCase()
    {
        var code = @"
            pin Display d0;

            void Main()
            {
                Display.Binary00 = 0b000000000000000000000000;
                Display.Binary0 = 0b0;
                Display.Binary01 = 0b000000000000000000000001;
                Display.Binary1 = 0b1;
                Display.Binary07 = 0b000000000000000000000111;
                Display.Binary7_ = 0b0000_0000_0000_0000_0000_0111;
                Display.Binary7 = 0b111;
                Display.BinaryMinus7 = -0b111;

                Display.Hex1 = 0x1;
                Display.Hex01 = 0x0001;
                Display.Hex01_ = 0x0000_0001;
                Display.Hex65535 = 0xFFFF;
                Display.Hex11259375 = 0xABCDEF;
                Display.Hex11259375lowerCase = 0xabcdef;
                Display.HexMinus15 = -0xF;

                Display.Normal0 = 0;
                Display.Normal1 = 1;
                Display.Normal_minus1 = -1;
                Display.Normal15 = 15;
                Display.Normal15_73 = 15.73;
            }
        ";

        var compileText = Program.CompileText(code);
        Console.WriteLine(compileText);

        var program = compileText.Split("\n");

        Emulator emulator = new(1);
        emulator.LoadProgram(program);

        var limit = 1000;
        while (!emulator.Stopped && --limit > 0)
        {
            emulator.Run(1);
            emulator.PrintSummary();
        }

        emulator.PrintSummary();

        var dev = emulator.Devices[0]!;
        Assert.AreEqual(0, dev.GeneralProperties["Binary00"]);
        Assert.AreEqual(0, dev.GeneralProperties["Binary0"]);
        Assert.AreEqual(1, dev.GeneralProperties["Binary01"]);
        Assert.AreEqual(1, dev.GeneralProperties["Binary1"]);
        Assert.AreEqual(7, dev.GeneralProperties["Binary07"]);
        Assert.AreEqual(7, dev.GeneralProperties["Binary7_"]);
        Assert.AreEqual(7, dev.GeneralProperties["Binary7"]);
        Assert.AreEqual(-7, dev.GeneralProperties["BinaryMinus7"]);

        Assert.AreEqual(1, dev.GeneralProperties["Hex1"]);
        Assert.AreEqual(1, dev.GeneralProperties["Hex01"]);
        Assert.AreEqual(1, dev.GeneralProperties["Hex01_"]);
        Assert.AreEqual(65535, dev.GeneralProperties["Hex65535"]);
        Assert.AreEqual(11259375, dev.GeneralProperties["Hex11259375"]);
        Assert.AreEqual(11259375, dev.GeneralProperties["Hex11259375lowerCase"]);
        Assert.AreEqual(-15, dev.GeneralProperties["HexMinus15"]);

        Assert.AreEqual(0, dev.GeneralProperties["Normal0"]);
        Assert.AreEqual(1, dev.GeneralProperties["Normal1"]);
        Assert.AreEqual(-1, dev.GeneralProperties["Normal_minus1"]);
        Assert.AreEqual(15, dev.GeneralProperties["Normal15"]);
        Assert.AreEqual(15.73, dev.GeneralProperties["Normal15_73"]);
    }
}