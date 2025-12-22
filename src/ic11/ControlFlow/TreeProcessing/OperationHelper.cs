using System.IO.Hashing;
using System.Text;

namespace ic11.ControlFlow.TreeProcessing;
public static class OperationHelper
{
    public static Dictionary<string, string> SymbolsUnaryOpMap = new()
    {
        ["!"] = "_not",
        ["-"] = "_neg",
        ["~"] = "not",
    };

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
        ["<<l"] = "sll",
        [">>l"] = "srl",
        ["<<a"] = "sla",
        [">>a"] = "sra",
    };

    public static Dictionary<string, string> SymbolsTernaryOpMap = new()
    {
        ["?"] = "select",
        ["~="] = "sap",
        ["~=="] = "sap",
        ["~!="] = "sna",
    };

    public static decimal Hash(string input) =>
        (int)Crc32.HashToUInt32(Encoding.ASCII.GetBytes(input));

    public static decimal ToASCII(ReadOnlySpan<char> input)
    {
        const int MAX_INT_DOUBLE = 53 / 8;

        int chars = 0;
        long result = 0;
        
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            
            if (c == '\\')
            {
                i++;
                
                if (i >= input.Length)
                    throw new InvalidOperationException("Escape character at end of string");

                c = input[i];
            }
            
            chars++;
            
            if (chars > MAX_INT_DOUBLE)
                throw new InvalidOperationException("String is too long to convert");
                
            if (!char.IsAscii(c))
                throw new InvalidOperationException("String does contain non-ASCII character");
            
            result <<= 8;
            result |= (byte)c;
        }
        
        return result;
    }
}
