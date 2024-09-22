using System;
using Antlr4.Runtime;

class Program
{
    static void Main(string[] args)
    {
        var file = "examples/test.ic11";
        var input = System.IO.File.ReadAllText(file);
         
        AntlrInputStream inputStream = new AntlrInputStream(input);
        Ic11Lexer lexer = new Ic11Lexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        Ic11Parser parser = new Ic11Parser(commonTokenStream);

        var tree = parser.program(); // Assuming 'program' is the entry point of your grammar

        Console.WriteLine(tree.ToStringTree(parser));
    }
}