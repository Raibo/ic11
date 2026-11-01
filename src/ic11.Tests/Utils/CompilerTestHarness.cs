using Antlr4.Runtime;

namespace ic11.Tests.Utils;

internal static class CompilerTestHarness
{
    public static Emulator.Emulator Create(string source, int cyclesPerTick = 128)
    {
        // A recovered syntax error must not turn a malformed fixture into a passing test.
        var errors = new FixtureErrors();
        var lexer = new Ic11Lexer(new AntlrInputStream(source));
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(errors);
        var parser = new Ic11Parser(new CommonTokenStream(lexer));
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errors);
        parser.program();
        Assert.AreEqual(0, errors.Messages.Count, string.Join(Environment.NewLine, errors.Messages));

        var assembly = Program.CompileText(source);
        var emulator = new Emulator.Emulator(cyclesPerTick);
        emulator.LoadProgram(assembly.Split('\n'));
        return emulator;
    }

    public static void Finish(Emulator.Emulator emulator, int maxTicks = 1000)
    {
        for (int i = 0; i < maxTicks && !emulator.Stopped; i++)
            emulator.Run();
        Assert.IsTrue(emulator.Stopped, $"Program did not finish within {maxTicks} ticks.\n{emulator.GetSummary()}");
    }

    private sealed class FixtureErrors : BaseErrorListener, IAntlrErrorListener<int>
    {
        public List<string> Messages { get; } = [];
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol,
            int line, int charPositionInLine, string msg, RecognitionException e) => Messages.Add($"{line}:{charPositionInLine}: {msg}");
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol,
            int line, int charPositionInLine, string msg, RecognitionException e) => Messages.Add($"{line}:{charPositionInLine}: {msg}");
    }
}
