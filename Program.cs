using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ic11.TreeProcessing;

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

        // print
        //Console.WriteLine(tree.ToStringTree(parser));

        // Traverse the parse tree
        ParseTreeWalker walker = new ParseTreeWalker();
        Ic11Listener listener = new Ic11Listener();

        // First pass
        walker.Walk(listener, tree);

        //foreach (var (alias, pin) in listener.TreeContext.PinAliases)
        //    Console.WriteLine($"{alias} = {pin}");

        var registers = new List<string> { "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7", "r8", "r9", "r10", "r11", "r12", "r13", "r14", "r15", };

        //Console.WriteLine(listener.Writer.ToString());
    }
}
