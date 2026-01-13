namespace ic11.Tests;

using ic11.Tests.Utils;

[TestClass]
public sealed class ConstantDeclarationTests
{
    [TestMethod]
    public void TestLiteralOperations()
    {
        var code = @"
            pin Display d0;

            const ADD = 4 + 2;
            const MUL = 6 * 7;
            const SUB = 2 - 4;

            void Main()
            {
                Display.ADD = ADD;
                Display.MUL = MUL;
                Display.SUB = SUB;
            }
        ";

        var emulator = EmulatorHelper.Run(code);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(6, dev.GeneralProperties["ADD"]);
        Assert.AreEqual(42, dev.GeneralProperties["MUL"]);
        Assert.AreEqual(-2, dev.GeneralProperties["SUB"]);
    }

    [TestMethod]
    public void TestConstantOperations()
    {
        var code = @"
            pin Display d0;

            const FOUR = 4;
            const TWO = 2;
            const SIX = 6;
            const SEVEN = 7;

            const ADD = FOUR + TWO;
            const MUL = SIX * SEVEN;
            const SUB = TWO - FOUR;

            void Main()
            {
                Display.ADD = ADD;
                Display.MUL = MUL;
                Display.SUB = SUB;
            }
        ";

        var emulator = EmulatorHelper.Run(code);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(6, dev.GeneralProperties["ADD"]);
        Assert.AreEqual(42, dev.GeneralProperties["MUL"]);
        Assert.AreEqual(-2, dev.GeneralProperties["SUB"]);
    }

    [TestMethod]
    public void TestConstantOrder()
    {
        var code = @"
            pin Display d0;
            const C1 = 0;
            const C2 = C1 + 1;
            const C3 = C1 + 1;
            const C4 = C1 + 1;
            const C5 = C1 + 1;
            const C6 = C1 + 1;
            const C7 = C1 + 1;
            const C8 = C1 + 1;
            const C9 = C1 + 1;
            const C10 = C1 + 1;
            const C11 = C1 + 1;
            const C12 = C1 + 1;
            const C13 = C1 + 1;
            const C14 = C1 + 1;
            const C15 = C1 + 1;

            void Main() {
                Display.Result = C1;
            }
        ";

        var emulator = EmulatorHelper.Run(code);
        var dev = emulator.Devices[0]!;
        Assert.AreEqual(0, dev.GeneralProperties["Result"]);
    }

    [TestMethod]
    public void TestIncorrectOrder()
    {
        var code = @"
            const C1 = 0;
            const C3 = C1 + C2;
            const C2 = 0;

            void Main() {}
        ";

        Assert.ThrowsException<System.Reflection.TargetInvocationException>(() => Program.CompileText(code));
    }

    [TestMethod]
    public void TestNonConstant()
    {
        var code = @"
            const C1 = rand();

            void Main() {}
        ";

        Assert.ThrowsException<System.Reflection.TargetInvocationException>(() => Program.CompileText(code));
    }
}