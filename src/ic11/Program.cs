using System.Text;
using Antlr4.Runtime;
using ic11.ControlFlow.Context;
using ic11.ControlFlow.InstructionsProcessing;
using ic11.ControlFlow.Nodes;
using ic11.ControlFlow.TreeProcessing;

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
        var input = File.ReadAllText(path);
        var output = CompileText(input);
        Console.WriteLine(output);

        if (shouldSave)
        {
            var directoryPath = Path.GetDirectoryName(path);
            var fileName = Path.Combine(directoryPath!, Path.GetFileNameWithoutExtension(path) + ".ic10");
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
}