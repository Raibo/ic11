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
		WHILE=10, IF=11, ELSE=12, YIELD=13, RETURN=14, VAR=15, PLUS=16, MINUS=17, 
		MUL=18, DIV=19, LT=20, GT=21, LE=22, GE=23, AND=24, OR=25, NEGATION=26, 
		IDENTIFIER=27, INTEGER=28, BOOLEAN=29, REAL=30, WS=31, COMMENT=32;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"WHILE", "IF", "ELSE", "YIELD", "RETURN", "VAR", "PLUS", "MINUS", "MUL", 
		"DIV", "LT", "GT", "LE", "GE", "AND", "OR", "NEGATION", "IDENTIFIER", 
		"INTEGER", "BOOLEAN", "REAL", "WS", "COMMENT"
	};


	public Ic11Lexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public Ic11Lexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'Pin'", "'='", "';'", "'void'", "'('", "')'", "'{'", "'}'", "'.'", 
		"'while'", "'if'", "'else'", "'yield'", "'return'", "'var'", "'+'", "'-'", 
		"'*'", "'/'", "'<'", "'>'", "'<='", "'>='", "'&&'", "'||'", "'!'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, "WHILE", "IF", 
		"ELSE", "YIELD", "RETURN", "VAR", "PLUS", "MINUS", "MUL", "DIV", "LT", 
		"GT", "LE", "GE", "AND", "OR", "NEGATION", "IDENTIFIER", "INTEGER", "BOOLEAN", 
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
		4,0,32,198,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,1,0,1,0,1,0,1,0,1,1,1,1,1,2,1,2,1,3,
		1,3,1,3,1,3,1,3,1,4,1,4,1,5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,1,9,1,9,1,
		9,1,9,1,9,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,12,
		1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,15,
		1,15,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,21,
		1,22,1,22,1,22,1,23,1,23,1,23,1,24,1,24,1,24,1,25,1,25,1,26,1,26,5,26,
		148,8,26,10,26,12,26,151,9,26,1,27,4,27,154,8,27,11,27,12,27,155,1,28,
		1,28,1,28,1,28,1,28,1,28,1,28,1,28,1,28,3,28,167,8,28,1,29,5,29,170,8,
		29,10,29,12,29,173,9,29,1,29,1,29,4,29,177,8,29,11,29,12,29,178,1,30,4,
		30,182,8,30,11,30,12,30,183,1,30,1,30,1,31,1,31,1,31,1,31,5,31,192,8,31,
		10,31,12,31,195,9,31,1,31,1,31,0,0,32,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,
		8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,
		20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,
		32,1,0,5,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,1,0,48,57,
		3,0,9,10,13,13,32,32,2,0,10,10,13,13,204,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,
		0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,
		17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,
		0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,
		0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,
		1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,
		0,0,61,1,0,0,0,0,63,1,0,0,0,1,65,1,0,0,0,3,69,1,0,0,0,5,71,1,0,0,0,7,73,
		1,0,0,0,9,78,1,0,0,0,11,80,1,0,0,0,13,82,1,0,0,0,15,84,1,0,0,0,17,86,1,
		0,0,0,19,88,1,0,0,0,21,94,1,0,0,0,23,97,1,0,0,0,25,102,1,0,0,0,27,108,
		1,0,0,0,29,115,1,0,0,0,31,119,1,0,0,0,33,121,1,0,0,0,35,123,1,0,0,0,37,
		125,1,0,0,0,39,127,1,0,0,0,41,129,1,0,0,0,43,131,1,0,0,0,45,134,1,0,0,
		0,47,137,1,0,0,0,49,140,1,0,0,0,51,143,1,0,0,0,53,145,1,0,0,0,55,153,1,
		0,0,0,57,166,1,0,0,0,59,171,1,0,0,0,61,181,1,0,0,0,63,187,1,0,0,0,65,66,
		5,80,0,0,66,67,5,105,0,0,67,68,5,110,0,0,68,2,1,0,0,0,69,70,5,61,0,0,70,
		4,1,0,0,0,71,72,5,59,0,0,72,6,1,0,0,0,73,74,5,118,0,0,74,75,5,111,0,0,
		75,76,5,105,0,0,76,77,5,100,0,0,77,8,1,0,0,0,78,79,5,40,0,0,79,10,1,0,
		0,0,80,81,5,41,0,0,81,12,1,0,0,0,82,83,5,123,0,0,83,14,1,0,0,0,84,85,5,
		125,0,0,85,16,1,0,0,0,86,87,5,46,0,0,87,18,1,0,0,0,88,89,5,119,0,0,89,
		90,5,104,0,0,90,91,5,105,0,0,91,92,5,108,0,0,92,93,5,101,0,0,93,20,1,0,
		0,0,94,95,5,105,0,0,95,96,5,102,0,0,96,22,1,0,0,0,97,98,5,101,0,0,98,99,
		5,108,0,0,99,100,5,115,0,0,100,101,5,101,0,0,101,24,1,0,0,0,102,103,5,
		121,0,0,103,104,5,105,0,0,104,105,5,101,0,0,105,106,5,108,0,0,106,107,
		5,100,0,0,107,26,1,0,0,0,108,109,5,114,0,0,109,110,5,101,0,0,110,111,5,
		116,0,0,111,112,5,117,0,0,112,113,5,114,0,0,113,114,5,110,0,0,114,28,1,
		0,0,0,115,116,5,118,0,0,116,117,5,97,0,0,117,118,5,114,0,0,118,30,1,0,
		0,0,119,120,5,43,0,0,120,32,1,0,0,0,121,122,5,45,0,0,122,34,1,0,0,0,123,
		124,5,42,0,0,124,36,1,0,0,0,125,126,5,47,0,0,126,38,1,0,0,0,127,128,5,
		60,0,0,128,40,1,0,0,0,129,130,5,62,0,0,130,42,1,0,0,0,131,132,5,60,0,0,
		132,133,5,61,0,0,133,44,1,0,0,0,134,135,5,62,0,0,135,136,5,61,0,0,136,
		46,1,0,0,0,137,138,5,38,0,0,138,139,5,38,0,0,139,48,1,0,0,0,140,141,5,
		124,0,0,141,142,5,124,0,0,142,50,1,0,0,0,143,144,5,33,0,0,144,52,1,0,0,
		0,145,149,7,0,0,0,146,148,7,1,0,0,147,146,1,0,0,0,148,151,1,0,0,0,149,
		147,1,0,0,0,149,150,1,0,0,0,150,54,1,0,0,0,151,149,1,0,0,0,152,154,7,2,
		0,0,153,152,1,0,0,0,154,155,1,0,0,0,155,153,1,0,0,0,155,156,1,0,0,0,156,
		56,1,0,0,0,157,158,5,116,0,0,158,159,5,114,0,0,159,160,5,117,0,0,160,167,
		5,101,0,0,161,162,5,102,0,0,162,163,5,97,0,0,163,164,5,108,0,0,164,165,
		5,115,0,0,165,167,5,101,0,0,166,157,1,0,0,0,166,161,1,0,0,0,167,58,1,0,
		0,0,168,170,7,2,0,0,169,168,1,0,0,0,170,173,1,0,0,0,171,169,1,0,0,0,171,
		172,1,0,0,0,172,174,1,0,0,0,173,171,1,0,0,0,174,176,5,46,0,0,175,177,7,
		2,0,0,176,175,1,0,0,0,177,178,1,0,0,0,178,176,1,0,0,0,178,179,1,0,0,0,
		179,60,1,0,0,0,180,182,7,3,0,0,181,180,1,0,0,0,182,183,1,0,0,0,183,181,
		1,0,0,0,183,184,1,0,0,0,184,185,1,0,0,0,185,186,6,30,0,0,186,62,1,0,0,
		0,187,188,5,47,0,0,188,189,5,47,0,0,189,193,1,0,0,0,190,192,8,4,0,0,191,
		190,1,0,0,0,192,195,1,0,0,0,193,191,1,0,0,0,193,194,1,0,0,0,194,196,1,
		0,0,0,195,193,1,0,0,0,196,197,6,31,0,0,197,64,1,0,0,0,8,0,149,155,166,
		171,178,183,193,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
