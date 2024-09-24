using Antlr4.Runtime;
using ic11.TreeProcessing;
using ic11.TreeProcessing.Context;
using ic11.TreeProcessing.Results;
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

        var sb = new StringBuilder();

        foreach (var instruction in context.Instructions)
        {
            Console.WriteLine(instruction.Render());
            sb.AppendLine(instruction.Render());
        }

        foreach (var variable in context.GlobalVariables)
        {
            Console.WriteLine($"{variable.Name}\t{variable.FirstInstructionIndex}\t{variable.LastInstructionIndex}");
        }
    }
}
