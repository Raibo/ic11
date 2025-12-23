namespace ic11.Tests;

using ic11.Tests.Utils;

[TestClass]
public sealed class IfStatementTest
{
    [TestMethod]
    public void TestIfElseBlock()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
            }

            real Test(p) {
                if (p == 1) {
                    return 42;
                } else {
                    return 1337;
                }
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
    }

    [TestMethod]
    public void TestIfElseStatement()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
            }

            real Test(p) {
                if (p == 1)
                    return 42;
                else
                    return 1337;
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
    }

    [TestMethod]
    public void TestIfElseBlockStatement()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
            }

            real Test(p) {
                if (p == 1) {
                    return 42;
                } else
                    return 1337;
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
    }

    [TestMethod]
    public void TestIfElseStatementBlock()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
            }

            real Test(p) {
                if (p == 1)
                    return 42;
                else {
                    return 1337;
                }
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
    }

    [TestMethod]
    public void TestIfElseIfBlock()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
                D.C = Test(3);
            }

            real Test(p) {
                if (p == 1) {
                    return 42;
                } else if (p == 2) {
                    return 1337;
                } else {
                    return 4711;
                }
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
        Assert.AreEqual(4711, dev.GeneralProperties["C"]);
    }

    [TestMethod]
    public void TestIfElseIfStatement()
    {
        var code = @"
            pin D d0;

            void Main()
            {
                D.A = Test(1);
                D.B = Test(2);
                D.C = Test(3);
            }

            real Test(p) {
                if (p == 1)
                    return 42;
                else if (p == 2)
                    return 1337;
                else
                    return 4711;
            }
        ";

        var emulator = EmulatorHelper.Run(code, 1);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(42, dev.GeneralProperties["A"]);
        Assert.AreEqual(1337, dev.GeneralProperties["B"]);
        Assert.AreEqual(4711, dev.GeneralProperties["C"]);
    }
}
