using System.IO.Hashing;
using System.Text;

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

    public static decimal Hash(string input) =>
        (int)Crc32.HashToUInt32(Encoding.ASCII.GetBytes(input));
}
