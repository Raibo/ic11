using Antlr4.Runtime;
using ic11.ControlFlow.Nodes;
using ic11.ControlFlow.TreeProcessing;
using ic11.TreeProcessing;
using ic11.TreeProcessing.Context;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var path = "examples/simpleProg.ic11";
        var input = File.ReadAllText(path);

        AntlrInputStream inputStream = new AntlrInputStream(input);
        Ic11Lexer lexer = new Ic11Lexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        Ic11Parser parser = new Ic11Parser(commonTokenStream);

        var tree = parser.program(); // Assuming 'program' is the entry point of your grammar

        var context = new CompileContext();

        var methodVisitor = new MethodFindVisitor(context);
        methodVisitor.Visit(tree);

        var visitor = new CompileVisitor(context);
        visitor.Visit(tree);

        GiveRegisters(context);

        var sb = new StringBuilder();

        foreach (var instruction in context.Instructions)
        {
            Console.WriteLine(instruction.Render());
            sb.AppendLine(instruction.Render());
        }

        var flowContext = new FlowContext();
        var flowAnalyzer = new ControlFlowAnalyzerVisitor(flowContext);
        flowAnalyzer.Visit(tree);

        new ReturnStatementsVisitor().Visit((Root)flowContext.Root);
        new ScopeVisitor().Visit((Root)flowContext.Root);
        new VariableVisitor().Visit((Root)flowContext.Root);
        new RegisterVisitor().Visit((Root)flowContext.Root);

        Console.WriteLine(new ControlFlowTreeVisualizer().Visualize(flowContext.Root));
    }

    private static void GiveRegisters(CompileContext context)
    {
        Stack<string> availableRegisters = new(new[] { "r14", "r13", "r12", "r11", "r10", "r9", "r8", "r7", "r6", "r5", "r4", "r3", "r2", "r1", "r0" });

        for (int i = 0; i < context.Instructions.Count; i++)
        {
            var instruction = context.Instructions[i];
            var scope = instruction.Scope;

            if (scope is null)
                continue;

            var purgeCandidates = scope.LocalVariables
                .Where(v => !v.IsPurged)
                .Where(v => v.Register is not null)
                .Where(v => v.LastInstructionIndex < i)
                .ToList();

            foreach (var purgeCandidate in purgeCandidates)
            {
                availableRegisters.Push(purgeCandidate.Register!);
                purgeCandidate.IsPurged = true;
            }

            var newVar = scope.LocalVariables.FirstOrDefault(v => v.FirstInstructionIndex == i);

            if (newVar is not null)
            {
                var register = availableRegisters.Pop();
                newVar.Register = register;
            }
        }
    }
}
