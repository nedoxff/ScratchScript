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
		T__31=32, T__32=33, T__33=34, T__34=35, Event=36, Type=37, ProcedureType=38, 
		NumberType=39, StringType=40, ListType=41, StringNumberType=42, BooleanType=43, 
		ArgumentReporterIdentifier=44, Hashtag=45, Minus=46, Colon=47, End=48, 
		Whitespace=49, NewLine=50, Number=51, Identifier=52, String=53, Color=54;
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
		"T__33", "T__34", "Event", "Type", "ProcedureType", "NumberType", "StringType", 
		"ListType", "StringNumberType", "BooleanType", "ArgumentReporterIdentifier", 
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
		null, "'set'", "'while'", "'repeat'", "'call'", "'raw'", "'push'", "'pushat'", 
		"'pop'", "'popat'", "'popall'", "'load'", "'proc'", "'on'", "'('", "')'", 
		"'!'", "'if'", "'else'", "'i:'", "'f:'", "'v:'", "'+'", "'~'", "'*'", 
		"'/'", "'%'", "'&&'", "'||'", "'^'", "'=='", "'!='", "'>'", "'>='", "'<'", 
		"'<='", "'start'", null, null, "':n'", "':s'", "':l'", "':sn'", "':b'", 
		"'argr:'", "'#'", "'-'", "':'", "'end'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		"Event", "Type", "ProcedureType", "NumberType", "StringType", "ListType", 
		"StringNumberType", "BooleanType", "ArgumentReporterIdentifier", "Hashtag", 
		"Minus", "Colon", "End", "Whitespace", "NewLine", "Number", "Identifier", 
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
		4,0,54,351,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,1,0,1,
		0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,3,1,3,
		1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,
		6,1,6,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,
		1,9,1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,
		13,1,13,1,14,1,14,1,15,1,15,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,
		18,1,18,1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,21,1,21,1,22,1,22,1,23,1,
		23,1,24,1,24,1,25,1,25,1,26,1,26,1,26,1,27,1,27,1,27,1,28,1,28,1,29,1,
		29,1,29,1,30,1,30,1,30,1,31,1,31,1,32,1,32,1,32,1,33,1,33,1,34,1,34,1,
		34,1,35,1,35,1,35,1,35,1,35,1,35,1,36,1,36,1,36,3,36,248,8,36,1,37,1,37,
		3,37,252,8,37,1,38,1,38,1,38,1,39,1,39,1,39,1,40,1,40,1,40,1,41,1,41,1,
		41,1,41,1,42,1,42,1,42,1,43,1,43,1,43,1,43,1,43,1,43,1,44,1,44,1,45,1,
		45,1,46,1,46,1,47,1,47,1,47,1,47,1,48,1,48,1,49,1,49,1,50,1,50,1,50,1,
		50,1,51,3,51,295,8,51,1,51,1,51,3,51,299,8,51,1,51,1,51,1,52,3,52,304,
		8,52,1,52,4,52,307,8,52,11,52,12,52,308,1,52,1,52,4,52,313,8,52,11,52,
		12,52,314,3,52,317,8,52,1,53,1,53,5,53,321,8,53,10,53,12,53,324,9,53,1,
		54,1,54,5,54,328,8,54,10,54,12,54,331,9,54,1,54,1,54,1,54,5,54,336,8,54,
		10,54,12,54,339,9,54,1,54,3,54,342,8,54,1,55,1,55,1,55,1,55,1,55,1,55,
		1,55,1,55,0,0,56,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,
		12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,
		24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,
		36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,47,95,
		48,97,0,99,0,101,49,103,50,105,51,107,52,109,53,111,54,1,0,9,1,0,48,57,
		2,0,48,57,65,70,2,0,9,9,32,32,2,0,10,10,13,13,1,0,46,46,3,0,65,90,95,95,
		97,122,4,0,48,57,65,90,95,95,97,122,1,0,34,34,1,0,39,39,361,0,1,1,0,0,
		0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,
		0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,
		0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,
		1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,
		0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,
		1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,
		0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,
		1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,
		0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,
		105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,1,113,1,0,0,0,3,
		117,1,0,0,0,5,123,1,0,0,0,7,130,1,0,0,0,9,135,1,0,0,0,11,139,1,0,0,0,13,
		144,1,0,0,0,15,151,1,0,0,0,17,155,1,0,0,0,19,161,1,0,0,0,21,168,1,0,0,
		0,23,173,1,0,0,0,25,178,1,0,0,0,27,181,1,0,0,0,29,183,1,0,0,0,31,185,1,
		0,0,0,33,187,1,0,0,0,35,190,1,0,0,0,37,195,1,0,0,0,39,198,1,0,0,0,41,201,
		1,0,0,0,43,204,1,0,0,0,45,206,1,0,0,0,47,208,1,0,0,0,49,210,1,0,0,0,51,
		212,1,0,0,0,53,214,1,0,0,0,55,217,1,0,0,0,57,220,1,0,0,0,59,222,1,0,0,
		0,61,225,1,0,0,0,63,228,1,0,0,0,65,230,1,0,0,0,67,233,1,0,0,0,69,235,1,
		0,0,0,71,238,1,0,0,0,73,247,1,0,0,0,75,251,1,0,0,0,77,253,1,0,0,0,79,256,
		1,0,0,0,81,259,1,0,0,0,83,262,1,0,0,0,85,266,1,0,0,0,87,269,1,0,0,0,89,
		275,1,0,0,0,91,277,1,0,0,0,93,279,1,0,0,0,95,281,1,0,0,0,97,285,1,0,0,
		0,99,287,1,0,0,0,101,289,1,0,0,0,103,298,1,0,0,0,105,303,1,0,0,0,107,318,
		1,0,0,0,109,341,1,0,0,0,111,343,1,0,0,0,113,114,5,115,0,0,114,115,5,101,
		0,0,115,116,5,116,0,0,116,2,1,0,0,0,117,118,5,119,0,0,118,119,5,104,0,
		0,119,120,5,105,0,0,120,121,5,108,0,0,121,122,5,101,0,0,122,4,1,0,0,0,
		123,124,5,114,0,0,124,125,5,101,0,0,125,126,5,112,0,0,126,127,5,101,0,
		0,127,128,5,97,0,0,128,129,5,116,0,0,129,6,1,0,0,0,130,131,5,99,0,0,131,
		132,5,97,0,0,132,133,5,108,0,0,133,134,5,108,0,0,134,8,1,0,0,0,135,136,
		5,114,0,0,136,137,5,97,0,0,137,138,5,119,0,0,138,10,1,0,0,0,139,140,5,
		112,0,0,140,141,5,117,0,0,141,142,5,115,0,0,142,143,5,104,0,0,143,12,1,
		0,0,0,144,145,5,112,0,0,145,146,5,117,0,0,146,147,5,115,0,0,147,148,5,
		104,0,0,148,149,5,97,0,0,149,150,5,116,0,0,150,14,1,0,0,0,151,152,5,112,
		0,0,152,153,5,111,0,0,153,154,5,112,0,0,154,16,1,0,0,0,155,156,5,112,0,
		0,156,157,5,111,0,0,157,158,5,112,0,0,158,159,5,97,0,0,159,160,5,116,0,
		0,160,18,1,0,0,0,161,162,5,112,0,0,162,163,5,111,0,0,163,164,5,112,0,0,
		164,165,5,97,0,0,165,166,5,108,0,0,166,167,5,108,0,0,167,20,1,0,0,0,168,
		169,5,108,0,0,169,170,5,111,0,0,170,171,5,97,0,0,171,172,5,100,0,0,172,
		22,1,0,0,0,173,174,5,112,0,0,174,175,5,114,0,0,175,176,5,111,0,0,176,177,
		5,99,0,0,177,24,1,0,0,0,178,179,5,111,0,0,179,180,5,110,0,0,180,26,1,0,
		0,0,181,182,5,40,0,0,182,28,1,0,0,0,183,184,5,41,0,0,184,30,1,0,0,0,185,
		186,5,33,0,0,186,32,1,0,0,0,187,188,5,105,0,0,188,189,5,102,0,0,189,34,
		1,0,0,0,190,191,5,101,0,0,191,192,5,108,0,0,192,193,5,115,0,0,193,194,
		5,101,0,0,194,36,1,0,0,0,195,196,5,105,0,0,196,197,5,58,0,0,197,38,1,0,
		0,0,198,199,5,102,0,0,199,200,5,58,0,0,200,40,1,0,0,0,201,202,5,118,0,
		0,202,203,5,58,0,0,203,42,1,0,0,0,204,205,5,43,0,0,205,44,1,0,0,0,206,
		207,5,126,0,0,207,46,1,0,0,0,208,209,5,42,0,0,209,48,1,0,0,0,210,211,5,
		47,0,0,211,50,1,0,0,0,212,213,5,37,0,0,213,52,1,0,0,0,214,215,5,38,0,0,
		215,216,5,38,0,0,216,54,1,0,0,0,217,218,5,124,0,0,218,219,5,124,0,0,219,
		56,1,0,0,0,220,221,5,94,0,0,221,58,1,0,0,0,222,223,5,61,0,0,223,224,5,
		61,0,0,224,60,1,0,0,0,225,226,5,33,0,0,226,227,5,61,0,0,227,62,1,0,0,0,
		228,229,5,62,0,0,229,64,1,0,0,0,230,231,5,62,0,0,231,232,5,61,0,0,232,
		66,1,0,0,0,233,234,5,60,0,0,234,68,1,0,0,0,235,236,5,60,0,0,236,237,5,
		61,0,0,237,70,1,0,0,0,238,239,5,115,0,0,239,240,5,116,0,0,240,241,5,97,
		0,0,241,242,5,114,0,0,242,243,5,116,0,0,243,72,1,0,0,0,244,248,3,77,38,
		0,245,248,3,79,39,0,246,248,3,81,40,0,247,244,1,0,0,0,247,245,1,0,0,0,
		247,246,1,0,0,0,248,74,1,0,0,0,249,252,3,83,41,0,250,252,3,85,42,0,251,
		249,1,0,0,0,251,250,1,0,0,0,252,76,1,0,0,0,253,254,5,58,0,0,254,255,5,
		110,0,0,255,78,1,0,0,0,256,257,5,58,0,0,257,258,5,115,0,0,258,80,1,0,0,
		0,259,260,5,58,0,0,260,261,5,108,0,0,261,82,1,0,0,0,262,263,5,58,0,0,263,
		264,5,115,0,0,264,265,5,110,0,0,265,84,1,0,0,0,266,267,5,58,0,0,267,268,
		5,98,0,0,268,86,1,0,0,0,269,270,5,97,0,0,270,271,5,114,0,0,271,272,5,103,
		0,0,272,273,5,114,0,0,273,274,5,58,0,0,274,88,1,0,0,0,275,276,5,35,0,0,
		276,90,1,0,0,0,277,278,5,45,0,0,278,92,1,0,0,0,279,280,5,58,0,0,280,94,
		1,0,0,0,281,282,5,101,0,0,282,283,5,110,0,0,283,284,5,100,0,0,284,96,1,
		0,0,0,285,286,7,0,0,0,286,98,1,0,0,0,287,288,7,1,0,0,288,100,1,0,0,0,289,
		290,7,2,0,0,290,291,1,0,0,0,291,292,6,50,0,0,292,102,1,0,0,0,293,295,5,
		13,0,0,294,293,1,0,0,0,294,295,1,0,0,0,295,296,1,0,0,0,296,299,5,10,0,
		0,297,299,7,3,0,0,298,294,1,0,0,0,298,297,1,0,0,0,299,300,1,0,0,0,300,
		301,6,51,1,0,301,104,1,0,0,0,302,304,3,91,45,0,303,302,1,0,0,0,303,304,
		1,0,0,0,304,306,1,0,0,0,305,307,3,97,48,0,306,305,1,0,0,0,307,308,1,0,
		0,0,308,306,1,0,0,0,308,309,1,0,0,0,309,316,1,0,0,0,310,312,7,4,0,0,311,
		313,3,97,48,0,312,311,1,0,0,0,313,314,1,0,0,0,314,312,1,0,0,0,314,315,
		1,0,0,0,315,317,1,0,0,0,316,310,1,0,0,0,316,317,1,0,0,0,317,106,1,0,0,
		0,318,322,7,5,0,0,319,321,7,6,0,0,320,319,1,0,0,0,321,324,1,0,0,0,322,
		320,1,0,0,0,322,323,1,0,0,0,323,108,1,0,0,0,324,322,1,0,0,0,325,329,5,
		34,0,0,326,328,8,7,0,0,327,326,1,0,0,0,328,331,1,0,0,0,329,327,1,0,0,0,
		329,330,1,0,0,0,330,332,1,0,0,0,331,329,1,0,0,0,332,342,5,34,0,0,333,337,
		5,39,0,0,334,336,8,8,0,0,335,334,1,0,0,0,336,339,1,0,0,0,337,335,1,0,0,
		0,337,338,1,0,0,0,338,340,1,0,0,0,339,337,1,0,0,0,340,342,5,39,0,0,341,
		325,1,0,0,0,341,333,1,0,0,0,342,110,1,0,0,0,343,344,3,89,44,0,344,345,
		3,99,49,0,345,346,3,99,49,0,346,347,3,99,49,0,347,348,3,99,49,0,348,349,
		3,99,49,0,349,350,3,99,49,0,350,112,1,0,0,0,13,0,247,251,294,298,303,308,
		314,316,322,329,337,341,2,0,1,0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
