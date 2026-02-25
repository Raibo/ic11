using System.Text;
using Antlr4.Runtime;
using ic11.ControlFlow.Context;
using ic11.ControlFlow.InstructionsProcessing;
using ic11.ControlFlow.Nodes;
using ic11.ControlFlow.TreeProcessing;
using System.Text.RegularExpressions;

namespace ic11;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1 && args.Length != 2)
        {
            Console.WriteLine("Usage: ic11 path [-w]");
            return;
        }

        var argsPath = args[0];
        var filePaths = new List<string>();
        var pathType = PathType.Nonexistant;
        var shouldSave = args.Contains("-w");

        if (File.Exists(argsPath))
            pathType = PathType.File;

        if (Directory.Exists(argsPath))
            pathType = PathType.Directory;

        if (pathType == PathType.Nonexistant)
        {
            Console.WriteLine("File or directory does not exist");
            return;
        }

        if (pathType == PathType.File)
        {
            CompileFile(argsPath, shouldSave);
            return;
        }

        if (pathType == PathType.Directory)
        {
            Console.WriteLine($"Compiling every *.ic11 file in directory {argsPath}");

            var ic11Files = Directory.EnumerateFiles(argsPath)
                .Where(p => Path.GetExtension(p).Equals(".ic11", StringComparison.InvariantCultureIgnoreCase))
                .Select(Path.GetFileName);

            foreach (var file in ic11Files)
            {
                Console.WriteLine($"\n\n{file}\n");
                CompileFile(Path.Combine(argsPath, file!), shouldSave);
            }
        }

        //Console.WriteLine(new ControlFlowTreeVisualizer().Visualize(flowContext.Root));
    }

    private static void CompileFile(string path, bool shouldSave)
    {
        var fullPath = Path.GetFullPath(path);
        var baseDir = Path.GetDirectoryName(fullPath) ?? Directory.GetCurrentDirectory();

        var input = File.ReadAllText(fullPath);

        // preprocess includes (this returns source with includes inlined)
        var expanded = IncludePreprocessor.ExpandIncludes(input, baseDir);

        var output = CompileText(expanded);
        Console.WriteLine(output);

        if (shouldSave)
        {
            var directoryPath = Path.GetDirectoryName(fullPath);
            var fileName = Path.Combine(directoryPath!, Path.GetFileNameWithoutExtension(fullPath) + ".ic10");
            File.WriteAllText(fileName, output);
        }
    }

    public static string CompileText(string input)
    {
        AntlrInputStream inputStream = new AntlrInputStream(input);
        Ic11Lexer lexer = new Ic11Lexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        Ic11Parser parser = new Ic11Parser(commonTokenStream);

        var tree = parser.program(); // Assuming 'program' is the entry point of your grammar

        var flowContext = new FlowContext();
        var flowAnalyzer = new ControlFlowBuilderVisitor(flowContext);
        flowAnalyzer.Visit(tree);

        new RootStatementsSorter().SortStatements((Root)flowContext.Root);
        new MethodsVisitor(flowContext).Visit((Root)flowContext.Root);
        new MethodCallsVisitor(flowContext).VisitRoot((Root)flowContext.Root);
        new ScopeVisitor(flowContext).Visit((Root)flowContext.Root);
        new VariableVisitor(flowContext).Visit((Root)flowContext.Root);
        new VariableCyclesAdjVisitor().VisitRoot((Root)flowContext.Root);
        new MethodCallGraphVisitor(flowContext).VisitRoot((Root)flowContext.Root);
        new RegisterVisitor(flowContext).DoWork();
        new MethodsRegisterRangesDistributor(flowContext).DoWork();
        var instructions = new Ic10CommandGenerator(flowContext).Visit((Root)flowContext.Root);

        UselessMoveRemover.Remove(instructions);
        LabelsRemoval.RemoveLabels(instructions);

        var output = new StringBuilder();

        foreach (var item in instructions)
            output.AppendLine(item.Render());

        return output.ToString();
    }

    private enum PathType
    {
        Nonexistant,
        File,
        Directory,
    }

    static class IncludePreprocessor
    {
        // Supports: #include <a.b> or #include "a.b"
        private static readonly Regex IncludeLine =
            new Regex(@"^\s*#include\s*(<([^>\r\n]+)>|""([^""\r\n]+)"")\s*$",
                      RegexOptions.Multiline);

        public static string ExpandIncludes(string input, string baseDir)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            return Expand(input, baseDir, seen);
        }

        private static string Expand(string input, string baseDir, HashSet<string> seen)
        {
            return IncludeLine.Replace(input, m =>
            {
                // group2 = <...> contents, group3 = "..." contents
                var rel = m.Groups[2].Success ? m.Groups[2].Value : m.Groups[3].Value;
                rel = rel.Trim();

                if (!rel.Contains('.'))
                    throw new Exception($"Invalid include (missing dot): {rel}");

                var full = Path.GetFullPath(Path.Combine(baseDir, rel));

                if (!File.Exists(full))
                    throw new FileNotFoundException($"Included file not found: {full}");

                if (!seen.Add(full))
                    return string.Empty;

                var includedText = File.ReadAllText(full);

                var nextBase = Path.GetDirectoryName(full) ?? baseDir;
                return Expand(includedText, nextBase, seen);
            });
        }
    }
}