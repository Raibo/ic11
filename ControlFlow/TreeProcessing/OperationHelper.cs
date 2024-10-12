namespace ic11.ControlFlow.TreeProcessing;
public static class OperationHelper
{
    public static Dictionary<string, string> SymbolsBinaryOpMap = new()
    {
        ["+"] = "add",
        ["-"] = "sub",
        ["*"] = "mul",
        ["/"] = "div",
        ["%"] = "mod",

        ["&"] = "and",
        ["|"] = "or",
        ["^"] = "xor",

        ["=="] = "seq",
        ["!="] = "sne",
        [">"] = "sgt",
        [">="] = "sge",
        ["<"] = "slt",
        ["<="] = "sle",

        ["<<"] = "sll",
        [">>"] = "srl",
    };

    public static Dictionary<string, string> SymbolsUnaryOpMap = new()
    {
        ["!"] = "_not",
        ["-"] = "_neg",
        ["~"] = "not",
    };
}
