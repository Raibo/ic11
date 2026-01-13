namespace ic11.Tests;

using ic11.Tests.Utils;

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

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(1588896491, dev.GeneralProperties["Hash"]);
        Assert.AreEqual(0x43_4C_45_41_52, dev.GeneralProperties["String"]);
        Assert.AreEqual(0, dev.GeneralProperties["EmptyString"]);
        Assert.AreEqual('\\', dev.GeneralProperties["StringEscape"]);
        Assert.AreEqual('\'', dev.GeneralProperties["StringQuote"]);
    }
}