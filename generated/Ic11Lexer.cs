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
		PINID=10, WHILE=11, IF=12, ELSE=13, YIELD=14, RETURN=15, VAR=16, ADD=17, 
		SUB=18, MUL=19, DIV=20, LT=21, GT=22, LE=23, GE=24, AND=25, OR=26, NEGATION=27, 
		BOOLEAN=28, IDENTIFIER=29, INTEGER=30, REAL=31, WS=32, COMMENT=33;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"PINID", "WHILE", "IF", "ELSE", "YIELD", "RETURN", "VAR", "ADD", "SUB", 
		"MUL", "DIV", "LT", "GT", "LE", "GE", "AND", "OR", "NEGATION", "BOOLEAN", 
		"IDENTIFIER", "INTEGER", "REAL", "WS", "COMMENT"
	};


	public Ic11Lexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public Ic11Lexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'pin'", "';'", "'void'", "'('", "')'", "'{'", "'}'", "'.'", "'='", 
		null, "'while'", "'if'", "'else'", "'yield'", "'return'", "'var'", "'+'", 
		"'-'", "'*'", "'/'", "'<'", "'>'", "'<='", "'>='", "'&&'", "'||'", "'!'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, "PINID", "WHILE", 
		"IF", "ELSE", "YIELD", "RETURN", "VAR", "ADD", "SUB", "MUL", "DIV", "LT", 
		"GT", "LE", "GE", "AND", "OR", "NEGATION", "BOOLEAN", "IDENTIFIER", "INTEGER", 
		"REAL", "WS", "COMMENT"
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
		4,0,33,216,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,1,0,1,0,1,0,1,0,1,1,1,1,1,
		2,1,2,1,2,1,2,1,2,1,3,1,3,1,4,1,4,1,5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,
		1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,3,9,105,8,9,1,10,1,
		10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,12,1,12,1,12,1,12,1,12,1,13,1,
		13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,
		15,1,15,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,
		22,1,22,1,22,1,23,1,23,1,23,1,24,1,24,1,24,1,25,1,25,1,25,1,26,1,26,1,
		27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,3,27,173,8,27,1,28,1,28,5,28,
		177,8,28,10,28,12,28,180,9,28,1,29,4,29,183,8,29,11,29,12,29,184,1,30,
		5,30,188,8,30,10,30,12,30,191,9,30,1,30,1,30,4,30,195,8,30,11,30,12,30,
		196,1,31,4,31,200,8,31,11,31,12,31,201,1,31,1,31,1,32,1,32,1,32,1,32,5,
		32,210,8,32,10,32,12,32,213,9,32,1,32,1,32,0,0,33,1,1,3,2,5,3,7,4,9,5,
		11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,
		18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,
		30,61,31,63,32,65,33,1,0,5,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,
		97,122,1,0,48,57,3,0,9,10,13,13,32,32,2,0,10,10,13,13,228,0,1,1,0,0,0,
		0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,
		0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,
		25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,
		0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,
		0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,
		1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,1,67,1,0,0,
		0,3,71,1,0,0,0,5,73,1,0,0,0,7,78,1,0,0,0,9,80,1,0,0,0,11,82,1,0,0,0,13,
		84,1,0,0,0,15,86,1,0,0,0,17,88,1,0,0,0,19,104,1,0,0,0,21,106,1,0,0,0,23,
		112,1,0,0,0,25,115,1,0,0,0,27,120,1,0,0,0,29,126,1,0,0,0,31,133,1,0,0,
		0,33,137,1,0,0,0,35,139,1,0,0,0,37,141,1,0,0,0,39,143,1,0,0,0,41,145,1,
		0,0,0,43,147,1,0,0,0,45,149,1,0,0,0,47,152,1,0,0,0,49,155,1,0,0,0,51,158,
		1,0,0,0,53,161,1,0,0,0,55,172,1,0,0,0,57,174,1,0,0,0,59,182,1,0,0,0,61,
		189,1,0,0,0,63,199,1,0,0,0,65,205,1,0,0,0,67,68,5,112,0,0,68,69,5,105,
		0,0,69,70,5,110,0,0,70,2,1,0,0,0,71,72,5,59,0,0,72,4,1,0,0,0,73,74,5,118,
		0,0,74,75,5,111,0,0,75,76,5,105,0,0,76,77,5,100,0,0,77,6,1,0,0,0,78,79,
		5,40,0,0,79,8,1,0,0,0,80,81,5,41,0,0,81,10,1,0,0,0,82,83,5,123,0,0,83,
		12,1,0,0,0,84,85,5,125,0,0,85,14,1,0,0,0,86,87,5,46,0,0,87,16,1,0,0,0,
		88,89,5,61,0,0,89,18,1,0,0,0,90,91,5,100,0,0,91,105,5,98,0,0,92,93,5,100,
		0,0,93,105,5,48,0,0,94,95,5,100,0,0,95,105,5,49,0,0,96,97,5,100,0,0,97,
		105,5,50,0,0,98,99,5,100,0,0,99,105,5,51,0,0,100,101,5,100,0,0,101,105,
		5,52,0,0,102,103,5,100,0,0,103,105,5,53,0,0,104,90,1,0,0,0,104,92,1,0,
		0,0,104,94,1,0,0,0,104,96,1,0,0,0,104,98,1,0,0,0,104,100,1,0,0,0,104,102,
		1,0,0,0,105,20,1,0,0,0,106,107,5,119,0,0,107,108,5,104,0,0,108,109,5,105,
		0,0,109,110,5,108,0,0,110,111,5,101,0,0,111,22,1,0,0,0,112,113,5,105,0,
		0,113,114,5,102,0,0,114,24,1,0,0,0,115,116,5,101,0,0,116,117,5,108,0,0,
		117,118,5,115,0,0,118,119,5,101,0,0,119,26,1,0,0,0,120,121,5,121,0,0,121,
		122,5,105,0,0,122,123,5,101,0,0,123,124,5,108,0,0,124,125,5,100,0,0,125,
		28,1,0,0,0,126,127,5,114,0,0,127,128,5,101,0,0,128,129,5,116,0,0,129,130,
		5,117,0,0,130,131,5,114,0,0,131,132,5,110,0,0,132,30,1,0,0,0,133,134,5,
		118,0,0,134,135,5,97,0,0,135,136,5,114,0,0,136,32,1,0,0,0,137,138,5,43,
		0,0,138,34,1,0,0,0,139,140,5,45,0,0,140,36,1,0,0,0,141,142,5,42,0,0,142,
		38,1,0,0,0,143,144,5,47,0,0,144,40,1,0,0,0,145,146,5,60,0,0,146,42,1,0,
		0,0,147,148,5,62,0,0,148,44,1,0,0,0,149,150,5,60,0,0,150,151,5,61,0,0,
		151,46,1,0,0,0,152,153,5,62,0,0,153,154,5,61,0,0,154,48,1,0,0,0,155,156,
		5,38,0,0,156,157,5,38,0,0,157,50,1,0,0,0,158,159,5,124,0,0,159,160,5,124,
		0,0,160,52,1,0,0,0,161,162,5,33,0,0,162,54,1,0,0,0,163,164,5,116,0,0,164,
		165,5,114,0,0,165,166,5,117,0,0,166,173,5,101,0,0,167,168,5,102,0,0,168,
		169,5,97,0,0,169,170,5,108,0,0,170,171,5,115,0,0,171,173,5,101,0,0,172,
		163,1,0,0,0,172,167,1,0,0,0,173,56,1,0,0,0,174,178,7,0,0,0,175,177,7,1,
		0,0,176,175,1,0,0,0,177,180,1,0,0,0,178,176,1,0,0,0,178,179,1,0,0,0,179,
		58,1,0,0,0,180,178,1,0,0,0,181,183,7,2,0,0,182,181,1,0,0,0,183,184,1,0,
		0,0,184,182,1,0,0,0,184,185,1,0,0,0,185,60,1,0,0,0,186,188,7,2,0,0,187,
		186,1,0,0,0,188,191,1,0,0,0,189,187,1,0,0,0,189,190,1,0,0,0,190,192,1,
		0,0,0,191,189,1,0,0,0,192,194,5,46,0,0,193,195,7,2,0,0,194,193,1,0,0,0,
		195,196,1,0,0,0,196,194,1,0,0,0,196,197,1,0,0,0,197,62,1,0,0,0,198,200,
		7,3,0,0,199,198,1,0,0,0,200,201,1,0,0,0,201,199,1,0,0,0,201,202,1,0,0,
		0,202,203,1,0,0,0,203,204,6,31,0,0,204,64,1,0,0,0,205,206,5,47,0,0,206,
		207,5,47,0,0,207,211,1,0,0,0,208,210,8,4,0,0,209,208,1,0,0,0,210,213,1,
		0,0,0,211,209,1,0,0,0,211,212,1,0,0,0,212,214,1,0,0,0,213,211,1,0,0,0,
		214,215,6,32,0,0,215,66,1,0,0,0,9,0,104,172,178,184,189,196,201,211,1,
		6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
