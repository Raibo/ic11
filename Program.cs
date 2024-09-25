using Antlr4.Runtime;
using ic11.TreeProcessing;
using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;
using Microsoft.Win32;
using System.Text;
using static Ic11Parser;

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
        var visitor = new TestVisitor(context);
        
        IValue result = visitor.Visit(tree);

        GiveRegisters(context);

        var sb = new StringBuilder();

        foreach (var instruction in context.Instructions)
        {
            Console.WriteLine(instruction.Render());
            sb.AppendLine(instruction.Render());
        }
    }

    private static void GiveRegisters(CompileContext context)
    {
        Stack<string> availableRegisters = new(new[] { "r14", "r13", "r12", "r11", "r10", "r9", "r8", "r7", "r6", "r5", "r4", "r3", "r2", "r1", "r0" });

        for(int i = 0; i < context.Instructions.Count; i++)
        {
            var instruction = context.Instructions[i];
            var scope = instruction.Scope;

            if (scope is null)
                continue;

            var purgeCandidates = scope.LocalVariables
                .Where(v => !v.Purged)
                .Where(v => v.Register is not null)
                .Where(v => v.LastInstructionIndex < i)
                .ToList();

            foreach (var purgeCandidate in purgeCandidates)
            {
                availableRegisters.Push(purgeCandidate.Register!);
                purgeCandidate.Purged = true;
                Console.WriteLine($"[{i}/{scope.Id}] Variable {purgeCandidate.Name}({purgeCandidate.LastInstructionIndex}) no longer needs register {purgeCandidate.Register}");
            }

            var newVar = scope.LocalVariables.FirstOrDefault(v => v.FirstInstructionIndex == i);

            if (newVar is not null)
            {
                var register = availableRegisters.Pop();
                newVar.Register = register;
                Console.WriteLine($"[{i}/{scope.Id}] Variable {newVar.Name}({newVar.LastInstructionIndex}) is given register {register}");
            }
        }
    }
}
