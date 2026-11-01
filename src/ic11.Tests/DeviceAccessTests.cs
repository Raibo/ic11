using ic11.Emulator;
using ic11.Tests.Utils;

namespace ic11.Tests;

[TestClass]
public sealed class DeviceAccessTests
{
    [TestMethod]
    public void PinAliasesFollowReconnectionBetweenTicks()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Target d0;
            void Main() { Target.Setting = 1; yield; Target.Setting = 2; }
            """);
        var original = emulator.Devices[0]!;
        emulator.Run();
        Assert.AreEqual(1, original.GetProperty("Setting"));
        var replacement = new Device();
        emulator.Devices[0] = replacement;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(1, original.GetProperty("Setting"));
        Assert.AreEqual(2, replacement.GetProperty("Setting"));
    }

    [TestMethod]
    public void PinAliasConnectionChecksSeeDisconnection()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Target d0;
            void Main() { yield; Base.Setting = Target.IsSet; }
            """);
        emulator.Run();
        emulator.Devices[0] = null;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(0, emulator.Devices[6]!.GetProperty("Setting"));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(2)]
    [DataRow(5)]
    public void SlotOperandsReachTheCorrectDeviceAndIndex(int index)
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            pin Output d1;
            void Main() {
                var index = Input.Setting;
                Output.Slots[index].Setting = Input.Slots[index + 1].Quantity;
            }
            """);
        var input = emulator.Devices[0]!;
        var output = emulator.Devices[1]!;
        input.SetProperty("Setting", index);
        input.SetSlotProperty(index + 1, "Quantity", 17 + index);
        input.SetSlotProperty(index, "Quantity", -100);
        output.SetSlotProperty(index + 1, "Setting", -200);
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(17 + index, output.GetSlotProperty(index, "Setting"));
        Assert.AreEqual(-200, output.GetSlotProperty(index + 1, "Setting"));
        var read = emulator.DeviceAccesses.Single(a => a.Kind == DeviceAccessKind.ReadSlot);
        var write = emulator.DeviceAccesses.Single(a => a.Kind == DeviceAccessKind.WriteSlot);
        Assert.AreSame(input, read.Device);
        Assert.AreEqual(index + 1, read.Index);
        Assert.AreEqual("Quantity", read.Property);
        Assert.AreSame(output, write.Device);
        Assert.AreEqual(index, write.Index);
        Assert.AreEqual("Setting", write.Property);
    }

    [TestMethod]
    public void DynamicReferenceIdsDoNotAliasAnotherDevice()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            void Main() {
                var id = Input.Setting;
                DeviceWithId(id + 1).Setting = DeviceWithId(id).Setting + 5;
            }
            """);
        emulator.Devices[0]!.SetProperty("Setting", 1000);
        var source = emulator.RegisterDevice(1000, 123);
        var target = emulator.RegisterDevice(1001, 123);
        var unrelated = emulator.RegisterDevice(2001, 123);
        source.SetProperty("Setting", 7);
        unrelated.SetProperty("Setting", 99);
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(12, target.GetProperty("Setting"));
        Assert.AreEqual(7, source.GetProperty("Setting"));
        Assert.AreEqual(99, unrelated.GetProperty("Setting"));
        Assert.AreSame(target, emulator.DeviceAccesses.Single(a => a.Kind == DeviceAccessKind.WriteProperty).Device);
    }

    [TestMethod]
    public void IndexedPinConnectionGuardAvoidsWritesToMissingDevices()
    {
        var emulator = CompilerTestHarness.Create("""
            void Main() {
                for (var i = 0; i < 6; i = i + 1) {
                    if (Pins[i].IsSet) { Pins[i].Setting = 40 + i; }
                }
            }
            """);
        emulator.Devices[1] = null;
        emulator.Devices[4] = null;
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        foreach (var i in new[] { 0, 2, 3, 5 })
            Assert.AreEqual(40 + i, emulator.Devices[i]!.GetProperty("Setting"));
        Assert.AreEqual(4, emulator.DeviceAccesses.Count(a => a.Kind == DeviceAccessKind.WriteProperty));
    }

    [TestMethod]
    public void NamedBatchWritesTouchOnlyMatchingTargets()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            void Main() {
                var name = Input.Setting;
                DevicesOfType(123).WithName(name).On = 1;
            }
            """);
        emulator.Devices[0]!.SetProperty("Setting", 456);
        var a = emulator.RegisterDevice(1001, 123, 456);
        var b = emulator.RegisterDevice(1002, 123, 456);
        var wrongName = emulator.RegisterDevice(1003, 123, 789);
        var wrongType = emulator.RegisterDevice(1004, 999, 456);
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(1, a.GetProperty("On"));
        Assert.AreEqual(1, b.GetProperty("On"));
        Assert.AreEqual(0, wrongName.GetProperty("On"));
        Assert.AreEqual(0, wrongType.GetProperty("On"));
        CollectionAssert.AreEquivalent(new[] { a, b },
            emulator.DeviceAccesses.Where(x => x.Kind == DeviceAccessKind.WriteProperty).Select(x => x.Device).ToArray());
    }

    [TestMethod]
    [DataRow(false, false, "Average", 4.0)]
    [DataRow(false, false, "Sum", 8.0)]
    [DataRow(false, false, "Minimum", 2.0)]
    [DataRow(false, false, "Maximum", 6.0)]
    [DataRow(true, false, "Average", 4.0)]
    [DataRow(true, false, "Sum", 8.0)]
    [DataRow(true, false, "Minimum", 2.0)]
    [DataRow(true, false, "Maximum", 6.0)]
    [DataRow(false, true, "Average", 4.0)]
    [DataRow(false, true, "Sum", 8.0)]
    [DataRow(false, true, "Minimum", 2.0)]
    [DataRow(false, true, "Maximum", 6.0)]
    [DataRow(true, true, "Average", 4.0)]
    [DataRow(true, true, "Sum", 8.0)]
    [DataRow(true, true, "Minimum", 2.0)]
    [DataRow(true, true, "Maximum", 6.0)]
    public void BatchReadsFilterAndAggregateTheCorrectOperands(bool named, bool slot, string mode, double expected)
    {
        var filter = named ? ".WithName(456)" : "";
        var target = slot ? ".Slots[2]" : "";
        var emulator = CompilerTestHarness.Create($"void Main() {{ Base.Setting = DevicesOfType(123){filter}{target}.Setting.{mode}; }}");
        var a = emulator.RegisterDevice(1001, 123, 456);
        var b = emulator.RegisterDevice(1002, 123, 456);
        var excluded = emulator.RegisterDevice(1003, named ? 123 : 999, 789);
        a.SetProperty("Setting", 2);
        b.SetProperty("Setting", 6);
        excluded.SetProperty("Setting", 1000);
        a.SetSlotProperty(2, "Setting", 2);
        b.SetSlotProperty(2, "Setting", 6);
        a.SetSlotProperty(1, "Setting", -999);
        b.SetSlotProperty(1, "Setting", -999);
        excluded.SetSlotProperty(2, "Setting", 1000);
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(expected, emulator.Devices[6]!.GetProperty("Setting"));
        var reads = emulator.DeviceAccesses.Where(x => x.Kind == (slot ? DeviceAccessKind.ReadSlot : DeviceAccessKind.ReadProperty)).ToArray();
        CollectionAssert.AreEquivalent(new[] { a, b }, reads.Select(x => x.Device).ToArray());
        if (slot)
            Assert.IsTrue(reads.All(x => x.Index == 2));
    }

    [TestMethod]
    public void BatchSlotWritesDoNotTouchOtherSlotsOrTypes()
    {
        var emulator = CompilerTestHarness.Create("void Main() { DevicesOfType(123).Slots[2].Setting = 42; }");
        var a = emulator.RegisterDevice(1001, 123);
        var b = emulator.RegisterDevice(1002, 123);
        var other = emulator.RegisterDevice(1003, 999);
        a.SetSlotProperty(1, "Setting", 11);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(42, a.GetSlotProperty(2, "Setting"));
        Assert.AreEqual(42, b.GetSlotProperty(2, "Setting"));
        Assert.AreEqual(0, other.GetSlotProperty(2, "Setting"));
        Assert.AreEqual(11, a.GetSlotProperty(1, "Setting"));
    }

    [TestMethod]
    public void DeviceStackReadsWritesAndClearsUseTheRequestedTarget()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Memory d0;
            void Main() {
                Memory.Stack[3] = 42;
                DeviceWithId(1001).Stack[4] = Memory.Stack[3] + 1;
                Base.Setting = DeviceWithId(1001).Stack[4];
                Memory.Stack.Clear();
                DeviceWithId(1001).Stack.Clear();
            }
            """);
        var remote = emulator.RegisterDevice(1001, 123);
        var untouched = emulator.RegisterDevice(1002, 123);
        untouched.Stack[4] = 99;
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(43, emulator.Devices[6]!.GetProperty("Setting"));
        Assert.IsTrue(emulator.Devices[0]!.Stack.All(x => x == 0));
        Assert.IsTrue(remote.Stack.All(x => x == 0));
        Assert.AreEqual(99, untouched.Stack[4]);
        var clears = emulator.DeviceAccesses.Where(x => x.Kind == DeviceAccessKind.ClearStack).ToArray();
        CollectionAssert.AreEquivalent(new[] { emulator.Devices[0]!, remote }, clears.Select(x => x.Device).ToArray());
    }

    [TestMethod]
    public void ReagentAccessPreservesModeTypeAndDevice()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Fabricator d0;
            void Main() {
                Base.Setting = Fabricator.Reagent[42].Required;
                Base.Mode = Pins[0].Reagent[43].Source;
            }
            """);
        emulator.Devices[0]!.Reagents[("Required", 42)] = 7;
        emulator.Devices[0]!.Reagents[("Contents", 42)] = 99;
        emulator.Devices[0]!.ReagentSources[43] = 12345;
        emulator.RecordDeviceAccess = true;
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(7, emulator.Devices[6]!.GetProperty("Setting"));
        Assert.AreEqual(12345, emulator.Devices[6]!.GetProperty("Mode"));
        var read = emulator.DeviceAccesses.Single(x => x.Kind == DeviceAccessKind.ReadReagent);
        Assert.AreEqual("Required", read.Property);
        Assert.AreEqual(42, read.Index);
    }

    [TestMethod]
    public void RepeatedReadsAndWritesRemainOrderedAcrossYieldAndSleep()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            pin Output d1;
            void Main() {
                Output.On = 0;
                var a = Input.Setting;
                var b = Input.Setting;
                Output.On = a + b;
                yield;
                Output.Setting = 9;
                sleep(1);
                Output.On = 2;
            }
            """);
        emulator.Devices[0]!.SetProperty("Setting", 1);
        emulator.RecordDeviceAccess = true;
        emulator.Run();
        CollectionAssert.AreEqual(new[] { DeviceAccessKind.WriteProperty, DeviceAccessKind.ReadProperty,
            DeviceAccessKind.ReadProperty, DeviceAccessKind.WriteProperty }, emulator.DeviceAccesses.Select(x => x.Kind).ToArray());
        Assert.AreEqual(0, emulator.DeviceAccesses[0].Value);
        Assert.AreEqual(2, emulator.DeviceAccesses[3].Value);
        emulator.Run();
        Assert.AreEqual("Setting", emulator.DeviceAccesses[^1].Property);
        Assert.AreEqual(9, emulator.DeviceAccesses[^1].Value);
        int beforeSleep = emulator.DeviceAccesses.Count;
        emulator.Run();
        Assert.AreEqual(beforeSleep, emulator.DeviceAccesses.Count);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual("On", emulator.DeviceAccesses[^1].Property);
        Assert.AreEqual(4L, emulator.DeviceAccesses[^1].Tick);
    }
}
