//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ../ScratchIR.g4 by ANTLR 4.12.0

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
public partial class ScratchIRLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, T__36=37, T__37=38, 
		T__38=39, Event=40, Type=41, ProcedureType=42, NumberType=43, StringType=44, 
		ListType=45, StringNumberType=46, BooleanType=47, WarpIdentifier=48, StackIndexIdentifier=49, 
		ProcedureIndexIdentifier=50, Hashtag=51, Minus=52, Colon=53, End=54, Whitespace=55, 
		NewLine=56, Number=57, Identifier=58, String=59, Color=60;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "T__28", "T__29", "T__30", "T__31", "T__32", 
		"T__33", "T__34", "T__35", "T__36", "T__37", "T__38", "Event", "Type", 
		"ProcedureType", "NumberType", "StringType", "ListType", "StringNumberType", 
		"BooleanType", "WarpIdentifier", "StackIndexIdentifier", "ProcedureIndexIdentifier", 
		"Hashtag", "Minus", "Colon", "End", "Digit", "HexDigit", "Whitespace", 
		"NewLine", "Number", "Identifier", "String", "Color"
	};


	public ScratchIRLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ScratchIRLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'load'", "'set'", "'while'", "'repeat'", "'call'", "'raw'", "'push'", 
		"'pushat'", "'pop'", "'popat'", "'popall'", "'proc'", "'on'", "'flag'", 
		"'('", "')'", "'rawshadow'", "'endshadow'", "'!'", "'if'", "'else'", "'i:'", 
		"'f:'", "'var:'", "'arr:'", "'+'", "'~'", "'*'", "'/'", "'%'", "'&&'", 
		"'||'", "'^'", "'=='", "'!='", "'>'", "'>='", "'<'", "'<='", "'start'", 
		null, null, "':number'", "':string'", "':list'", "':sn'", "':bool'", "':w'", 
		"':si:'", "':pi:'", "'#'", "'-'", "':'", "'end'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, "Event", "Type", "ProcedureType", "NumberType", 
		"StringType", "ListType", "StringNumberType", "BooleanType", "WarpIdentifier", 
		"StackIndexIdentifier", "ProcedureIndexIdentifier", "Hashtag", "Minus", 
		"Colon", "End", "Whitespace", "NewLine", "Number", "Identifier", "String", 
		"Color"
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

	public override string GrammarFileName { get { return "ScratchIR.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ScratchIRLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,60,422,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,1,0,1,0,1,0,1,0,
		1,0,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,3,1,
		3,1,4,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,
		1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,
		1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,13,1,13,
		1,13,1,13,1,13,1,14,1,14,1,15,1,15,1,16,1,16,1,16,1,16,1,16,1,16,1,16,
		1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,18,
		1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,20,1,20,1,21,1,21,1,21,1,22,1,22,
		1,22,1,23,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,25,1,25,1,26,
		1,26,1,27,1,27,1,28,1,28,1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,31,1,32,
		1,32,1,33,1,33,1,33,1,34,1,34,1,34,1,35,1,35,1,36,1,36,1,36,1,37,1,37,
		1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,3,40,292,8,
		40,1,41,1,41,3,41,296,8,41,1,42,1,42,1,42,1,42,1,42,1,42,1,42,1,42,1,43,
		1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,45,
		1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,1,48,1,48,
		1,48,1,48,1,48,1,49,1,49,1,49,1,49,1,49,1,50,1,50,1,51,1,51,1,52,1,52,
		1,53,1,53,1,53,1,53,1,54,1,54,1,55,1,55,1,56,1,56,1,56,1,56,1,57,3,57,
		362,8,57,1,57,1,57,3,57,366,8,57,1,57,1,57,1,58,3,58,371,8,58,1,58,4,58,
		374,8,58,11,58,12,58,375,1,58,1,58,4,58,380,8,58,11,58,12,58,381,3,58,
		384,8,58,1,59,1,59,5,59,388,8,59,10,59,12,59,391,9,59,1,60,1,60,1,60,1,
		60,5,60,397,8,60,10,60,12,60,400,9,60,1,60,1,60,1,60,1,60,1,60,5,60,407,
		8,60,10,60,12,60,410,9,60,1,60,3,60,413,8,60,1,61,1,61,1,61,1,61,1,61,
		1,61,1,61,1,61,0,0,62,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,
		11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,
		23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,
		35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,
		47,95,48,97,49,99,50,101,51,103,52,105,53,107,54,109,0,111,0,113,55,115,
		56,117,57,119,58,121,59,123,60,1,0,11,1,0,48,57,3,0,48,57,65,70,97,102,
		2,0,9,9,32,32,2,0,10,10,13,13,1,0,46,46,3,0,65,90,95,95,97,122,4,0,48,
		57,65,90,95,95,97,122,4,0,10,10,13,13,34,34,92,92,2,0,34,34,92,92,4,0,
		10,10,13,13,39,39,92,92,2,0,39,39,92,92,434,0,1,1,0,0,0,0,3,1,0,0,0,0,
		5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,
		0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,
		1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,
		0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,
		1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,
		0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,
		1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,
		0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,
		1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,
		0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,
		0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,0,0,1,125,1,0,0,0,3,130,1,0,
		0,0,5,134,1,0,0,0,7,140,1,0,0,0,9,147,1,0,0,0,11,152,1,0,0,0,13,156,1,
		0,0,0,15,161,1,0,0,0,17,168,1,0,0,0,19,172,1,0,0,0,21,178,1,0,0,0,23,185,
		1,0,0,0,25,190,1,0,0,0,27,193,1,0,0,0,29,198,1,0,0,0,31,200,1,0,0,0,33,
		202,1,0,0,0,35,212,1,0,0,0,37,222,1,0,0,0,39,224,1,0,0,0,41,227,1,0,0,
		0,43,232,1,0,0,0,45,235,1,0,0,0,47,238,1,0,0,0,49,243,1,0,0,0,51,248,1,
		0,0,0,53,250,1,0,0,0,55,252,1,0,0,0,57,254,1,0,0,0,59,256,1,0,0,0,61,258,
		1,0,0,0,63,261,1,0,0,0,65,264,1,0,0,0,67,266,1,0,0,0,69,269,1,0,0,0,71,
		272,1,0,0,0,73,274,1,0,0,0,75,277,1,0,0,0,77,279,1,0,0,0,79,282,1,0,0,
		0,81,291,1,0,0,0,83,295,1,0,0,0,85,297,1,0,0,0,87,305,1,0,0,0,89,313,1,
		0,0,0,91,319,1,0,0,0,93,323,1,0,0,0,95,329,1,0,0,0,97,332,1,0,0,0,99,337,
		1,0,0,0,101,342,1,0,0,0,103,344,1,0,0,0,105,346,1,0,0,0,107,348,1,0,0,
		0,109,352,1,0,0,0,111,354,1,0,0,0,113,356,1,0,0,0,115,365,1,0,0,0,117,
		370,1,0,0,0,119,385,1,0,0,0,121,412,1,0,0,0,123,414,1,0,0,0,125,126,5,
		108,0,0,126,127,5,111,0,0,127,128,5,97,0,0,128,129,5,100,0,0,129,2,1,0,
		0,0,130,131,5,115,0,0,131,132,5,101,0,0,132,133,5,116,0,0,133,4,1,0,0,
		0,134,135,5,119,0,0,135,136,5,104,0,0,136,137,5,105,0,0,137,138,5,108,
		0,0,138,139,5,101,0,0,139,6,1,0,0,0,140,141,5,114,0,0,141,142,5,101,0,
		0,142,143,5,112,0,0,143,144,5,101,0,0,144,145,5,97,0,0,145,146,5,116,0,
		0,146,8,1,0,0,0,147,148,5,99,0,0,148,149,5,97,0,0,149,150,5,108,0,0,150,
		151,5,108,0,0,151,10,1,0,0,0,152,153,5,114,0,0,153,154,5,97,0,0,154,155,
		5,119,0,0,155,12,1,0,0,0,156,157,5,112,0,0,157,158,5,117,0,0,158,159,5,
		115,0,0,159,160,5,104,0,0,160,14,1,0,0,0,161,162,5,112,0,0,162,163,5,117,
		0,0,163,164,5,115,0,0,164,165,5,104,0,0,165,166,5,97,0,0,166,167,5,116,
		0,0,167,16,1,0,0,0,168,169,5,112,0,0,169,170,5,111,0,0,170,171,5,112,0,
		0,171,18,1,0,0,0,172,173,5,112,0,0,173,174,5,111,0,0,174,175,5,112,0,0,
		175,176,5,97,0,0,176,177,5,116,0,0,177,20,1,0,0,0,178,179,5,112,0,0,179,
		180,5,111,0,0,180,181,5,112,0,0,181,182,5,97,0,0,182,183,5,108,0,0,183,
		184,5,108,0,0,184,22,1,0,0,0,185,186,5,112,0,0,186,187,5,114,0,0,187,188,
		5,111,0,0,188,189,5,99,0,0,189,24,1,0,0,0,190,191,5,111,0,0,191,192,5,
		110,0,0,192,26,1,0,0,0,193,194,5,102,0,0,194,195,5,108,0,0,195,196,5,97,
		0,0,196,197,5,103,0,0,197,28,1,0,0,0,198,199,5,40,0,0,199,30,1,0,0,0,200,
		201,5,41,0,0,201,32,1,0,0,0,202,203,5,114,0,0,203,204,5,97,0,0,204,205,
		5,119,0,0,205,206,5,115,0,0,206,207,5,104,0,0,207,208,5,97,0,0,208,209,
		5,100,0,0,209,210,5,111,0,0,210,211,5,119,0,0,211,34,1,0,0,0,212,213,5,
		101,0,0,213,214,5,110,0,0,214,215,5,100,0,0,215,216,5,115,0,0,216,217,
		5,104,0,0,217,218,5,97,0,0,218,219,5,100,0,0,219,220,5,111,0,0,220,221,
		5,119,0,0,221,36,1,0,0,0,222,223,5,33,0,0,223,38,1,0,0,0,224,225,5,105,
		0,0,225,226,5,102,0,0,226,40,1,0,0,0,227,228,5,101,0,0,228,229,5,108,0,
		0,229,230,5,115,0,0,230,231,5,101,0,0,231,42,1,0,0,0,232,233,5,105,0,0,
		233,234,5,58,0,0,234,44,1,0,0,0,235,236,5,102,0,0,236,237,5,58,0,0,237,
		46,1,0,0,0,238,239,5,118,0,0,239,240,5,97,0,0,240,241,5,114,0,0,241,242,
		5,58,0,0,242,48,1,0,0,0,243,244,5,97,0,0,244,245,5,114,0,0,245,246,5,114,
		0,0,246,247,5,58,0,0,247,50,1,0,0,0,248,249,5,43,0,0,249,52,1,0,0,0,250,
		251,5,126,0,0,251,54,1,0,0,0,252,253,5,42,0,0,253,56,1,0,0,0,254,255,5,
		47,0,0,255,58,1,0,0,0,256,257,5,37,0,0,257,60,1,0,0,0,258,259,5,38,0,0,
		259,260,5,38,0,0,260,62,1,0,0,0,261,262,5,124,0,0,262,263,5,124,0,0,263,
		64,1,0,0,0,264,265,5,94,0,0,265,66,1,0,0,0,266,267,5,61,0,0,267,268,5,
		61,0,0,268,68,1,0,0,0,269,270,5,33,0,0,270,271,5,61,0,0,271,70,1,0,0,0,
		272,273,5,62,0,0,273,72,1,0,0,0,274,275,5,62,0,0,275,276,5,61,0,0,276,
		74,1,0,0,0,277,278,5,60,0,0,278,76,1,0,0,0,279,280,5,60,0,0,280,281,5,
		61,0,0,281,78,1,0,0,0,282,283,5,115,0,0,283,284,5,116,0,0,284,285,5,97,
		0,0,285,286,5,114,0,0,286,287,5,116,0,0,287,80,1,0,0,0,288,292,3,85,42,
		0,289,292,3,87,43,0,290,292,3,89,44,0,291,288,1,0,0,0,291,289,1,0,0,0,
		291,290,1,0,0,0,292,82,1,0,0,0,293,296,3,91,45,0,294,296,3,93,46,0,295,
		293,1,0,0,0,295,294,1,0,0,0,296,84,1,0,0,0,297,298,5,58,0,0,298,299,5,
		110,0,0,299,300,5,117,0,0,300,301,5,109,0,0,301,302,5,98,0,0,302,303,5,
		101,0,0,303,304,5,114,0,0,304,86,1,0,0,0,305,306,5,58,0,0,306,307,5,115,
		0,0,307,308,5,116,0,0,308,309,5,114,0,0,309,310,5,105,0,0,310,311,5,110,
		0,0,311,312,5,103,0,0,312,88,1,0,0,0,313,314,5,58,0,0,314,315,5,108,0,
		0,315,316,5,105,0,0,316,317,5,115,0,0,317,318,5,116,0,0,318,90,1,0,0,0,
		319,320,5,58,0,0,320,321,5,115,0,0,321,322,5,110,0,0,322,92,1,0,0,0,323,
		324,5,58,0,0,324,325,5,98,0,0,325,326,5,111,0,0,326,327,5,111,0,0,327,
		328,5,108,0,0,328,94,1,0,0,0,329,330,5,58,0,0,330,331,5,119,0,0,331,96,
		1,0,0,0,332,333,5,58,0,0,333,334,5,115,0,0,334,335,5,105,0,0,335,336,5,
		58,0,0,336,98,1,0,0,0,337,338,5,58,0,0,338,339,5,112,0,0,339,340,5,105,
		0,0,340,341,5,58,0,0,341,100,1,0,0,0,342,343,5,35,0,0,343,102,1,0,0,0,
		344,345,5,45,0,0,345,104,1,0,0,0,346,347,5,58,0,0,347,106,1,0,0,0,348,
		349,5,101,0,0,349,350,5,110,0,0,350,351,5,100,0,0,351,108,1,0,0,0,352,
		353,7,0,0,0,353,110,1,0,0,0,354,355,7,1,0,0,355,112,1,0,0,0,356,357,7,
		2,0,0,357,358,1,0,0,0,358,359,6,56,0,0,359,114,1,0,0,0,360,362,5,13,0,
		0,361,360,1,0,0,0,361,362,1,0,0,0,362,363,1,0,0,0,363,366,5,10,0,0,364,
		366,7,3,0,0,365,361,1,0,0,0,365,364,1,0,0,0,366,367,1,0,0,0,367,368,6,
		57,1,0,368,116,1,0,0,0,369,371,3,103,51,0,370,369,1,0,0,0,370,371,1,0,
		0,0,371,373,1,0,0,0,372,374,3,109,54,0,373,372,1,0,0,0,374,375,1,0,0,0,
		375,373,1,0,0,0,375,376,1,0,0,0,376,383,1,0,0,0,377,379,7,4,0,0,378,380,
		3,109,54,0,379,378,1,0,0,0,380,381,1,0,0,0,381,379,1,0,0,0,381,382,1,0,
		0,0,382,384,1,0,0,0,383,377,1,0,0,0,383,384,1,0,0,0,384,118,1,0,0,0,385,
		389,7,5,0,0,386,388,7,6,0,0,387,386,1,0,0,0,388,391,1,0,0,0,389,387,1,
		0,0,0,389,390,1,0,0,0,390,120,1,0,0,0,391,389,1,0,0,0,392,398,5,34,0,0,
		393,397,8,7,0,0,394,395,5,92,0,0,395,397,7,8,0,0,396,393,1,0,0,0,396,394,
		1,0,0,0,397,400,1,0,0,0,398,396,1,0,0,0,398,399,1,0,0,0,399,401,1,0,0,
		0,400,398,1,0,0,0,401,413,5,34,0,0,402,408,5,39,0,0,403,407,8,9,0,0,404,
		405,5,92,0,0,405,407,7,10,0,0,406,403,1,0,0,0,406,404,1,0,0,0,407,410,
		1,0,0,0,408,406,1,0,0,0,408,409,1,0,0,0,409,411,1,0,0,0,410,408,1,0,0,
		0,411,413,5,39,0,0,412,392,1,0,0,0,412,402,1,0,0,0,413,122,1,0,0,0,414,
		415,3,101,50,0,415,416,3,111,55,0,416,417,3,111,55,0,417,418,3,111,55,
		0,418,419,3,111,55,0,419,420,3,111,55,0,420,421,3,111,55,0,421,124,1,0,
		0,0,15,0,291,295,361,365,370,375,381,383,389,396,398,406,408,412,2,0,1,
		0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
