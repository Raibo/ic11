using Antlr4.Runtime;
using ic11.ControlFlow.Context;
using ic11.ControlFlow.InstructionsProcessing;
using ic11.ControlFlow.Nodes;
using ic11.ControlFlow.TreeProcessing;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var path = "examples/simpleProg.ic11";
        if (args.Length > 0)
            path = args[0];

        if (args.Length > 1)
        {
            Console.WriteLine("Usage: ic11 [path]");
        }
        
        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"File {path} does not exist");
            return;
        }

        var input = File.ReadAllText(path);

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
        new ScopeVisitor(flowContext).Visit((Root)flowContext.Root);
        new VariableVisitor(flowContext).Visit((Root)flowContext.Root);
        new RegisterVisitor().Visit((Root)flowContext.Root);
        var instructions = new Ic10CommandGenerator(flowContext).Visit((Root)flowContext.Root);

        UselessMoveRemover.Remove(instructions);
        LabelsRemoval.RemoveLabels(instructions);

        foreach (var item in instructions)
            Console.WriteLine(item.Render());

        //Console.WriteLine(new ControlFlowTreeVisualizer().Visualize(flowContext.Root));
    }
}
