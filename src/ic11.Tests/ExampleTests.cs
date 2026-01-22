namespace ic11.Tests;

[TestClass]
public class ExampleTests
{
    private static readonly string EXAMPLES_DIRECTORY = Path.Combine(AppContext.BaseDirectory, "examples");
    // Paths must be relative to "examples"-directory
    private static readonly HashSet<string> KNOWN_BROKEN = [
    ];

    [TestMethod]
    [DynamicData(nameof(GetExampleFiles), DynamicDataSourceType.Method)]
    public void CompileExample(string filename)
    {
        string relativePath = Path.GetRelativePath(EXAMPLES_DIRECTORY, filename);
        if (KNOWN_BROKEN.Contains(relativePath.Replace('\\', '/')))
            Assert.Inconclusive($"Known failing example: {filename}");

        string code = File.ReadAllText(filename);
        var instructions = Program.CompileText(code);

        Assert.IsFalse(string.IsNullOrWhiteSpace(instructions));
    }

    public static IEnumerable<object[]> GetExampleFiles()
    {
        foreach (var file in Directory.EnumerateFiles(EXAMPLES_DIRECTORY, "*.ic11", SearchOption.AllDirectories))
            yield return new object[] { file };
    }
}
