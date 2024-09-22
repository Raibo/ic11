using System;
using Antlr4.Runtime;

class Program
{
    static void Main(string[] args)
    {
        var input = "Pin FabElectro = d0;\r\nPin DialElectro = d3;\r\n\r\nvoid Main()\r\n{   \r\n    while()\r\n    {\r\n        yield;\r\n        \r\n        if (FabElectro.IsSet && DialElectro.IsSet)\r\n        {            \r\n            if (!FabElectro.Activate)\r\n            {\r\n                FabElectro.ClearMemory = true;\r\n                return;\r\n            }\r\n            \r\n            var craftsDone = FabElectro.ExportCount;\r\n            var craftsTarget = DialElectro.Setting;\r\n            \r\n            var craftsLeft = craftsTarget - craftsDone;\r\n            \r\n            DialElectro.Setting = craftsLeft;\r\n            FabElectro.ClearMemory = true;\r\n            \r\n            if (craftsLeft <= 0)\r\n            {\r\n                DialElectro.Setting = 1;\r\n                FabElectro.Activate = false;\r\n            }\r\n        }\r\n    }\r\n}";

        AntlrInputStream inputStream = new AntlrInputStream(input);
        Ic11Lexer lexer = new Ic11Lexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        Ic11Parser parser = new Ic11Parser(commonTokenStream);

        var tree = parser.program(); // Assuming 'program' is the entry point of your grammar

        Console.WriteLine(tree.ToStringTree(parser));
    }
}