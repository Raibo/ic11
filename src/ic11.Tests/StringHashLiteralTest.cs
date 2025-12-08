namespace ic11.Tests;

using ic11.Emulator;

[TestClass]
public sealed class StringHashLiteralTest
{
    [TestMethod]
    public void TestStringHash()
    {
        var code = @"
            pin Display d0;

            void Main()
            {
                Display.Hash = ""ItemGlassSheets"";
                Display.String = 'CLEAR';
                Display.EmptyString = '';
                Display.StringEscape = '\\';
                Display.StringQuote = '\'';
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
        Assert.AreEqual(1588896491, dev.GeneralProperties["Hash"]);
        Assert.AreEqual(0x43_4C_45_41_52, dev.GeneralProperties["String"]);
        Assert.AreEqual(0, dev.GeneralProperties["EmptyString"]);
        Assert.AreEqual('\\', dev.GeneralProperties["StringEscape"]);
        Assert.AreEqual('\'', dev.GeneralProperties["StringQuote"]);
    }
}