//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:/GitHub/Elan/Parser/Elan.g4 by ANTLR 4.13.1

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

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class ElanLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		BYTE_ORDER_MARK=1, NL=2, SINGLE_LINE_COMMENT=3, COMMENT_MARKER=4, AS=5, 
		ABSTRACT=6, CASE=7, CATCH=8, CLASS=9, CONSTANT=10, CONSTRUCTOR=11, CURRY=12, 
		DEFAULT=13, ELSE=14, END=15, ENUMERATION=16, FOR=17, FOREACH=18, FUNCTION=19, 
		GLOBAL=20, IF=21, IMMUTABLE=22, IN=23, INHERITS=24, LAMBDA=25, LET=26, 
		MAIN=27, PARTIAL=28, PRIVATE=29, PROCEDURE=30, PROPERTY=31, REPEAT=32, 
		RETURN=33, SELF=34, STEP=35, SWITCH=36, THEN=37, TO=38, TRY=39, UNTIL=40, 
		VAR=41, WHILE=42, WITH=43, BOOL_VALUE=44, VALUE_TYPE=45, ARRAY=46, LIST=47, 
		DICTIONARY=48, ITERABLE=49, ASSIGN=50, ARROW=51, OPEN_BRACE=52, CLOSE_BRACE=53, 
		OPEN_SQ_BRACKET=54, CLOSE_SQ_BRACKET=55, OPEN_BRACKET=56, CLOSE_BRACKET=57, 
		DOUBLE_DOT=58, DOT=59, COMMA=60, COLON=61, PLUS=62, MINUS=63, MULT=64, 
		DIVIDE=65, POWER=66, MOD=67, INT_DIV=68, LT=69, GT=70, OP_AND=71, OP_NOT=72, 
		OP_OR=73, OP_XOR=74, OP_EQ=75, OP_NE=76, OP_LE=77, OP_GE=78, TYPENAME=79, 
		IDENTIFIER=80, LITERAL_INTEGER=81, LITERAL_FLOAT=82, LITERAL_DECIMAL=83, 
		LITERAL_CHAR=84, LITERAL_STRING=85, VERBATIM_ONLY_STRING=86, WHITESPACES=87, 
		NEWLINE=88, WS=89;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"BYTE_ORDER_MARK", "NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "AS", 
		"ABSTRACT", "CASE", "CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", 
		"DEFAULT", "ELSE", "END", "ENUMERATION", "FOR", "FOREACH", "FUNCTION", 
		"GLOBAL", "IF", "IMMUTABLE", "IN", "INHERITS", "LAMBDA", "LET", "MAIN", 
		"PARTIAL", "PRIVATE", "PROCEDURE", "PROPERTY", "REPEAT", "RETURN", "SELF", 
		"STEP", "SWITCH", "THEN", "TO", "TRY", "UNTIL", "VAR", "WHILE", "WITH", 
		"BOOL_VALUE", "VALUE_TYPE", "ARRAY", "LIST", "DICTIONARY", "ITERABLE", 
		"ASSIGN", "ARROW", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", 
		"OPEN_BRACKET", "CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", 
		"PLUS", "MINUS", "MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", 
		"OP_AND", "OP_NOT", "OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", 
		"TYPENAME", "IDENTIFIER", "LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_DECIMAL", 
		"LITERAL_CHAR", "LITERAL_STRING", "VERBATIM_ONLY_STRING", "WHITESPACES", 
		"InputCharacter", "NewLineCharacter", "ExponentPart", "CommonCharacter", 
		"SimpleEscapeSequence", "HexEscapeSequence", "NewLine", "Whitespace", 
		"UnicodeClassZS", "IdentifierStartingUCorLC", "IdentifierStartingLC", 
		"IdentifierStartingUC", "IdentifierPartCharacter", "LetterCharacter", 
		"DecimalDigitCharacter", "ConnectingCharacter", "CombiningCharacter", 
		"FormattingCharacter", "UnicodeEscapeSequence", "HexDigit", "UnicodeClassLU", 
		"UnicodeClassLL", "UnicodeClassLT", "UnicodeClassLM", "UnicodeClassLO", 
		"UnicodeClassNL", "UnicodeClassMN", "UnicodeClassMC", "UnicodeClassCF", 
		"UnicodeClassPC", "UnicodeClassND", "NEWLINE", "WS"
	};


	public ElanLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ElanLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'\\u00EF\\u00BB\\u00BF'", null, null, "'#'", "'as'", "'abstract'", 
		"'case'", "'catch'", "'class'", "'constant'", "'constructor'", "'curry'", 
		"'default'", "'else'", "'end'", "'enumeration'", "'for'", "'foreach'", 
		"'function'", "'global'", "'if'", "'immutable'", "'in'", "'inherits'", 
		"'lambda'", "'let'", "'main'", "'partial'", "'private'", "'procedure'", 
		"'property'", "'repeat'", "'return'", "'self'", "'step'", "'switch'", 
		"'then'", "'to'", "'try'", "'until'", "'var'", "'while'", "'with'", null, 
		null, "'Array'", "'List'", "'Dictionary'", "'Iter'", "'='", "'->'", "'{'", 
		"'}'", "'['", "']'", "'('", "')'", "'..'", "'.'", null, "':'", "'+'", 
		"'-'", "'*'", "'/'", "'^'", "'mod'", "'div'", "'<'", "'>'", "'and'", "'not'", 
		"'or'", "'xor'", null, null, "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "BYTE_ORDER_MARK", "NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", 
		"AS", "ABSTRACT", "CASE", "CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", 
		"CURRY", "DEFAULT", "ELSE", "END", "ENUMERATION", "FOR", "FOREACH", "FUNCTION", 
		"GLOBAL", "IF", "IMMUTABLE", "IN", "INHERITS", "LAMBDA", "LET", "MAIN", 
		"PARTIAL", "PRIVATE", "PROCEDURE", "PROPERTY", "REPEAT", "RETURN", "SELF", 
		"STEP", "SWITCH", "THEN", "TO", "TRY", "UNTIL", "VAR", "WHILE", "WITH", 
		"BOOL_VALUE", "VALUE_TYPE", "ARRAY", "LIST", "DICTIONARY", "ITERABLE", 
		"ASSIGN", "ARROW", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", 
		"OPEN_BRACKET", "CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", 
		"PLUS", "MINUS", "MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", 
		"OP_AND", "OP_NOT", "OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", 
		"TYPENAME", "IDENTIFIER", "LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_DECIMAL", 
		"LITERAL_CHAR", "LITERAL_STRING", "VERBATIM_ONLY_STRING", "WHITESPACES", 
		"NEWLINE", "WS"
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

	public override string GrammarFileName { get { return "Elan.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ElanLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,89,1016,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,2,62,7,62,2,63,
		7,63,2,64,7,64,2,65,7,65,2,66,7,66,2,67,7,67,2,68,7,68,2,69,7,69,2,70,
		7,70,2,71,7,71,2,72,7,72,2,73,7,73,2,74,7,74,2,75,7,75,2,76,7,76,2,77,
		7,77,2,78,7,78,2,79,7,79,2,80,7,80,2,81,7,81,2,82,7,82,2,83,7,83,2,84,
		7,84,2,85,7,85,2,86,7,86,2,87,7,87,2,88,7,88,2,89,7,89,2,90,7,90,2,91,
		7,91,2,92,7,92,2,93,7,93,2,94,7,94,2,95,7,95,2,96,7,96,2,97,7,97,2,98,
		7,98,2,99,7,99,2,100,7,100,2,101,7,101,2,102,7,102,2,103,7,103,2,104,7,
		104,2,105,7,105,2,106,7,106,2,107,7,107,2,108,7,108,2,109,7,109,2,110,
		7,110,2,111,7,111,2,112,7,112,2,113,7,113,2,114,7,114,2,115,7,115,2,116,
		7,116,2,117,7,117,2,118,7,118,2,119,7,119,1,0,1,0,1,0,1,0,1,1,4,1,247,
		8,1,11,1,12,1,248,1,2,3,2,252,8,2,1,2,1,2,5,2,256,8,2,10,2,12,2,259,9,
		2,1,2,1,2,1,3,1,3,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,1,5,1,5,1,5,1,6,
		1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,
		9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,
		1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,12,
		1,12,1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,15,
		1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,16,1,16,1,16,
		1,16,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,
		1,18,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,
		1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,23,
		1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,24,
		1,24,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,
		1,27,1,27,1,27,1,27,1,28,1,28,1,28,1,28,1,28,1,28,1,28,1,28,1,29,1,29,
		1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,30,1,30,1,30,1,30,1,30,1,30,
		1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,1,31,1,31,1,32,1,32,1,32,1,32,
		1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,1,34,1,35,
		1,35,1,35,1,35,1,35,1,35,1,35,1,36,1,36,1,36,1,36,1,36,1,37,1,37,1,37,
		1,38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,
		1,41,1,41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,42,1,43,1,43,1,43,
		1,43,1,43,1,43,1,43,1,43,1,43,3,43,527,8,43,1,44,1,44,1,44,1,44,1,44,1,
		44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,
		44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,44,1,
		44,1,44,1,44,1,44,1,44,1,44,1,44,3,44,569,8,44,1,45,1,45,1,45,1,45,1,45,
		1,45,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,1,47,1,47,1,47,1,47,1,47,
		1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,48,1,49,1,49,1,50,1,50,1,50,1,51,
		1,51,1,52,1,52,1,53,1,53,1,54,1,54,1,55,1,55,1,56,1,56,1,57,1,57,1,57,
		1,58,1,58,1,59,1,59,5,59,622,8,59,10,59,12,59,625,9,59,1,59,3,59,628,8,
		59,1,60,1,60,1,61,1,61,1,62,1,62,1,63,1,63,1,64,1,64,1,65,1,65,1,66,1,
		66,1,66,1,66,1,67,1,67,1,67,1,67,1,68,1,68,1,69,1,69,1,70,1,70,1,70,1,
		70,1,71,1,71,1,71,1,71,1,72,1,72,1,72,1,73,1,73,1,73,1,73,1,74,1,74,1,
		74,1,74,3,74,673,8,74,1,75,1,75,1,75,1,75,1,75,1,75,5,75,681,8,75,10,75,
		12,75,684,9,75,1,75,1,75,1,75,3,75,689,8,75,1,76,1,76,1,76,1,77,1,77,1,
		77,1,78,1,78,1,79,1,79,1,80,1,80,5,80,703,8,80,10,80,12,80,706,9,80,1,
		80,5,80,709,8,80,10,80,12,80,712,9,80,1,81,1,81,5,81,716,8,81,10,81,12,
		81,719,9,81,1,81,5,81,722,8,81,10,81,12,81,725,9,81,3,81,727,8,81,1,81,
		1,81,1,81,5,81,732,8,81,10,81,12,81,735,9,81,1,81,5,81,738,8,81,10,81,
		12,81,741,9,81,1,81,3,81,744,8,81,1,81,3,81,747,8,81,1,81,1,81,5,81,751,
		8,81,10,81,12,81,754,9,81,1,81,5,81,757,8,81,10,81,12,81,760,9,81,1,81,
		1,81,1,81,3,81,765,8,81,3,81,767,8,81,3,81,769,8,81,1,82,1,82,1,82,1,83,
		1,83,1,83,3,83,777,8,83,1,83,1,83,1,84,1,84,1,84,5,84,784,8,84,10,84,12,
		84,787,9,84,1,84,1,84,1,85,1,85,1,85,1,85,1,85,1,85,1,85,5,85,798,8,85,
		10,85,12,85,801,9,85,1,85,1,85,1,86,4,86,806,8,86,11,86,12,86,807,1,86,
		1,86,1,87,1,87,1,88,1,88,1,89,1,89,3,89,818,8,89,1,89,1,89,5,89,822,8,
		89,10,89,12,89,825,9,89,1,89,5,89,828,8,89,10,89,12,89,831,9,89,1,90,1,
		90,1,90,3,90,836,8,90,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,
		1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,1,91,3,91,860,8,
		91,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,
		92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,3,92,887,8,92,
		1,93,1,93,1,93,3,93,892,8,93,1,94,1,94,3,94,896,8,94,1,95,1,95,1,96,1,
		96,3,96,902,8,96,1,96,5,96,905,8,96,10,96,12,96,908,9,96,1,97,1,97,5,97,
		912,8,97,10,97,12,97,915,9,97,1,98,1,98,5,98,919,8,98,10,98,12,98,922,
		9,98,1,99,1,99,1,99,1,99,3,99,928,8,99,1,100,1,100,1,100,1,100,1,100,1,
		100,1,100,3,100,937,8,100,1,101,1,101,3,101,941,8,101,1,102,1,102,3,102,
		945,8,102,1,103,1,103,1,103,3,103,950,8,103,1,104,1,104,3,104,954,8,104,
		1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,
		1,105,1,105,1,105,1,105,1,105,1,105,1,105,1,105,3,105,976,8,105,1,106,
		3,106,979,8,106,1,107,1,107,1,108,1,108,1,109,1,109,1,110,1,110,1,111,
		1,111,1,112,1,112,1,113,1,113,1,114,1,114,1,115,1,115,1,116,1,116,1,117,
		1,117,1,118,3,118,1004,8,118,1,118,1,118,3,118,1008,8,118,1,119,4,119,
		1011,8,119,11,119,12,119,1012,1,119,1,119,0,0,120,1,1,3,2,5,3,7,4,9,5,
		11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,
		18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,
		30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,
		42,85,43,87,44,89,45,91,46,93,47,95,48,97,49,99,50,101,51,103,52,105,53,
		107,54,109,55,111,56,113,57,115,58,117,59,119,60,121,61,123,62,125,63,
		127,64,129,65,131,66,133,67,135,68,137,69,139,70,141,71,143,72,145,73,
		147,74,149,75,151,76,153,77,155,78,157,79,159,80,161,81,163,82,165,83,
		167,84,169,85,171,86,173,87,175,0,177,0,179,0,181,0,183,0,185,0,187,0,
		189,0,191,0,193,0,195,0,197,0,199,0,201,0,203,0,205,0,207,0,209,0,211,
		0,213,0,215,0,217,0,219,0,221,0,223,0,225,0,227,0,229,0,231,0,233,0,235,
		0,237,88,239,89,1,0,23,2,0,10,10,12,13,2,0,9,9,32,32,1,0,48,57,6,0,68,
		68,70,70,77,77,100,100,102,102,109,109,6,0,10,10,13,13,39,39,92,92,133,
		133,8232,8233,3,0,34,34,133,133,8232,8233,1,0,34,34,4,0,10,10,13,13,133,
		133,8232,8233,2,0,69,69,101,101,2,0,43,43,45,45,2,0,9,9,11,12,9,0,32,32,
		160,160,5760,5760,6158,6158,8192,8198,8200,8202,8239,8239,8287,8287,12288,
		12288,3,0,48,57,65,70,97,102,82,0,65,90,192,214,216,222,256,310,313,327,
		330,381,385,386,388,395,398,401,403,404,406,408,412,413,415,416,418,425,
		428,435,437,444,452,461,463,475,478,494,497,500,502,504,506,562,570,571,
		573,574,577,582,584,590,880,882,886,895,902,906,908,929,931,939,975,980,
		984,1006,1012,1015,1017,1018,1021,1071,1120,1152,1162,1229,1232,1326,1329,
		1366,4256,4293,4295,4301,7680,7828,7838,7934,7944,7951,7960,7965,7976,
		7983,7992,7999,8008,8013,8025,8031,8040,8047,8120,8123,8136,8139,8152,
		8155,8168,8172,8184,8187,8450,8455,8459,8461,8464,8466,8469,8477,8484,
		8493,8496,8499,8510,8511,8517,8579,11264,11310,11360,11364,11367,11376,
		11378,11381,11390,11392,11394,11490,11499,11501,11506,42560,42562,42604,
		42624,42650,42786,42798,42802,42862,42873,42886,42891,42893,42896,42898,
		42902,42925,42928,42929,65313,65338,81,0,97,122,181,246,248,255,257,375,
		378,384,387,389,392,402,405,411,414,417,419,421,424,429,432,436,438,447,
		454,460,462,499,501,505,507,569,572,578,583,659,661,687,881,883,887,893,
		912,974,976,977,981,983,985,1011,1013,1119,1121,1153,1163,1215,1218,1327,
		1377,1415,7424,7467,7531,7543,7545,7578,7681,7837,7839,7943,7952,7957,
		7968,7975,7984,7991,8000,8005,8016,8023,8032,8039,8048,8061,8064,8071,
		8080,8087,8096,8103,8112,8116,8118,8119,8126,8132,8134,8135,8144,8147,
		8150,8151,8160,8167,8178,8180,8182,8183,8458,8467,8495,8505,8508,8509,
		8518,8521,8526,8580,11312,11358,11361,11372,11377,11387,11393,11500,11502,
		11507,11520,11557,11559,11565,42561,42605,42625,42651,42787,42801,42803,
		42872,42874,42876,42879,42887,42892,42894,42897,42901,42903,42921,43002,
		43866,43876,43877,64256,64262,64275,64279,65345,65370,6,0,453,459,498,
		8079,8088,8095,8104,8111,8124,8140,8188,8188,33,0,688,705,710,721,736,
		740,748,750,884,890,1369,1600,1765,1766,2036,2037,2042,2074,2084,2088,
		2417,3654,3782,4348,6103,6211,6823,7293,7468,7530,7544,7615,8305,8319,
		8336,8348,11388,11389,11631,11823,12293,12341,12347,12542,40981,42237,
		42508,42623,42652,42653,42775,42783,42864,42888,43000,43001,43471,43494,
		43632,43741,43763,43764,43868,43871,65392,65439,234,0,170,186,443,451,
		660,1514,1520,1522,1568,1599,1601,1610,1646,1647,1649,1747,1749,1788,1791,
		1808,1810,1839,1869,1957,1969,2026,2048,2069,2112,2136,2208,2226,2308,
		2361,2365,2384,2392,2401,2418,2432,2437,2444,2447,2448,2451,2472,2474,
		2480,2482,2489,2493,2510,2524,2525,2527,2529,2544,2545,2565,2570,2575,
		2576,2579,2600,2602,2608,2610,2611,2613,2614,2616,2617,2649,2652,2654,
		2676,2693,2701,2703,2705,2707,2728,2730,2736,2738,2739,2741,2745,2749,
		2768,2784,2785,2821,2828,2831,2832,2835,2856,2858,2864,2866,2867,2869,
		2873,2877,2913,2929,2947,2949,2954,2958,2960,2962,2965,2969,2970,2972,
		2986,2990,3001,3024,3084,3086,3088,3090,3112,3114,3129,3133,3212,3214,
		3216,3218,3240,3242,3251,3253,3257,3261,3294,3296,3297,3313,3314,3333,
		3340,3342,3344,3346,3386,3389,3406,3424,3425,3450,3455,3461,3478,3482,
		3505,3507,3515,3517,3526,3585,3632,3634,3635,3648,3653,3713,3714,3716,
		3722,3725,3735,3737,3743,3745,3747,3749,3751,3754,3755,3757,3760,3762,
		3763,3773,3780,3804,3807,3840,3911,3913,3948,3976,3980,4096,4138,4159,
		4181,4186,4189,4193,4208,4213,4225,4238,4346,4349,4680,4682,4685,4688,
		4694,4696,4701,4704,4744,4746,4749,4752,4784,4786,4789,4792,4798,4800,
		4805,4808,4822,4824,4880,4882,4885,4888,4954,4992,5007,5024,5108,5121,
		5740,5743,5759,5761,5786,5792,5866,5873,5880,5888,5900,5902,5905,5920,
		5937,5952,5969,5984,5996,5998,6000,6016,6067,6108,6210,6212,6263,6272,
		6312,6314,6389,6400,6430,6480,6509,6512,6516,6528,6571,6593,6599,6656,
		6678,6688,6740,6917,6963,6981,6987,7043,7072,7086,7087,7098,7141,7168,
		7203,7245,7247,7258,7287,7401,7404,7406,7409,7413,7414,8501,8504,11568,
		11623,11648,11670,11680,11686,11688,11694,11696,11702,11704,11710,11712,
		11718,11720,11726,11728,11734,11736,11742,12294,12348,12353,12438,12447,
		12538,12543,12589,12593,12686,12704,12730,12784,12799,13312,19893,19968,
		40908,40960,40980,40982,42124,42192,42231,42240,42507,42512,42527,42538,
		42539,42606,42725,42999,43009,43011,43013,43015,43018,43020,43042,43072,
		43123,43138,43187,43250,43255,43259,43301,43312,43334,43360,43388,43396,
		43442,43488,43492,43495,43503,43514,43518,43520,43560,43584,43586,43588,
		43595,43616,43631,43633,43638,43642,43695,43697,43709,43712,43714,43739,
		43740,43744,43754,43762,43782,43785,43790,43793,43798,43808,43814,43816,
		43822,43968,44002,44032,55203,55216,55238,55243,55291,63744,64109,64112,
		64217,64285,64296,64298,64310,64312,64316,64318,64433,64467,64829,64848,
		64911,64914,64967,65008,65019,65136,65140,65142,65276,65382,65391,65393,
		65437,65440,65470,65474,65479,65482,65487,65490,65495,65498,65500,2,0,
		5870,5872,8544,8559,3,0,2307,2307,2366,2368,2377,2380,3,0,173,173,1536,
		1539,1757,1757,6,0,95,95,8255,8256,8276,8276,65075,65076,65101,65103,65343,
		65343,37,0,48,57,1632,1641,1776,1785,1984,1993,2406,2415,2534,2543,2662,
		2671,2790,2799,2918,2927,3046,3055,3174,3183,3302,3311,3430,3439,3558,
		3567,3664,3673,3792,3801,3872,3881,4160,4169,4240,4249,6112,6121,6160,
		6169,6470,6479,6608,6617,6784,6793,6800,6809,6992,7001,7088,7097,7232,
		7241,7248,7257,42528,42537,43216,43225,43264,43273,43472,43481,43504,43513,
		43600,43609,44016,44025,65296,65305,1062,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,
		0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,
		17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,
		0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,
		0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,
		1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,
		0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,
		1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,
		0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,
		1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,
		0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,
		0,0,0,115,1,0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,
		0,0,0,125,1,0,0,0,0,127,1,0,0,0,0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,
		0,0,0,135,1,0,0,0,0,137,1,0,0,0,0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,
		0,0,0,145,1,0,0,0,0,147,1,0,0,0,0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,
		0,0,0,155,1,0,0,0,0,157,1,0,0,0,0,159,1,0,0,0,0,161,1,0,0,0,0,163,1,0,
		0,0,0,165,1,0,0,0,0,167,1,0,0,0,0,169,1,0,0,0,0,171,1,0,0,0,0,173,1,0,
		0,0,0,237,1,0,0,0,0,239,1,0,0,0,1,241,1,0,0,0,3,246,1,0,0,0,5,251,1,0,
		0,0,7,262,1,0,0,0,9,264,1,0,0,0,11,267,1,0,0,0,13,276,1,0,0,0,15,281,1,
		0,0,0,17,287,1,0,0,0,19,293,1,0,0,0,21,302,1,0,0,0,23,314,1,0,0,0,25,320,
		1,0,0,0,27,328,1,0,0,0,29,333,1,0,0,0,31,337,1,0,0,0,33,349,1,0,0,0,35,
		353,1,0,0,0,37,361,1,0,0,0,39,370,1,0,0,0,41,377,1,0,0,0,43,380,1,0,0,
		0,45,390,1,0,0,0,47,393,1,0,0,0,49,402,1,0,0,0,51,409,1,0,0,0,53,413,1,
		0,0,0,55,418,1,0,0,0,57,426,1,0,0,0,59,434,1,0,0,0,61,444,1,0,0,0,63,453,
		1,0,0,0,65,460,1,0,0,0,67,467,1,0,0,0,69,472,1,0,0,0,71,477,1,0,0,0,73,
		484,1,0,0,0,75,489,1,0,0,0,77,492,1,0,0,0,79,496,1,0,0,0,81,502,1,0,0,
		0,83,506,1,0,0,0,85,512,1,0,0,0,87,526,1,0,0,0,89,568,1,0,0,0,91,570,1,
		0,0,0,93,576,1,0,0,0,95,581,1,0,0,0,97,592,1,0,0,0,99,597,1,0,0,0,101,
		599,1,0,0,0,103,602,1,0,0,0,105,604,1,0,0,0,107,606,1,0,0,0,109,608,1,
		0,0,0,111,610,1,0,0,0,113,612,1,0,0,0,115,614,1,0,0,0,117,617,1,0,0,0,
		119,619,1,0,0,0,121,629,1,0,0,0,123,631,1,0,0,0,125,633,1,0,0,0,127,635,
		1,0,0,0,129,637,1,0,0,0,131,639,1,0,0,0,133,641,1,0,0,0,135,645,1,0,0,
		0,137,649,1,0,0,0,139,651,1,0,0,0,141,653,1,0,0,0,143,657,1,0,0,0,145,
		661,1,0,0,0,147,664,1,0,0,0,149,672,1,0,0,0,151,688,1,0,0,0,153,690,1,
		0,0,0,155,693,1,0,0,0,157,696,1,0,0,0,159,698,1,0,0,0,161,700,1,0,0,0,
		163,768,1,0,0,0,165,770,1,0,0,0,167,773,1,0,0,0,169,780,1,0,0,0,171,790,
		1,0,0,0,173,805,1,0,0,0,175,811,1,0,0,0,177,813,1,0,0,0,179,815,1,0,0,
		0,181,835,1,0,0,0,183,859,1,0,0,0,185,886,1,0,0,0,187,891,1,0,0,0,189,
		895,1,0,0,0,191,897,1,0,0,0,193,901,1,0,0,0,195,909,1,0,0,0,197,916,1,
		0,0,0,199,927,1,0,0,0,201,936,1,0,0,0,203,940,1,0,0,0,205,944,1,0,0,0,
		207,949,1,0,0,0,209,953,1,0,0,0,211,975,1,0,0,0,213,978,1,0,0,0,215,980,
		1,0,0,0,217,982,1,0,0,0,219,984,1,0,0,0,221,986,1,0,0,0,223,988,1,0,0,
		0,225,990,1,0,0,0,227,992,1,0,0,0,229,994,1,0,0,0,231,996,1,0,0,0,233,
		998,1,0,0,0,235,1000,1,0,0,0,237,1007,1,0,0,0,239,1010,1,0,0,0,241,242,
		5,239,0,0,242,243,5,187,0,0,243,244,5,191,0,0,244,2,1,0,0,0,245,247,7,
		0,0,0,246,245,1,0,0,0,247,248,1,0,0,0,248,246,1,0,0,0,248,249,1,0,0,0,
		249,4,1,0,0,0,250,252,3,3,1,0,251,250,1,0,0,0,251,252,1,0,0,0,252,253,
		1,0,0,0,253,257,3,7,3,0,254,256,3,175,87,0,255,254,1,0,0,0,256,259,1,0,
		0,0,257,255,1,0,0,0,257,258,1,0,0,0,258,260,1,0,0,0,259,257,1,0,0,0,260,
		261,6,2,0,0,261,6,1,0,0,0,262,263,5,35,0,0,263,8,1,0,0,0,264,265,5,97,
		0,0,265,266,5,115,0,0,266,10,1,0,0,0,267,268,5,97,0,0,268,269,5,98,0,0,
		269,270,5,115,0,0,270,271,5,116,0,0,271,272,5,114,0,0,272,273,5,97,0,0,
		273,274,5,99,0,0,274,275,5,116,0,0,275,12,1,0,0,0,276,277,5,99,0,0,277,
		278,5,97,0,0,278,279,5,115,0,0,279,280,5,101,0,0,280,14,1,0,0,0,281,282,
		5,99,0,0,282,283,5,97,0,0,283,284,5,116,0,0,284,285,5,99,0,0,285,286,5,
		104,0,0,286,16,1,0,0,0,287,288,5,99,0,0,288,289,5,108,0,0,289,290,5,97,
		0,0,290,291,5,115,0,0,291,292,5,115,0,0,292,18,1,0,0,0,293,294,5,99,0,
		0,294,295,5,111,0,0,295,296,5,110,0,0,296,297,5,115,0,0,297,298,5,116,
		0,0,298,299,5,97,0,0,299,300,5,110,0,0,300,301,5,116,0,0,301,20,1,0,0,
		0,302,303,5,99,0,0,303,304,5,111,0,0,304,305,5,110,0,0,305,306,5,115,0,
		0,306,307,5,116,0,0,307,308,5,114,0,0,308,309,5,117,0,0,309,310,5,99,0,
		0,310,311,5,116,0,0,311,312,5,111,0,0,312,313,5,114,0,0,313,22,1,0,0,0,
		314,315,5,99,0,0,315,316,5,117,0,0,316,317,5,114,0,0,317,318,5,114,0,0,
		318,319,5,121,0,0,319,24,1,0,0,0,320,321,5,100,0,0,321,322,5,101,0,0,322,
		323,5,102,0,0,323,324,5,97,0,0,324,325,5,117,0,0,325,326,5,108,0,0,326,
		327,5,116,0,0,327,26,1,0,0,0,328,329,5,101,0,0,329,330,5,108,0,0,330,331,
		5,115,0,0,331,332,5,101,0,0,332,28,1,0,0,0,333,334,5,101,0,0,334,335,5,
		110,0,0,335,336,5,100,0,0,336,30,1,0,0,0,337,338,5,101,0,0,338,339,5,110,
		0,0,339,340,5,117,0,0,340,341,5,109,0,0,341,342,5,101,0,0,342,343,5,114,
		0,0,343,344,5,97,0,0,344,345,5,116,0,0,345,346,5,105,0,0,346,347,5,111,
		0,0,347,348,5,110,0,0,348,32,1,0,0,0,349,350,5,102,0,0,350,351,5,111,0,
		0,351,352,5,114,0,0,352,34,1,0,0,0,353,354,5,102,0,0,354,355,5,111,0,0,
		355,356,5,114,0,0,356,357,5,101,0,0,357,358,5,97,0,0,358,359,5,99,0,0,
		359,360,5,104,0,0,360,36,1,0,0,0,361,362,5,102,0,0,362,363,5,117,0,0,363,
		364,5,110,0,0,364,365,5,99,0,0,365,366,5,116,0,0,366,367,5,105,0,0,367,
		368,5,111,0,0,368,369,5,110,0,0,369,38,1,0,0,0,370,371,5,103,0,0,371,372,
		5,108,0,0,372,373,5,111,0,0,373,374,5,98,0,0,374,375,5,97,0,0,375,376,
		5,108,0,0,376,40,1,0,0,0,377,378,5,105,0,0,378,379,5,102,0,0,379,42,1,
		0,0,0,380,381,5,105,0,0,381,382,5,109,0,0,382,383,5,109,0,0,383,384,5,
		117,0,0,384,385,5,116,0,0,385,386,5,97,0,0,386,387,5,98,0,0,387,388,5,
		108,0,0,388,389,5,101,0,0,389,44,1,0,0,0,390,391,5,105,0,0,391,392,5,110,
		0,0,392,46,1,0,0,0,393,394,5,105,0,0,394,395,5,110,0,0,395,396,5,104,0,
		0,396,397,5,101,0,0,397,398,5,114,0,0,398,399,5,105,0,0,399,400,5,116,
		0,0,400,401,5,115,0,0,401,48,1,0,0,0,402,403,5,108,0,0,403,404,5,97,0,
		0,404,405,5,109,0,0,405,406,5,98,0,0,406,407,5,100,0,0,407,408,5,97,0,
		0,408,50,1,0,0,0,409,410,5,108,0,0,410,411,5,101,0,0,411,412,5,116,0,0,
		412,52,1,0,0,0,413,414,5,109,0,0,414,415,5,97,0,0,415,416,5,105,0,0,416,
		417,5,110,0,0,417,54,1,0,0,0,418,419,5,112,0,0,419,420,5,97,0,0,420,421,
		5,114,0,0,421,422,5,116,0,0,422,423,5,105,0,0,423,424,5,97,0,0,424,425,
		5,108,0,0,425,56,1,0,0,0,426,427,5,112,0,0,427,428,5,114,0,0,428,429,5,
		105,0,0,429,430,5,118,0,0,430,431,5,97,0,0,431,432,5,116,0,0,432,433,5,
		101,0,0,433,58,1,0,0,0,434,435,5,112,0,0,435,436,5,114,0,0,436,437,5,111,
		0,0,437,438,5,99,0,0,438,439,5,101,0,0,439,440,5,100,0,0,440,441,5,117,
		0,0,441,442,5,114,0,0,442,443,5,101,0,0,443,60,1,0,0,0,444,445,5,112,0,
		0,445,446,5,114,0,0,446,447,5,111,0,0,447,448,5,112,0,0,448,449,5,101,
		0,0,449,450,5,114,0,0,450,451,5,116,0,0,451,452,5,121,0,0,452,62,1,0,0,
		0,453,454,5,114,0,0,454,455,5,101,0,0,455,456,5,112,0,0,456,457,5,101,
		0,0,457,458,5,97,0,0,458,459,5,116,0,0,459,64,1,0,0,0,460,461,5,114,0,
		0,461,462,5,101,0,0,462,463,5,116,0,0,463,464,5,117,0,0,464,465,5,114,
		0,0,465,466,5,110,0,0,466,66,1,0,0,0,467,468,5,115,0,0,468,469,5,101,0,
		0,469,470,5,108,0,0,470,471,5,102,0,0,471,68,1,0,0,0,472,473,5,115,0,0,
		473,474,5,116,0,0,474,475,5,101,0,0,475,476,5,112,0,0,476,70,1,0,0,0,477,
		478,5,115,0,0,478,479,5,119,0,0,479,480,5,105,0,0,480,481,5,116,0,0,481,
		482,5,99,0,0,482,483,5,104,0,0,483,72,1,0,0,0,484,485,5,116,0,0,485,486,
		5,104,0,0,486,487,5,101,0,0,487,488,5,110,0,0,488,74,1,0,0,0,489,490,5,
		116,0,0,490,491,5,111,0,0,491,76,1,0,0,0,492,493,5,116,0,0,493,494,5,114,
		0,0,494,495,5,121,0,0,495,78,1,0,0,0,496,497,5,117,0,0,497,498,5,110,0,
		0,498,499,5,116,0,0,499,500,5,105,0,0,500,501,5,108,0,0,501,80,1,0,0,0,
		502,503,5,118,0,0,503,504,5,97,0,0,504,505,5,114,0,0,505,82,1,0,0,0,506,
		507,5,119,0,0,507,508,5,104,0,0,508,509,5,105,0,0,509,510,5,108,0,0,510,
		511,5,101,0,0,511,84,1,0,0,0,512,513,5,119,0,0,513,514,5,105,0,0,514,515,
		5,116,0,0,515,516,5,104,0,0,516,86,1,0,0,0,517,518,5,116,0,0,518,519,5,
		114,0,0,519,520,5,117,0,0,520,527,5,101,0,0,521,522,5,102,0,0,522,523,
		5,97,0,0,523,524,5,108,0,0,524,525,5,115,0,0,525,527,5,101,0,0,526,517,
		1,0,0,0,526,521,1,0,0,0,527,88,1,0,0,0,528,529,5,86,0,0,529,530,5,97,0,
		0,530,531,5,108,0,0,531,532,5,117,0,0,532,569,5,101,0,0,533,534,5,73,0,
		0,534,535,5,110,0,0,535,569,5,116,0,0,536,537,5,70,0,0,537,538,5,108,0,
		0,538,539,5,111,0,0,539,540,5,97,0,0,540,569,5,116,0,0,541,542,5,68,0,
		0,542,543,5,101,0,0,543,544,5,99,0,0,544,545,5,105,0,0,545,546,5,109,0,
		0,546,547,5,97,0,0,547,569,5,108,0,0,548,549,5,78,0,0,549,550,5,117,0,
		0,550,551,5,109,0,0,551,552,5,98,0,0,552,553,5,101,0,0,553,569,5,114,0,
		0,554,555,5,67,0,0,555,556,5,104,0,0,556,557,5,97,0,0,557,569,5,114,0,
		0,558,559,5,83,0,0,559,560,5,116,0,0,560,561,5,114,0,0,561,562,5,105,0,
		0,562,563,5,110,0,0,563,569,5,103,0,0,564,565,5,66,0,0,565,566,5,111,0,
		0,566,567,5,111,0,0,567,569,5,108,0,0,568,528,1,0,0,0,568,533,1,0,0,0,
		568,536,1,0,0,0,568,541,1,0,0,0,568,548,1,0,0,0,568,554,1,0,0,0,568,558,
		1,0,0,0,568,564,1,0,0,0,569,90,1,0,0,0,570,571,5,65,0,0,571,572,5,114,
		0,0,572,573,5,114,0,0,573,574,5,97,0,0,574,575,5,121,0,0,575,92,1,0,0,
		0,576,577,5,76,0,0,577,578,5,105,0,0,578,579,5,115,0,0,579,580,5,116,0,
		0,580,94,1,0,0,0,581,582,5,68,0,0,582,583,5,105,0,0,583,584,5,99,0,0,584,
		585,5,116,0,0,585,586,5,105,0,0,586,587,5,111,0,0,587,588,5,110,0,0,588,
		589,5,97,0,0,589,590,5,114,0,0,590,591,5,121,0,0,591,96,1,0,0,0,592,593,
		5,73,0,0,593,594,5,116,0,0,594,595,5,101,0,0,595,596,5,114,0,0,596,98,
		1,0,0,0,597,598,5,61,0,0,598,100,1,0,0,0,599,600,5,45,0,0,600,601,5,62,
		0,0,601,102,1,0,0,0,602,603,5,123,0,0,603,104,1,0,0,0,604,605,5,125,0,
		0,605,106,1,0,0,0,606,607,5,91,0,0,607,108,1,0,0,0,608,609,5,93,0,0,609,
		110,1,0,0,0,610,611,5,40,0,0,611,112,1,0,0,0,612,613,5,41,0,0,613,114,
		1,0,0,0,614,615,5,46,0,0,615,616,5,46,0,0,616,116,1,0,0,0,617,618,5,46,
		0,0,618,118,1,0,0,0,619,623,5,44,0,0,620,622,7,1,0,0,621,620,1,0,0,0,622,
		625,1,0,0,0,623,621,1,0,0,0,623,624,1,0,0,0,624,627,1,0,0,0,625,623,1,
		0,0,0,626,628,3,3,1,0,627,626,1,0,0,0,627,628,1,0,0,0,628,120,1,0,0,0,
		629,630,5,58,0,0,630,122,1,0,0,0,631,632,5,43,0,0,632,124,1,0,0,0,633,
		634,5,45,0,0,634,126,1,0,0,0,635,636,5,42,0,0,636,128,1,0,0,0,637,638,
		5,47,0,0,638,130,1,0,0,0,639,640,5,94,0,0,640,132,1,0,0,0,641,642,5,109,
		0,0,642,643,5,111,0,0,643,644,5,100,0,0,644,134,1,0,0,0,645,646,5,100,
		0,0,646,647,5,105,0,0,647,648,5,118,0,0,648,136,1,0,0,0,649,650,5,60,0,
		0,650,138,1,0,0,0,651,652,5,62,0,0,652,140,1,0,0,0,653,654,5,97,0,0,654,
		655,5,110,0,0,655,656,5,100,0,0,656,142,1,0,0,0,657,658,5,110,0,0,658,
		659,5,111,0,0,659,660,5,116,0,0,660,144,1,0,0,0,661,662,5,111,0,0,662,
		663,5,114,0,0,663,146,1,0,0,0,664,665,5,120,0,0,665,666,5,111,0,0,666,
		667,5,114,0,0,667,148,1,0,0,0,668,669,5,61,0,0,669,673,5,61,0,0,670,671,
		5,105,0,0,671,673,5,115,0,0,672,668,1,0,0,0,672,670,1,0,0,0,673,150,1,
		0,0,0,674,675,5,60,0,0,675,689,5,62,0,0,676,677,5,105,0,0,677,678,5,115,
		0,0,678,682,1,0,0,0,679,681,7,1,0,0,680,679,1,0,0,0,681,684,1,0,0,0,682,
		680,1,0,0,0,682,683,1,0,0,0,683,685,1,0,0,0,684,682,1,0,0,0,685,686,5,
		110,0,0,686,687,5,111,0,0,687,689,5,116,0,0,688,674,1,0,0,0,688,676,1,
		0,0,0,689,152,1,0,0,0,690,691,5,60,0,0,691,692,5,61,0,0,692,154,1,0,0,
		0,693,694,5,62,0,0,694,695,5,61,0,0,695,156,1,0,0,0,696,697,3,197,98,0,
		697,158,1,0,0,0,698,699,3,195,97,0,699,160,1,0,0,0,700,710,7,2,0,0,701,
		703,5,95,0,0,702,701,1,0,0,0,703,706,1,0,0,0,704,702,1,0,0,0,704,705,1,
		0,0,0,705,707,1,0,0,0,706,704,1,0,0,0,707,709,7,2,0,0,708,704,1,0,0,0,
		709,712,1,0,0,0,710,708,1,0,0,0,710,711,1,0,0,0,711,162,1,0,0,0,712,710,
		1,0,0,0,713,723,7,2,0,0,714,716,5,95,0,0,715,714,1,0,0,0,716,719,1,0,0,
		0,717,715,1,0,0,0,717,718,1,0,0,0,718,720,1,0,0,0,719,717,1,0,0,0,720,
		722,7,2,0,0,721,717,1,0,0,0,722,725,1,0,0,0,723,721,1,0,0,0,723,724,1,
		0,0,0,724,727,1,0,0,0,725,723,1,0,0,0,726,713,1,0,0,0,726,727,1,0,0,0,
		727,728,1,0,0,0,728,729,5,46,0,0,729,739,7,2,0,0,730,732,5,95,0,0,731,
		730,1,0,0,0,732,735,1,0,0,0,733,731,1,0,0,0,733,734,1,0,0,0,734,736,1,
		0,0,0,735,733,1,0,0,0,736,738,7,2,0,0,737,733,1,0,0,0,738,741,1,0,0,0,
		739,737,1,0,0,0,739,740,1,0,0,0,740,743,1,0,0,0,741,739,1,0,0,0,742,744,
		3,179,89,0,743,742,1,0,0,0,743,744,1,0,0,0,744,746,1,0,0,0,745,747,7,3,
		0,0,746,745,1,0,0,0,746,747,1,0,0,0,747,769,1,0,0,0,748,758,7,2,0,0,749,
		751,5,95,0,0,750,749,1,0,0,0,751,754,1,0,0,0,752,750,1,0,0,0,752,753,1,
		0,0,0,753,755,1,0,0,0,754,752,1,0,0,0,755,757,7,2,0,0,756,752,1,0,0,0,
		757,760,1,0,0,0,758,756,1,0,0,0,758,759,1,0,0,0,759,766,1,0,0,0,760,758,
		1,0,0,0,761,767,7,3,0,0,762,764,3,179,89,0,763,765,7,3,0,0,764,763,1,0,
		0,0,764,765,1,0,0,0,765,767,1,0,0,0,766,761,1,0,0,0,766,762,1,0,0,0,767,
		769,1,0,0,0,768,726,1,0,0,0,768,748,1,0,0,0,769,164,1,0,0,0,770,771,3,
		163,81,0,771,772,5,68,0,0,772,166,1,0,0,0,773,776,5,39,0,0,774,777,8,4,
		0,0,775,777,3,181,90,0,776,774,1,0,0,0,776,775,1,0,0,0,777,778,1,0,0,0,
		778,779,5,39,0,0,779,168,1,0,0,0,780,785,5,34,0,0,781,784,8,5,0,0,782,
		784,3,181,90,0,783,781,1,0,0,0,783,782,1,0,0,0,784,787,1,0,0,0,785,783,
		1,0,0,0,785,786,1,0,0,0,786,788,1,0,0,0,787,785,1,0,0,0,788,789,5,34,0,
		0,789,170,1,0,0,0,790,791,5,34,0,0,791,792,5,34,0,0,792,793,5,34,0,0,793,
		799,1,0,0,0,794,798,8,6,0,0,795,796,5,34,0,0,796,798,5,34,0,0,797,794,
		1,0,0,0,797,795,1,0,0,0,798,801,1,0,0,0,799,797,1,0,0,0,799,800,1,0,0,
		0,800,802,1,0,0,0,801,799,1,0,0,0,802,803,5,34,0,0,803,172,1,0,0,0,804,
		806,3,189,94,0,805,804,1,0,0,0,806,807,1,0,0,0,807,805,1,0,0,0,807,808,
		1,0,0,0,808,809,1,0,0,0,809,810,6,86,0,0,810,174,1,0,0,0,811,812,8,7,0,
		0,812,176,1,0,0,0,813,814,7,7,0,0,814,178,1,0,0,0,815,817,7,8,0,0,816,
		818,7,9,0,0,817,816,1,0,0,0,817,818,1,0,0,0,818,819,1,0,0,0,819,829,7,
		2,0,0,820,822,5,95,0,0,821,820,1,0,0,0,822,825,1,0,0,0,823,821,1,0,0,0,
		823,824,1,0,0,0,824,826,1,0,0,0,825,823,1,0,0,0,826,828,7,2,0,0,827,823,
		1,0,0,0,828,831,1,0,0,0,829,827,1,0,0,0,829,830,1,0,0,0,830,180,1,0,0,
		0,831,829,1,0,0,0,832,836,3,183,91,0,833,836,3,185,92,0,834,836,3,211,
		105,0,835,832,1,0,0,0,835,833,1,0,0,0,835,834,1,0,0,0,836,182,1,0,0,0,
		837,838,5,92,0,0,838,860,5,39,0,0,839,840,5,92,0,0,840,860,5,34,0,0,841,
		842,5,92,0,0,842,860,5,92,0,0,843,844,5,92,0,0,844,860,5,48,0,0,845,846,
		5,92,0,0,846,860,5,97,0,0,847,848,5,92,0,0,848,860,5,98,0,0,849,850,5,
		92,0,0,850,860,5,102,0,0,851,852,5,92,0,0,852,860,5,110,0,0,853,854,5,
		92,0,0,854,860,5,114,0,0,855,856,5,92,0,0,856,860,5,116,0,0,857,858,5,
		92,0,0,858,860,5,118,0,0,859,837,1,0,0,0,859,839,1,0,0,0,859,841,1,0,0,
		0,859,843,1,0,0,0,859,845,1,0,0,0,859,847,1,0,0,0,859,849,1,0,0,0,859,
		851,1,0,0,0,859,853,1,0,0,0,859,855,1,0,0,0,859,857,1,0,0,0,860,184,1,
		0,0,0,861,862,5,92,0,0,862,863,5,120,0,0,863,864,1,0,0,0,864,887,3,213,
		106,0,865,866,5,92,0,0,866,867,5,120,0,0,867,868,1,0,0,0,868,869,3,213,
		106,0,869,870,3,213,106,0,870,887,1,0,0,0,871,872,5,92,0,0,872,873,5,120,
		0,0,873,874,1,0,0,0,874,875,3,213,106,0,875,876,3,213,106,0,876,877,3,
		213,106,0,877,887,1,0,0,0,878,879,5,92,0,0,879,880,5,120,0,0,880,881,1,
		0,0,0,881,882,3,213,106,0,882,883,3,213,106,0,883,884,3,213,106,0,884,
		885,3,213,106,0,885,887,1,0,0,0,886,861,1,0,0,0,886,865,1,0,0,0,886,871,
		1,0,0,0,886,878,1,0,0,0,887,186,1,0,0,0,888,889,5,13,0,0,889,892,5,10,
		0,0,890,892,7,7,0,0,891,888,1,0,0,0,891,890,1,0,0,0,892,188,1,0,0,0,893,
		896,3,191,95,0,894,896,7,10,0,0,895,893,1,0,0,0,895,894,1,0,0,0,896,190,
		1,0,0,0,897,898,7,11,0,0,898,192,1,0,0,0,899,902,3,217,108,0,900,902,3,
		215,107,0,901,899,1,0,0,0,901,900,1,0,0,0,902,906,1,0,0,0,903,905,3,199,
		99,0,904,903,1,0,0,0,905,908,1,0,0,0,906,904,1,0,0,0,906,907,1,0,0,0,907,
		194,1,0,0,0,908,906,1,0,0,0,909,913,3,217,108,0,910,912,3,199,99,0,911,
		910,1,0,0,0,912,915,1,0,0,0,913,911,1,0,0,0,913,914,1,0,0,0,914,196,1,
		0,0,0,915,913,1,0,0,0,916,920,3,215,107,0,917,919,3,199,99,0,918,917,1,
		0,0,0,919,922,1,0,0,0,920,918,1,0,0,0,920,921,1,0,0,0,921,198,1,0,0,0,
		922,920,1,0,0,0,923,928,3,215,107,0,924,928,3,217,108,0,925,928,3,203,
		101,0,926,928,5,95,0,0,927,923,1,0,0,0,927,924,1,0,0,0,927,925,1,0,0,0,
		927,926,1,0,0,0,928,200,1,0,0,0,929,937,3,215,107,0,930,937,3,217,108,
		0,931,937,3,219,109,0,932,937,3,221,110,0,933,937,3,223,111,0,934,937,
		3,225,112,0,935,937,3,211,105,0,936,929,1,0,0,0,936,930,1,0,0,0,936,931,
		1,0,0,0,936,932,1,0,0,0,936,933,1,0,0,0,936,934,1,0,0,0,936,935,1,0,0,
		0,937,202,1,0,0,0,938,941,3,235,117,0,939,941,3,211,105,0,940,938,1,0,
		0,0,940,939,1,0,0,0,941,204,1,0,0,0,942,945,3,233,116,0,943,945,3,211,
		105,0,944,942,1,0,0,0,944,943,1,0,0,0,945,206,1,0,0,0,946,950,3,227,113,
		0,947,950,3,229,114,0,948,950,3,211,105,0,949,946,1,0,0,0,949,947,1,0,
		0,0,949,948,1,0,0,0,950,208,1,0,0,0,951,954,3,231,115,0,952,954,3,211,
		105,0,953,951,1,0,0,0,953,952,1,0,0,0,954,210,1,0,0,0,955,956,5,92,0,0,
		956,957,5,117,0,0,957,958,1,0,0,0,958,959,3,213,106,0,959,960,3,213,106,
		0,960,961,3,213,106,0,961,962,3,213,106,0,962,976,1,0,0,0,963,964,5,92,
		0,0,964,965,5,85,0,0,965,966,1,0,0,0,966,967,3,213,106,0,967,968,3,213,
		106,0,968,969,3,213,106,0,969,970,3,213,106,0,970,971,3,213,106,0,971,
		972,3,213,106,0,972,973,3,213,106,0,973,974,3,213,106,0,974,976,1,0,0,
		0,975,955,1,0,0,0,975,963,1,0,0,0,976,212,1,0,0,0,977,979,7,12,0,0,978,
		977,1,0,0,0,979,214,1,0,0,0,980,981,7,13,0,0,981,216,1,0,0,0,982,983,7,
		14,0,0,983,218,1,0,0,0,984,985,7,15,0,0,985,220,1,0,0,0,986,987,7,16,0,
		0,987,222,1,0,0,0,988,989,7,17,0,0,989,224,1,0,0,0,990,991,7,18,0,0,991,
		226,1,0,0,0,992,993,2,768,784,0,993,228,1,0,0,0,994,995,7,19,0,0,995,230,
		1,0,0,0,996,997,7,20,0,0,997,232,1,0,0,0,998,999,7,21,0,0,999,234,1,0,
		0,0,1000,1001,7,22,0,0,1001,236,1,0,0,0,1002,1004,5,13,0,0,1003,1002,1,
		0,0,0,1003,1004,1,0,0,0,1004,1005,1,0,0,0,1005,1008,5,10,0,0,1006,1008,
		5,13,0,0,1007,1003,1,0,0,0,1007,1006,1,0,0,0,1008,238,1,0,0,0,1009,1011,
		7,1,0,0,1010,1009,1,0,0,0,1011,1012,1,0,0,0,1012,1010,1,0,0,0,1012,1013,
		1,0,0,0,1013,1014,1,0,0,0,1014,1015,6,119,0,0,1015,240,1,0,0,0,54,0,248,
		251,257,526,568,623,627,672,682,688,704,710,717,723,726,733,739,743,746,
		752,758,764,766,768,776,783,785,797,799,807,817,823,829,835,859,886,891,
		895,901,906,913,920,927,936,940,944,949,953,975,978,1003,1007,1012,1,6,
		0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
