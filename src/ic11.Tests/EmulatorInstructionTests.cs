using ic11.Emulator;

namespace ic11.Tests;

[TestClass]
public sealed class EmulatorInstructionTests
{
    private static Emulator.Emulator Load(params string[] lines)
    {
        var emulator = new Emulator.Emulator();
        emulator.LoadProgram(lines);
        return emulator;
    }

    [TestMethod]
    [DataRow("abs", -2.5, 2.5)]
    [DataRow("sqrt", 9.0, 3.0)]
    [DataRow("exp", 0.0, 1.0)]
    [DataRow("exp", 1.0, Math.E)]
    [DataRow("log", 1.0, 0.0)]
    [DataRow("log", Math.E, 1.0)]
    [DataRow("sin", 0.0, 0.0)]
    [DataRow("sin", Math.PI / 2, 1.0)]
    [DataRow("cos", 0.0, 1.0)]
    [DataRow("cos", Math.PI, -1.0)]
    [DataRow("tan", 0.0, 0.0)]
    [DataRow("tan", Math.PI / 4, 1.0)]
    [DataRow("asin", 0.0, 0.0)]
    [DataRow("asin", 1.0, Math.PI / 2)]
    [DataRow("acos", 1.0, 0.0)]
    [DataRow("acos", 0.0, Math.PI / 2)]
    [DataRow("atan", 0.0, 0.0)]
    [DataRow("atan", 1.0, Math.PI / 4)]
    [DataRow("ceil", -1.5, -1.0)]
    [DataRow("floor", -1.5, -2.0)]
    [DataRow("trunc", -1.5, -1.0)]
    [DataRow("round", 2.5, 2.0)]
    [DataRow("round", 3.5, 4.0)]
    [DataRow("not", 3.0, -4.0)]
    [DataRow("seqz", 0.0, 1.0)]
    [DataRow("snez", 2.0, 1.0)]
    [DataRow("snez", 0.0, 0.0)]
    [DataRow("sgez", 0.0, 1.0)]
    [DataRow("sgez", -1.0, 0.0)]
    [DataRow("sgtz", 0.0, 0.0)]
    [DataRow("sgtz", 1.0, 1.0)]
    [DataRow("slez", 0.0, 1.0)]
    [DataRow("slez", 1.0, 0.0)]
    [DataRow("sltz", -1.0, 1.0)]
    [DataRow("sltz", 0.0, 0.0)]
    [DataRow("snan", double.NaN, 1.0)]
    [DataRow("snan", 1.0, 0.0)]
    [DataRow("snanz", double.NaN, 0.0)]
    [DataRow("snanz", 1.0, 1.0)]
    public void UnaryInstructionsExecute(string opcode, double input, double expected)
    {
        var emulator = Load($"{opcode} r1 r0");
        emulator.Registers[0] = input;
        emulator.Run();
        Assert.AreEqual(expected, emulator.Registers[1], 1e-12);
    }

    [TestMethod]
    [DataRow("select", 0.0, 10.0, 20.0, 20.0)]
    [DataRow("select", -1.0, 10.0, 20.0, 10.0)]
    [DataRow("sap", 100.0, 101.0, 0.02, 1.0)]
    [DataRow("sna", 100.0, 101.0, 0.02, 0.0)]
    [DataRow("sap", 100.0, 110.0, 0.01, 0.0)]
    [DataRow("sna", 100.0, 110.0, 0.01, 1.0)]
    public void TernaryInstructionsExecute(string opcode, double a, double b, double c, double expected)
    {
        var emulator = Load($"{opcode} r3 r0 r1 r2");
        emulator.Registers[0] = a;
        emulator.Registers[1] = b;
        emulator.Registers[2] = c;
        emulator.Run();
        Assert.AreEqual(expected, emulator.Registers[3]);
    }

    [TestMethod]
    public void NamedBatchWritesFilterBothPrefabAndName()
    {
        var emulator = Load("sbn 123 456 Setting 42");
        var selected = new Device();
        selected.SetProperty("PrefabHash", 123);
        selected.SetProperty("NameHash", 456);
        var otherName = new Device();
        otherName.SetProperty("PrefabHash", 123);
        otherName.SetProperty("NameHash", 789);
        var otherType = new Device();
        otherType.SetProperty("PrefabHash", 999);
        otherType.SetProperty("NameHash", 456);
        emulator.BatchDevices.Add(1001, selected);
        emulator.BatchDevices.Add(1002, otherName);
        emulator.BatchDevices.Add(1003, otherType);
        emulator.Run();
        Assert.AreEqual(42, selected.GetProperty("Setting"));
        Assert.AreEqual(0, otherName.GetProperty("Setting"));
        Assert.AreEqual(0, otherType.GetProperty("Setting"));
        Assert.AreEqual(3, emulator.BatchDevices.Count, "A batch write must not invent a device.");
    }

    [TestMethod]
    [DataRow("l", "s")]
    [DataRow("ld", "sd")]
    public void ReferenceAddressingSupportsImmediateAndRegisterIds(string load, string store)
    {
        var emulator = Load($"{load} r0 1001 Setting", "move r1 1002", $"{store} r1 Setting r0");
        var source = emulator.RegisterDevice(1001, 123);
        var destination = emulator.RegisterDevice(1002, 123);
        source.SetProperty("Setting", 42);
        emulator.Run();
        Assert.AreEqual(42, destination.GetProperty("Setting"));
        Assert.AreEqual(42, source.GetProperty("Setting"));
    }

    [TestMethod]
    [DataRow("Average")]
    [DataRow("Sum")]
    [DataRow("Minimum")]
    [DataRow("Maximum")]
    public void EmptyBatchReadsHaveExplicitResults(string mode)
    {
        var emulator = Load($"lb r0 123 Setting {mode}");
        emulator.Run();
        var actual = emulator.Registers[0];
        switch (mode)
        {
            case "Average": Assert.IsTrue(double.IsNaN(actual)); break;
            case "Sum": Assert.AreEqual(0, actual); break;
            case "Minimum": Assert.IsTrue(double.IsPositiveInfinity(actual)); break;
            case "Maximum": Assert.IsTrue(double.IsNegativeInfinity(actual)); break;
        }
        Assert.AreEqual(0, emulator.BatchDevices.Count);
    }

    [TestMethod]
    public void MissingReferenceIdIsAnErrorInsteadOfAnInventedDevice()
    {
        var emulator = Load("ld r0 1001 Setting");
        var error = Assert.ThrowsException<Exception>(() => emulator.Run());
        StringAssert.Contains(error.Message, "1001");
        Assert.AreEqual(0, emulator.BatchDevices.Count);
    }

    [TestMethod]
    public void UnmatchedBatchWriteDoesNotCreateDevices()
    {
        var emulator = Load("sb 123 Setting 1", "sbn 123 456 Setting 2");
        emulator.Run();
        Assert.AreEqual(0, emulator.BatchDevices.Count);
    }

    [TestMethod]
    public void UnsupportedInstructionIsReportedExplicitly()
    {
        var error = Assert.ThrowsException<NotSupportedException>(() => Load("unsupported r0 1"));
        StringAssert.Contains(error.Message, "unsupported");
    }

    [TestMethod]
    public void HaltPreventsFollowingDeviceWrites()
    {
        var emulator = Load("s db Setting 1", "hcf", "s db Setting 2");
        emulator.Run(10);
        Assert.IsTrue(emulator.Stopped);
        Assert.AreEqual(1, emulator.Devices[6]!.GetProperty("Setting"));
    }
}
