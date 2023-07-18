//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ../ScratchScript.g4 by ANTLR 4.12.0

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

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
[System.CLSCompliant(false)]
public partial class ScratchScriptLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		Whitespace=1, NewLine=2, Semicolon=3, LeftParen=4, RightParen=5, LeftBracket=6, 
		RightBracket=7, LeftBrace=8, RightBrace=9, Assignment=10, Comma=11, Not=12, 
		Arrow=13, Colon=14, SingleLineCommentStart=15, MultiLineCommentStart=16, 
		MultiLineCommentEnd=17, Comment=18, At=19, Hashtag=20, Multiply=21, Plus=22, 
		Minus=23, Divide=24, Power=25, Modulus=26, And=27, Or=28, Xor=29, EmptyArray=30, 
		Greater=31, Lesser=32, GreaterOrEqual=33, LesserOrEqual=34, Equal=35, 
		NotEqual=36, AdditionAsignment=37, SubtractionAssignment=38, MultiplicationAssignment=39, 
		DivisionAssignment=40, ModulusAssignment=41, If=42, Else=43, True=44, 
		False=45, Break=46, Continue=47, While=48, VariableDeclare=49, Import=50, 
		ProcedureDeclare=51, Return=52, Repeat=53, Event=54, From=55, Namespace=56, 
		Number=57, Identifier=58, String=59, Color=60;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"Digit", "HexDigit", "Whitespace", "NewLine", "Semicolon", "LeftParen", 
		"RightParen", "LeftBracket", "RightBracket", "LeftBrace", "RightBrace", 
		"Assignment", "Comma", "Not", "Arrow", "Colon", "SingleLineCommentStart", 
		"MultiLineCommentStart", "MultiLineCommentEnd", "Comment", "At", "Hashtag", 
		"Multiply", "Plus", "Minus", "Divide", "Power", "Modulus", "And", "Or", 
		"Xor", "EmptyArray", "Greater", "Lesser", "GreaterOrEqual", "LesserOrEqual", 
		"Equal", "NotEqual", "AdditionAsignment", "SubtractionAssignment", "MultiplicationAssignment", 
		"DivisionAssignment", "ModulusAssignment", "If", "Else", "True", "False", 
		"Break", "Continue", "While", "VariableDeclare", "Import", "ProcedureDeclare", 
		"Return", "Repeat", "Event", "From", "Namespace", "Number", "Identifier", 
		"String", "Color"
	};


	public ScratchScriptLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ScratchScriptLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, "';'", "'('", "')'", "'['", "']'", "'{'", "'}'", "'='", 
		"','", "'!'", "'->'", "':'", "'//'", "'/*'", "'*/'", null, "'@'", "'#'", 
		"'*'", "'+'", "'-'", "'/'", "'**'", "'%'", "'&&'", "'||'", "'^'", "'[]'", 
		"'>'", "'<'", "'>='", "'<='", "'=='", "'!='", "'+='", "'-='", "'*='", 
		"'/='", "'%='", null, "'else'", "'true'", "'false'", "'break'", "'continue'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "Whitespace", "NewLine", "Semicolon", "LeftParen", "RightParen", 
		"LeftBracket", "RightBracket", "LeftBrace", "RightBrace", "Assignment", 
		"Comma", "Not", "Arrow", "Colon", "SingleLineCommentStart", "MultiLineCommentStart", 
		"MultiLineCommentEnd", "Comment", "At", "Hashtag", "Multiply", "Plus", 
		"Minus", "Divide", "Power", "Modulus", "And", "Or", "Xor", "EmptyArray", 
		"Greater", "Lesser", "GreaterOrEqual", "LesserOrEqual", "Equal", "NotEqual", 
		"AdditionAsignment", "SubtractionAssignment", "MultiplicationAssignment", 
		"DivisionAssignment", "ModulusAssignment", "If", "Else", "True", "False", 
		"Break", "Continue", "While", "VariableDeclare", "Import", "ProcedureDeclare", 
		"Return", "Repeat", "Event", "From", "Namespace", "Number", "Identifier", 
		"String", "Color"
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

	public override string GrammarFileName { get { return "ScratchScript.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ScratchScriptLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,60,442,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,1,0,1,0,1,1,1,1,
		1,2,1,2,1,2,1,2,1,3,3,3,135,8,3,1,3,1,3,3,3,139,8,3,1,3,1,3,1,4,1,4,1,
		5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,1,9,1,10,1,10,1,11,1,11,1,12,1,12,1,
		13,1,13,1,14,1,14,1,14,1,15,1,15,1,16,1,16,1,16,1,17,1,17,1,17,1,18,1,
		18,1,18,1,19,1,19,1,19,5,19,180,8,19,10,19,12,19,183,9,19,1,19,1,19,5,
		19,187,8,19,10,19,12,19,190,9,19,1,19,1,19,3,19,194,8,19,1,20,1,20,1,21,
		1,21,1,22,1,22,1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,26,1,27,1,27,
		1,28,1,28,1,28,1,29,1,29,1,29,1,30,1,30,1,31,1,31,1,31,1,32,1,32,1,33,
		1,33,1,34,1,34,1,34,1,35,1,35,1,35,1,36,1,36,1,36,1,37,1,37,1,37,1,38,
		1,38,1,38,1,39,1,39,1,39,1,40,1,40,1,40,1,41,1,41,1,41,1,42,1,42,1,42,
		1,43,1,43,1,43,1,43,4,43,259,8,43,11,43,12,43,260,1,44,1,44,1,44,1,44,
		1,44,1,45,1,45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,46,1,47,1,47,
		1,47,1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,49,
		1,49,1,49,1,49,1,49,1,49,1,49,4,49,301,8,49,11,49,12,49,302,1,50,1,50,
		1,50,1,50,1,50,4,50,310,8,50,11,50,12,50,311,1,51,1,51,1,51,1,51,1,51,
		1,51,1,51,1,51,4,51,322,8,51,11,51,12,51,323,1,52,1,52,1,52,1,52,1,52,
		1,52,1,52,1,52,1,52,1,52,4,52,336,8,52,11,52,12,52,337,1,53,1,53,1,53,
		1,53,1,53,1,53,1,53,1,53,4,53,348,8,53,11,53,12,53,349,1,54,1,54,1,54,
		1,54,1,54,1,54,1,54,1,54,4,54,360,8,54,11,54,12,54,361,1,55,1,55,1,55,
		1,55,4,55,368,8,55,11,55,12,55,369,1,56,1,56,1,56,1,56,1,56,1,56,4,56,
		378,8,56,11,56,12,56,379,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,57,
		1,57,1,57,4,57,393,8,57,11,57,12,57,394,1,58,4,58,398,8,58,11,58,12,58,
		399,1,58,1,58,4,58,404,8,58,11,58,12,58,405,3,58,408,8,58,1,59,1,59,5,
		59,412,8,59,10,59,12,59,415,9,59,1,60,1,60,5,60,419,8,60,10,60,12,60,422,
		9,60,1,60,1,60,1,60,5,60,427,8,60,10,60,12,60,430,9,60,1,60,3,60,433,8,
		60,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,61,1,188,0,62,1,0,3,0,5,1,7,2,
		9,3,11,4,13,5,15,6,17,7,19,8,21,9,23,10,25,11,27,12,29,13,31,14,33,15,
		35,16,37,17,39,18,41,19,43,20,45,21,47,22,49,23,51,24,53,25,55,26,57,27,
		59,28,61,29,63,30,65,31,67,32,69,33,71,34,73,35,75,36,77,37,79,38,81,39,
		83,40,85,41,87,42,89,43,91,44,93,45,95,46,97,47,99,48,101,49,103,50,105,
		51,107,52,109,53,111,54,113,55,115,56,117,57,119,58,121,59,123,60,1,0,
		9,1,0,48,57,2,0,48,57,65,70,2,0,9,9,32,32,2,0,10,10,13,13,1,0,46,46,3,
		0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,1,0,34,34,1,0,39,39,
		462,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,
		1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,
		0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,
		1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,
		0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,
		1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,
		0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,
		1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,
		0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,
		103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,
		113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,
		123,1,0,0,0,1,125,1,0,0,0,3,127,1,0,0,0,5,129,1,0,0,0,7,138,1,0,0,0,9,
		142,1,0,0,0,11,144,1,0,0,0,13,146,1,0,0,0,15,148,1,0,0,0,17,150,1,0,0,
		0,19,152,1,0,0,0,21,154,1,0,0,0,23,156,1,0,0,0,25,158,1,0,0,0,27,160,1,
		0,0,0,29,162,1,0,0,0,31,165,1,0,0,0,33,167,1,0,0,0,35,170,1,0,0,0,37,173,
		1,0,0,0,39,193,1,0,0,0,41,195,1,0,0,0,43,197,1,0,0,0,45,199,1,0,0,0,47,
		201,1,0,0,0,49,203,1,0,0,0,51,205,1,0,0,0,53,207,1,0,0,0,55,210,1,0,0,
		0,57,212,1,0,0,0,59,215,1,0,0,0,61,218,1,0,0,0,63,220,1,0,0,0,65,223,1,
		0,0,0,67,225,1,0,0,0,69,227,1,0,0,0,71,230,1,0,0,0,73,233,1,0,0,0,75,236,
		1,0,0,0,77,239,1,0,0,0,79,242,1,0,0,0,81,245,1,0,0,0,83,248,1,0,0,0,85,
		251,1,0,0,0,87,254,1,0,0,0,89,262,1,0,0,0,91,267,1,0,0,0,93,272,1,0,0,
		0,95,278,1,0,0,0,97,284,1,0,0,0,99,293,1,0,0,0,101,304,1,0,0,0,103,313,
		1,0,0,0,105,325,1,0,0,0,107,339,1,0,0,0,109,351,1,0,0,0,111,363,1,0,0,
		0,113,371,1,0,0,0,115,381,1,0,0,0,117,397,1,0,0,0,119,409,1,0,0,0,121,
		432,1,0,0,0,123,434,1,0,0,0,125,126,7,0,0,0,126,2,1,0,0,0,127,128,7,1,
		0,0,128,4,1,0,0,0,129,130,7,2,0,0,130,131,1,0,0,0,131,132,6,2,0,0,132,
		6,1,0,0,0,133,135,5,13,0,0,134,133,1,0,0,0,134,135,1,0,0,0,135,136,1,0,
		0,0,136,139,5,10,0,0,137,139,7,3,0,0,138,134,1,0,0,0,138,137,1,0,0,0,139,
		140,1,0,0,0,140,141,6,3,1,0,141,8,1,0,0,0,142,143,5,59,0,0,143,10,1,0,
		0,0,144,145,5,40,0,0,145,12,1,0,0,0,146,147,5,41,0,0,147,14,1,0,0,0,148,
		149,5,91,0,0,149,16,1,0,0,0,150,151,5,93,0,0,151,18,1,0,0,0,152,153,5,
		123,0,0,153,20,1,0,0,0,154,155,5,125,0,0,155,22,1,0,0,0,156,157,5,61,0,
		0,157,24,1,0,0,0,158,159,5,44,0,0,159,26,1,0,0,0,160,161,5,33,0,0,161,
		28,1,0,0,0,162,163,5,45,0,0,163,164,5,62,0,0,164,30,1,0,0,0,165,166,5,
		58,0,0,166,32,1,0,0,0,167,168,5,47,0,0,168,169,5,47,0,0,169,34,1,0,0,0,
		170,171,5,47,0,0,171,172,5,42,0,0,172,36,1,0,0,0,173,174,5,42,0,0,174,
		175,5,47,0,0,175,38,1,0,0,0,176,181,3,33,16,0,177,180,8,3,0,0,178,180,
		3,5,2,0,179,177,1,0,0,0,179,178,1,0,0,0,180,183,1,0,0,0,181,179,1,0,0,
		0,181,182,1,0,0,0,182,194,1,0,0,0,183,181,1,0,0,0,184,188,3,35,17,0,185,
		187,9,0,0,0,186,185,1,0,0,0,187,190,1,0,0,0,188,189,1,0,0,0,188,186,1,
		0,0,0,189,191,1,0,0,0,190,188,1,0,0,0,191,192,3,37,18,0,192,194,1,0,0,
		0,193,176,1,0,0,0,193,184,1,0,0,0,194,40,1,0,0,0,195,196,5,64,0,0,196,
		42,1,0,0,0,197,198,5,35,0,0,198,44,1,0,0,0,199,200,5,42,0,0,200,46,1,0,
		0,0,201,202,5,43,0,0,202,48,1,0,0,0,203,204,5,45,0,0,204,50,1,0,0,0,205,
		206,5,47,0,0,206,52,1,0,0,0,207,208,5,42,0,0,208,209,5,42,0,0,209,54,1,
		0,0,0,210,211,5,37,0,0,211,56,1,0,0,0,212,213,5,38,0,0,213,214,5,38,0,
		0,214,58,1,0,0,0,215,216,5,124,0,0,216,217,5,124,0,0,217,60,1,0,0,0,218,
		219,5,94,0,0,219,62,1,0,0,0,220,221,5,91,0,0,221,222,5,93,0,0,222,64,1,
		0,0,0,223,224,5,62,0,0,224,66,1,0,0,0,225,226,5,60,0,0,226,68,1,0,0,0,
		227,228,5,62,0,0,228,229,5,61,0,0,229,70,1,0,0,0,230,231,5,60,0,0,231,
		232,5,61,0,0,232,72,1,0,0,0,233,234,5,61,0,0,234,235,5,61,0,0,235,74,1,
		0,0,0,236,237,5,33,0,0,237,238,5,61,0,0,238,76,1,0,0,0,239,240,5,43,0,
		0,240,241,5,61,0,0,241,78,1,0,0,0,242,243,5,45,0,0,243,244,5,61,0,0,244,
		80,1,0,0,0,245,246,5,42,0,0,246,247,5,61,0,0,247,82,1,0,0,0,248,249,5,
		47,0,0,249,250,5,61,0,0,250,84,1,0,0,0,251,252,5,37,0,0,252,253,5,61,0,
		0,253,86,1,0,0,0,254,255,5,105,0,0,255,256,5,102,0,0,256,258,1,0,0,0,257,
		259,3,5,2,0,258,257,1,0,0,0,259,260,1,0,0,0,260,258,1,0,0,0,260,261,1,
		0,0,0,261,88,1,0,0,0,262,263,5,101,0,0,263,264,5,108,0,0,264,265,5,115,
		0,0,265,266,5,101,0,0,266,90,1,0,0,0,267,268,5,116,0,0,268,269,5,114,0,
		0,269,270,5,117,0,0,270,271,5,101,0,0,271,92,1,0,0,0,272,273,5,102,0,0,
		273,274,5,97,0,0,274,275,5,108,0,0,275,276,5,115,0,0,276,277,5,101,0,0,
		277,94,1,0,0,0,278,279,5,98,0,0,279,280,5,114,0,0,280,281,5,101,0,0,281,
		282,5,97,0,0,282,283,5,107,0,0,283,96,1,0,0,0,284,285,5,99,0,0,285,286,
		5,111,0,0,286,287,5,110,0,0,287,288,5,116,0,0,288,289,5,105,0,0,289,290,
		5,110,0,0,290,291,5,117,0,0,291,292,5,101,0,0,292,98,1,0,0,0,293,294,5,
		119,0,0,294,295,5,104,0,0,295,296,5,105,0,0,296,297,5,108,0,0,297,298,
		5,101,0,0,298,300,1,0,0,0,299,301,3,5,2,0,300,299,1,0,0,0,301,302,1,0,
		0,0,302,300,1,0,0,0,302,303,1,0,0,0,303,100,1,0,0,0,304,305,5,118,0,0,
		305,306,5,97,0,0,306,307,5,114,0,0,307,309,1,0,0,0,308,310,3,5,2,0,309,
		308,1,0,0,0,310,311,1,0,0,0,311,309,1,0,0,0,311,312,1,0,0,0,312,102,1,
		0,0,0,313,314,5,105,0,0,314,315,5,109,0,0,315,316,5,112,0,0,316,317,5,
		111,0,0,317,318,5,114,0,0,318,319,5,116,0,0,319,321,1,0,0,0,320,322,3,
		5,2,0,321,320,1,0,0,0,322,323,1,0,0,0,323,321,1,0,0,0,323,324,1,0,0,0,
		324,104,1,0,0,0,325,326,5,102,0,0,326,327,5,117,0,0,327,328,5,110,0,0,
		328,329,5,99,0,0,329,330,5,116,0,0,330,331,5,105,0,0,331,332,5,111,0,0,
		332,333,5,110,0,0,333,335,1,0,0,0,334,336,3,5,2,0,335,334,1,0,0,0,336,
		337,1,0,0,0,337,335,1,0,0,0,337,338,1,0,0,0,338,106,1,0,0,0,339,340,5,
		114,0,0,340,341,5,101,0,0,341,342,5,116,0,0,342,343,5,117,0,0,343,344,
		5,114,0,0,344,345,5,110,0,0,345,347,1,0,0,0,346,348,3,5,2,0,347,346,1,
		0,0,0,348,349,1,0,0,0,349,347,1,0,0,0,349,350,1,0,0,0,350,108,1,0,0,0,
		351,352,5,114,0,0,352,353,5,101,0,0,353,354,5,112,0,0,354,355,5,101,0,
		0,355,356,5,97,0,0,356,357,5,116,0,0,357,359,1,0,0,0,358,360,3,5,2,0,359,
		358,1,0,0,0,360,361,1,0,0,0,361,359,1,0,0,0,361,362,1,0,0,0,362,110,1,
		0,0,0,363,364,5,111,0,0,364,365,5,110,0,0,365,367,1,0,0,0,366,368,3,5,
		2,0,367,366,1,0,0,0,368,369,1,0,0,0,369,367,1,0,0,0,369,370,1,0,0,0,370,
		112,1,0,0,0,371,372,5,102,0,0,372,373,5,114,0,0,373,374,5,111,0,0,374,
		375,5,109,0,0,375,377,1,0,0,0,376,378,3,5,2,0,377,376,1,0,0,0,378,379,
		1,0,0,0,379,377,1,0,0,0,379,380,1,0,0,0,380,114,1,0,0,0,381,382,5,110,
		0,0,382,383,5,97,0,0,383,384,5,109,0,0,384,385,5,101,0,0,385,386,5,115,
		0,0,386,387,5,112,0,0,387,388,5,97,0,0,388,389,5,99,0,0,389,390,5,101,
		0,0,390,392,1,0,0,0,391,393,3,5,2,0,392,391,1,0,0,0,393,394,1,0,0,0,394,
		392,1,0,0,0,394,395,1,0,0,0,395,116,1,0,0,0,396,398,3,1,0,0,397,396,1,
		0,0,0,398,399,1,0,0,0,399,397,1,0,0,0,399,400,1,0,0,0,400,407,1,0,0,0,
		401,403,7,4,0,0,402,404,3,1,0,0,403,402,1,0,0,0,404,405,1,0,0,0,405,403,
		1,0,0,0,405,406,1,0,0,0,406,408,1,0,0,0,407,401,1,0,0,0,407,408,1,0,0,
		0,408,118,1,0,0,0,409,413,7,5,0,0,410,412,7,6,0,0,411,410,1,0,0,0,412,
		415,1,0,0,0,413,411,1,0,0,0,413,414,1,0,0,0,414,120,1,0,0,0,415,413,1,
		0,0,0,416,420,5,34,0,0,417,419,8,7,0,0,418,417,1,0,0,0,419,422,1,0,0,0,
		420,418,1,0,0,0,420,421,1,0,0,0,421,423,1,0,0,0,422,420,1,0,0,0,423,433,
		5,34,0,0,424,428,5,39,0,0,425,427,8,8,0,0,426,425,1,0,0,0,427,430,1,0,
		0,0,428,426,1,0,0,0,428,429,1,0,0,0,429,431,1,0,0,0,430,428,1,0,0,0,431,
		433,5,39,0,0,432,416,1,0,0,0,432,424,1,0,0,0,433,122,1,0,0,0,434,435,3,
		43,21,0,435,436,3,3,1,0,436,437,3,3,1,0,437,438,3,3,1,0,438,439,3,3,1,
		0,439,440,3,3,1,0,440,441,3,3,1,0,441,124,1,0,0,0,24,0,134,138,179,181,
		188,193,260,302,311,323,337,349,361,369,379,394,399,405,407,413,420,428,
		432,2,0,1,0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
