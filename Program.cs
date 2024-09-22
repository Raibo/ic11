using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

class Program
{
    static void Main(string[] args)
    {
        var file = "examples/test.ic11";
        var input = File.ReadAllText(file);
         
        AntlrInputStream inputStream = new AntlrInputStream(input);
        Ic11Lexer lexer = new Ic11Lexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        Ic11Parser parser = new Ic11Parser(commonTokenStream);

        var tree = parser.program(); // Assuming 'program' is the entry point of your grammar

        // print
        Console.WriteLine(tree.ToStringTree(parser));
    }
}
