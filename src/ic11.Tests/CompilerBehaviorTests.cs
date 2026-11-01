using System.Globalization;
using System.Text.RegularExpressions;
using ic11.Tests.Utils;

namespace ic11.Tests;

[TestClass]
public sealed class CompilerBehaviorTests
{
    [TestMethod]
    [DataRow("x * 3 + 2", -2.0, -4.0)]
    [DataRow("x / 4", 7.0, 1.75)]
    [DataRow("x % 5", -7.0, 3.0)]
    [DataRow("((x + 3) * 2 - (x - 1)) / 2", 5.0, 6.0)]
    [DataRow("x << 2", 12.0, 48.0)]
    [DataRow("x >> 2", 12.0, 3.0)]
    [DataRow("x & 15", 18.0, 2.0)]
    [DataRow("x & 3", -1.5, 3.0)]
    [DataRow("x | 3", 8.0, 11.0)]
    [DataRow("x ^ 3", 7.0, 4.0)]
    [DataRow("~x", 3.0, -4.0)]
    [DataRow("!x", 0.0, 1.0)]
    [DataRow("!x", -2.0, 0.0)]
    [DataRow("-x", -2.0, 2.0)]
    [DataRow("abs(x)", -2.5, 2.5)]
    [DataRow("sqrt(x)", 9.0, 3.0)]
    [DataRow("round(x)", 2.5, 2.0)]
    [DataRow("round(x)", 3.5, 4.0)]
    [DataRow("floor(x)", -1.5, -2.0)]
    [DataRow("ceil(x)", -1.5, -1.0)]
    [DataRow("trunc(x)", -1.5, -1.0)]
    [DataRow("sin(x)", 0.0, 0.0)]
    [DataRow("cos(x)", 0.0, 1.0)]
    [DataRow("exp(x)", 0.0, 1.0)]
    [DataRow("log(x)", 1.0, 0.0)]
    [DataRow("(x < 0) ? 11 : 22", -1.0, 11.0)]
    [DataRow("(x < 0) ? 11 : 22", 0.0, 22.0)]
    [DataRow("sap(x, 101, 0.02)", 100.0, 1.0)]
    [DataRow("sna(x, 110, 0.01)", 100.0, 1.0)]
    public void FoldedAndRuntimeExpressionsMatchIndependentResults(string expression, double input, double expected)
    {
        var literal = input.ToString("R", CultureInfo.InvariantCulture);
        var constantExpression = Regex.Replace(expression, @"\bx\b", $"({literal})");
        foreach (bool folded in new[] { false, true })
        {
            var value = folded ? constantExpression : expression;
            var source = $"pin Input d0; pin Output d1; void Main() {{ var x = Input.Setting; Output.Setting = {value}; }}";
            var emulator = CompilerTestHarness.Create(source);
            emulator.Devices[0]!.SetProperty("Setting", input);
            CompilerTestHarness.Finish(emulator);
            Assert.AreEqual(expected, emulator.Devices[1]!.GetProperty("Setting"), 1e-10,
                $"{expression}, input={input}, constant-folded={folded}");
        }
    }

    [TestMethod]
    [DataRow(-10.0, -3.0)]
    [DataRow(0.0, 0.0)]
    [DataRow(1.0, 2.0)]
    [DataRow(7.0, -4.0)]
    [DataRow(100.0, 5.0)]
    public void NestedExpressionsPreserveBothOperands(double a, double b)
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            pin Output d1;
            void Main() {
                var a = Input.Setting;
                var b = Input.Mode;
                Output.Setting = (a + b) * (a - b);
            }
            """);
        emulator.Devices[0]!.SetProperty("Setting", a);
        emulator.Devices[0]!.SetProperty("Mode", b);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(a * a - b * b, emulator.Devices[1]!.GetProperty("Setting"));
    }

    [TestMethod]
    [DataRow(-5.0, 1)]
    [DataRow(0.0, 8)]
    [DataRow(7.0, 128)]
    [DataRow(40.0, 1)]
    public void NestedCallsPreserveLiveVariablesAndRestoreStack(double input, int instructionsPerTick)
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            pin Output d1;
            real Twice(x) { return x * 2; }
            real Mix(a, b) { var left = a * 3; return left + Twice(b) + a; }
            void Main() {
                var x = Input.Setting;
                var keep1 = x + 1;
                var keep2 = x + 2;
                var keep3 = x + 3;
                var keep4 = x + 4;
                var keep5 = x + 5;
                var keep6 = x + 6;
                var keep7 = x + 7;
                var keep8 = x + 8;
                var result = Mix(keep1, keep2);
                Output.Setting = result + keep1 + keep2 + keep3 + keep4 + keep5 + keep6 + keep7 + keep8;
            }
            """, instructionsPerTick);
        emulator.Devices[0]!.SetProperty("Setting", input);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(14 * input + 44, emulator.Devices[1]!.GetProperty("Setting"));
        Assert.AreEqual(0, emulator.Registers[emulator.StackPointerRegister]);
    }

    [TestMethod]
    [DataRow(-2.0, 10.0)]
    [DataRow(0.0, 20.0)]
    [DataRow(3.0, 30.0)]
    public void NestedBranchesReturnFromTheCorrectPath(double input, double expected)
    {
        var emulator = CompilerTestHarness.Create("""
            pin Input d0;
            pin Output d1;
            real Classify(x) {
                if (x < 0) { return 10; }
                else { if (x == 0) { return 20; } else { return 30; } }
            }
            void Main() { Output.Setting = Classify(Input.Setting); }
            """);
        emulator.Devices[0]!.SetProperty("Setting", input);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(expected, emulator.Devices[1]!.GetProperty("Setting"));
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(128)]
    public void BreakAndContinueDoNotSkipLoopUpdates(int instructionsPerTick)
    {
        var emulator = CompilerTestHarness.Create("""
            void Main() {
                var sum = 0;
                for (var i = 0; i < 10; i = i + 1) {
                    if (i == 2) continue;
                    if (i == 5) break;
                    sum = sum + i;
                }
                Base.Setting = sum;
            }
            """, instructionsPerTick);
        CompilerTestHarness.Finish(emulator);
        Assert.AreEqual(8, emulator.Devices[6]!.GetProperty("Setting"));
    }

    [TestMethod]
    public void ArraySortChecksEveryElementIncludingDuplicatesAndNegatives()
    {
        var emulator = CompilerTestHarness.Create("""
            pin Output d0;
            void Main() {
                var values = { 7, -3, 2, 9, 0, 2 };
                Sort(values, 6);
                for (var i = 0; i < 6; i = i + 1) { Output.Slots[i].Setting = values[i]; }
            }
            void Sort(values, count) {
                for (var i = 0; i < count; i = i + 1) {
                    for (var j = 0; j < count - 1; j = j + 1) {
                        if (values[j] > values[j + 1]) { Swap(values, j, j + 1); }
                    }
                }
            }
            void Swap(values, a, b) {
                var saved = values[a];
                values[a] = values[b];
                values[b] = saved;
            }
            """);
        CompilerTestHarness.Finish(emulator);
        var actual = Enumerable.Range(0, 6).Select(i => emulator.Devices[0]!.GetSlotProperty(i, "Setting")).ToArray();
        CollectionAssert.AreEqual(new double[] { -3, 0, 2, 2, 7, 9 }, actual);
    }
}
