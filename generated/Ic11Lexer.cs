//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Ic11.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class Ic11Lexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, PINID=14, WHILE=15, IF=16, ELSE=17, 
		YIELD=18, RETURN=19, CONTINUE=20, BREAK=21, BASE_DEVICE=22, VAR=23, CONST=24, 
		ADD=25, SUB=26, MUL=27, DIV=28, MOD=29, LT=30, GT=31, LE=32, GE=33, AND=34, 
		OR=35, EQ=36, NE=37, NEGATION=38, ABS=39, DEVICE=40, DEVICE_WITH_ID=41, 
		BOOLEAN=42, IDENTIFIER=43, INTEGER=44, REAL=45, WS=46, LINE_COMMENT=47, 
		MULTILINE_COMMENT=48;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "PINID", "WHILE", "IF", "ELSE", "YIELD", 
		"RETURN", "CONTINUE", "BREAK", "BASE_DEVICE", "VAR", "CONST", "ADD", "SUB", 
		"MUL", "DIV", "MOD", "LT", "GT", "LE", "GE", "AND", "OR", "EQ", "NE", 
		"NEGATION", "ABS", "DEVICE", "DEVICE_WITH_ID", "BOOLEAN", "IDENTIFIER", 
		"INTEGER", "REAL", "WS", "LINE_COMMENT", "MULTILINE_COMMENT"
	};


	public Ic11Lexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public Ic11Lexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "';'", "'pin'", "'void'", "'real'", "'('", "','", "')'", "'{'", 
		"'}'", "'.'", "'='", "'['", "']'", null, "'while'", "'if'", "'else'", 
		"'yield'", "'return'", "'continue'", "'break'", "'Base'", "'var'", "'const'", 
		"'+'", "'-'", "'*'", "'/'", "'%'", "'<'", "'>'", "'<='", "'>='", "'&&'", 
		"'||'", "'=='", "'!='", "'!'", "'Abs'", "'Device'", "'DeviceWithId'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, "PINID", "WHILE", "IF", "ELSE", "YIELD", "RETURN", "CONTINUE", 
		"BREAK", "BASE_DEVICE", "VAR", "CONST", "ADD", "SUB", "MUL", "DIV", "MOD", 
		"LT", "GT", "LE", "GE", "AND", "OR", "EQ", "NE", "NEGATION", "ABS", "DEVICE", 
		"DEVICE_WITH_ID", "BOOLEAN", "IDENTIFIER", "INTEGER", "REAL", "WS", "LINE_COMMENT", 
		"MULTILINE_COMMENT"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Ic11.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static Ic11Lexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,48,330,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,1,0,1,0,1,1,1,1,
		1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,5,1,5,1,6,1,
		6,1,7,1,7,1,8,1,8,1,9,1,9,1,10,1,10,1,11,1,11,1,12,1,12,1,13,1,13,1,13,
		1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,3,13,146,8,13,1,
		14,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,16,1,
		17,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,18,1,18,1,19,1,
		19,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,1,20,1,20,1,20,1,
		21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,23,1,23,1,
		23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,1,27,1,28,1,28,1,29,1,29,1,30,1,
		30,1,31,1,31,1,31,1,32,1,32,1,32,1,33,1,33,1,33,1,34,1,34,1,34,1,35,1,
		35,1,35,1,36,1,36,1,36,1,37,1,37,1,38,1,38,1,38,1,38,1,39,1,39,1,39,1,
		39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,
		40,1,40,1,40,1,41,1,41,1,41,1,41,1,41,1,41,1,41,1,41,1,41,3,41,272,8,41,
		1,42,1,42,5,42,276,8,42,10,42,12,42,279,9,42,1,43,4,43,282,8,43,11,43,
		12,43,283,1,44,5,44,287,8,44,10,44,12,44,290,9,44,1,44,1,44,4,44,294,8,
		44,11,44,12,44,295,1,45,4,45,299,8,45,11,45,12,45,300,1,45,1,45,1,46,1,
		46,1,46,1,46,5,46,309,8,46,10,46,12,46,312,9,46,1,46,1,46,1,47,1,47,1,
		47,1,47,1,47,5,47,321,8,47,10,47,12,47,324,9,47,1,47,1,47,1,47,1,47,1,
		47,1,322,0,48,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,
		25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,
		49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,
		73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,47,95,48,
		1,0,5,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,1,0,48,57,3,
		0,9,10,13,13,32,32,2,0,10,10,13,13,344,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,
		0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,
		1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,
		0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,
		1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,
		0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,
		1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,
		0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,
		1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,
		0,0,95,1,0,0,0,1,97,1,0,0,0,3,99,1,0,0,0,5,103,1,0,0,0,7,108,1,0,0,0,9,
		113,1,0,0,0,11,115,1,0,0,0,13,117,1,0,0,0,15,119,1,0,0,0,17,121,1,0,0,
		0,19,123,1,0,0,0,21,125,1,0,0,0,23,127,1,0,0,0,25,129,1,0,0,0,27,145,1,
		0,0,0,29,147,1,0,0,0,31,153,1,0,0,0,33,156,1,0,0,0,35,161,1,0,0,0,37,167,
		1,0,0,0,39,174,1,0,0,0,41,183,1,0,0,0,43,189,1,0,0,0,45,194,1,0,0,0,47,
		198,1,0,0,0,49,204,1,0,0,0,51,206,1,0,0,0,53,208,1,0,0,0,55,210,1,0,0,
		0,57,212,1,0,0,0,59,214,1,0,0,0,61,216,1,0,0,0,63,218,1,0,0,0,65,221,1,
		0,0,0,67,224,1,0,0,0,69,227,1,0,0,0,71,230,1,0,0,0,73,233,1,0,0,0,75,236,
		1,0,0,0,77,238,1,0,0,0,79,242,1,0,0,0,81,249,1,0,0,0,83,271,1,0,0,0,85,
		273,1,0,0,0,87,281,1,0,0,0,89,288,1,0,0,0,91,298,1,0,0,0,93,304,1,0,0,
		0,95,315,1,0,0,0,97,98,5,59,0,0,98,2,1,0,0,0,99,100,5,112,0,0,100,101,
		5,105,0,0,101,102,5,110,0,0,102,4,1,0,0,0,103,104,5,118,0,0,104,105,5,
		111,0,0,105,106,5,105,0,0,106,107,5,100,0,0,107,6,1,0,0,0,108,109,5,114,
		0,0,109,110,5,101,0,0,110,111,5,97,0,0,111,112,5,108,0,0,112,8,1,0,0,0,
		113,114,5,40,0,0,114,10,1,0,0,0,115,116,5,44,0,0,116,12,1,0,0,0,117,118,
		5,41,0,0,118,14,1,0,0,0,119,120,5,123,0,0,120,16,1,0,0,0,121,122,5,125,
		0,0,122,18,1,0,0,0,123,124,5,46,0,0,124,20,1,0,0,0,125,126,5,61,0,0,126,
		22,1,0,0,0,127,128,5,91,0,0,128,24,1,0,0,0,129,130,5,93,0,0,130,26,1,0,
		0,0,131,132,5,100,0,0,132,146,5,98,0,0,133,134,5,100,0,0,134,146,5,48,
		0,0,135,136,5,100,0,0,136,146,5,49,0,0,137,138,5,100,0,0,138,146,5,50,
		0,0,139,140,5,100,0,0,140,146,5,51,0,0,141,142,5,100,0,0,142,146,5,52,
		0,0,143,144,5,100,0,0,144,146,5,53,0,0,145,131,1,0,0,0,145,133,1,0,0,0,
		145,135,1,0,0,0,145,137,1,0,0,0,145,139,1,0,0,0,145,141,1,0,0,0,145,143,
		1,0,0,0,146,28,1,0,0,0,147,148,5,119,0,0,148,149,5,104,0,0,149,150,5,105,
		0,0,150,151,5,108,0,0,151,152,5,101,0,0,152,30,1,0,0,0,153,154,5,105,0,
		0,154,155,5,102,0,0,155,32,1,0,0,0,156,157,5,101,0,0,157,158,5,108,0,0,
		158,159,5,115,0,0,159,160,5,101,0,0,160,34,1,0,0,0,161,162,5,121,0,0,162,
		163,5,105,0,0,163,164,5,101,0,0,164,165,5,108,0,0,165,166,5,100,0,0,166,
		36,1,0,0,0,167,168,5,114,0,0,168,169,5,101,0,0,169,170,5,116,0,0,170,171,
		5,117,0,0,171,172,5,114,0,0,172,173,5,110,0,0,173,38,1,0,0,0,174,175,5,
		99,0,0,175,176,5,111,0,0,176,177,5,110,0,0,177,178,5,116,0,0,178,179,5,
		105,0,0,179,180,5,110,0,0,180,181,5,117,0,0,181,182,5,101,0,0,182,40,1,
		0,0,0,183,184,5,98,0,0,184,185,5,114,0,0,185,186,5,101,0,0,186,187,5,97,
		0,0,187,188,5,107,0,0,188,42,1,0,0,0,189,190,5,66,0,0,190,191,5,97,0,0,
		191,192,5,115,0,0,192,193,5,101,0,0,193,44,1,0,0,0,194,195,5,118,0,0,195,
		196,5,97,0,0,196,197,5,114,0,0,197,46,1,0,0,0,198,199,5,99,0,0,199,200,
		5,111,0,0,200,201,5,110,0,0,201,202,5,115,0,0,202,203,5,116,0,0,203,48,
		1,0,0,0,204,205,5,43,0,0,205,50,1,0,0,0,206,207,5,45,0,0,207,52,1,0,0,
		0,208,209,5,42,0,0,209,54,1,0,0,0,210,211,5,47,0,0,211,56,1,0,0,0,212,
		213,5,37,0,0,213,58,1,0,0,0,214,215,5,60,0,0,215,60,1,0,0,0,216,217,5,
		62,0,0,217,62,1,0,0,0,218,219,5,60,0,0,219,220,5,61,0,0,220,64,1,0,0,0,
		221,222,5,62,0,0,222,223,5,61,0,0,223,66,1,0,0,0,224,225,5,38,0,0,225,
		226,5,38,0,0,226,68,1,0,0,0,227,228,5,124,0,0,228,229,5,124,0,0,229,70,
		1,0,0,0,230,231,5,61,0,0,231,232,5,61,0,0,232,72,1,0,0,0,233,234,5,33,
		0,0,234,235,5,61,0,0,235,74,1,0,0,0,236,237,5,33,0,0,237,76,1,0,0,0,238,
		239,5,65,0,0,239,240,5,98,0,0,240,241,5,115,0,0,241,78,1,0,0,0,242,243,
		5,68,0,0,243,244,5,101,0,0,244,245,5,118,0,0,245,246,5,105,0,0,246,247,
		5,99,0,0,247,248,5,101,0,0,248,80,1,0,0,0,249,250,5,68,0,0,250,251,5,101,
		0,0,251,252,5,118,0,0,252,253,5,105,0,0,253,254,5,99,0,0,254,255,5,101,
		0,0,255,256,5,87,0,0,256,257,5,105,0,0,257,258,5,116,0,0,258,259,5,104,
		0,0,259,260,5,73,0,0,260,261,5,100,0,0,261,82,1,0,0,0,262,263,5,116,0,
		0,263,264,5,114,0,0,264,265,5,117,0,0,265,272,5,101,0,0,266,267,5,102,
		0,0,267,268,5,97,0,0,268,269,5,108,0,0,269,270,5,115,0,0,270,272,5,101,
		0,0,271,262,1,0,0,0,271,266,1,0,0,0,272,84,1,0,0,0,273,277,7,0,0,0,274,
		276,7,1,0,0,275,274,1,0,0,0,276,279,1,0,0,0,277,275,1,0,0,0,277,278,1,
		0,0,0,278,86,1,0,0,0,279,277,1,0,0,0,280,282,7,2,0,0,281,280,1,0,0,0,282,
		283,1,0,0,0,283,281,1,0,0,0,283,284,1,0,0,0,284,88,1,0,0,0,285,287,7,2,
		0,0,286,285,1,0,0,0,287,290,1,0,0,0,288,286,1,0,0,0,288,289,1,0,0,0,289,
		291,1,0,0,0,290,288,1,0,0,0,291,293,5,46,0,0,292,294,7,2,0,0,293,292,1,
		0,0,0,294,295,1,0,0,0,295,293,1,0,0,0,295,296,1,0,0,0,296,90,1,0,0,0,297,
		299,7,3,0,0,298,297,1,0,0,0,299,300,1,0,0,0,300,298,1,0,0,0,300,301,1,
		0,0,0,301,302,1,0,0,0,302,303,6,45,0,0,303,92,1,0,0,0,304,305,5,47,0,0,
		305,306,5,47,0,0,306,310,1,0,0,0,307,309,8,4,0,0,308,307,1,0,0,0,309,312,
		1,0,0,0,310,308,1,0,0,0,310,311,1,0,0,0,311,313,1,0,0,0,312,310,1,0,0,
		0,313,314,6,46,0,0,314,94,1,0,0,0,315,316,5,47,0,0,316,317,5,42,0,0,317,
		322,1,0,0,0,318,321,3,95,47,0,319,321,9,0,0,0,320,318,1,0,0,0,320,319,
		1,0,0,0,321,324,1,0,0,0,322,323,1,0,0,0,322,320,1,0,0,0,323,325,1,0,0,
		0,324,322,1,0,0,0,325,326,5,42,0,0,326,327,5,47,0,0,327,328,1,0,0,0,328,
		329,6,47,0,0,329,96,1,0,0,0,11,0,145,271,277,283,288,295,300,310,320,322,
		1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
