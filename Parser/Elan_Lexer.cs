//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c://Elan//Repository//Parser//Elan_Lexer.g4 by ANTLR 4.13.1

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
public partial class Elan_Lexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		NL=1, SINGLE_LINE_COMMENT=2, COMMENT_MARKER=3, AS=4, ABSTRACT=5, CALL=6, 
		CASE=7, CATCH=8, CLASS=9, CONSTANT=10, CONSTRUCTOR=11, CURRY=12, DEFAULT=13, 
		ELSE=14, END=15, ENUM=16, FOR=17, FOREACH=18, FUNCTION=19, GLOBAL=20, 
		IF=21, IMMUTABLE=22, IN=23, INHERITS=24, LAMBDA=25, LET=26, MAIN=27, NEW=28, 
		OF=29, OUT=30, PARTIAL=31, PRINT=32, PRIVATE=33, PROCEDURE=34, PROPERTY=35, 
		REPEAT=36, RETURN=37, SELF=38, SET=39, STEP=40, SWITCH=41, SYSTEM=42, 
		TEST=43, THEN=44, THROW=45, TO=46, TRY=47, UNTIL=48, VAR=49, WHILE=50, 
		WITH=51, BOOL_VALUE=52, VALUE_TYPE=53, ARRAY=54, LIST=55, DICTIONARY=56, 
		ITERABLE=57, ASSIGN=58, ARROW=59, OPEN_BRACE=60, CLOSE_BRACE=61, OPEN_SQ_BRACKET=62, 
		CLOSE_SQ_BRACKET=63, OPEN_BRACKET=64, CLOSE_BRACKET=65, DOUBLE_DOT=66, 
		DOT=67, COMMA=68, COLON=69, PLUS=70, MINUS=71, MULT=72, DIVIDE=73, POWER=74, 
		MOD=75, INT_DIV=76, LT=77, GT=78, OP_AND=79, OP_NOT=80, OP_OR=81, OP_XOR=82, 
		OP_EQ=83, OP_NE=84, OP_LE=85, OP_GE=86, TYPENAME=87, IDENTIFIER=88, LITERAL_INTEGER=89, 
		LITERAL_FLOAT=90, LITERAL_CHAR=91, LITERAL_STRING=92, WHITESPACES=93, 
		NEWLINE=94, WS=95;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "AS", "ABSTRACT", "CALL", 
		"CASE", "CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", "DEFAULT", 
		"ELSE", "END", "ENUM", "FOR", "FOREACH", "FUNCTION", "GLOBAL", "IF", "IMMUTABLE", 
		"IN", "INHERITS", "LAMBDA", "LET", "MAIN", "NEW", "OF", "OUT", "PARTIAL", 
		"PRINT", "PRIVATE", "PROCEDURE", "PROPERTY", "REPEAT", "RETURN", "SELF", 
		"SET", "STEP", "SWITCH", "SYSTEM", "TEST", "THEN", "THROW", "TO", "TRY", 
		"UNTIL", "VAR", "WHILE", "WITH", "BOOL_VALUE", "VALUE_TYPE", "ARRAY", 
		"LIST", "DICTIONARY", "ITERABLE", "ASSIGN", "ARROW", "OPEN_BRACE", "CLOSE_BRACE", 
		"OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", "OPEN_BRACKET", "CLOSE_BRACKET", 
		"DOUBLE_DOT", "DOT", "COMMA", "COLON", "PLUS", "MINUS", "MULT", "DIVIDE", 
		"POWER", "MOD", "INT_DIV", "LT", "GT", "OP_AND", "OP_NOT", "OP_OR", "OP_XOR", 
		"OP_EQ", "OP_NE", "OP_LE", "OP_GE", "TYPENAME", "IDENTIFIER", "LITERAL_INTEGER", 
		"LITERAL_FLOAT", "LITERAL_CHAR", "LITERAL_STRING", "WHITESPACES", "InputCharacter", 
		"NewLineCharacter", "ExponentPart", "CommonCharacter", "SimpleEscapeSequence", 
		"HexEscapeSequence", "NewLine", "Whitespace", "UnicodeClassZS", "IdentifierStartingUCorLC", 
		"IdentifierStartingLC", "IdentifierStartingUC", "IdentifierPartCharacter", 
		"LetterCharacter", "DecimalDigitCharacter", "ConnectingCharacter", "FormattingCharacter", 
		"UnicodeEscapeSequence", "HexDigit", "UnicodeClassLU", "UnicodeClassLL", 
		"UnicodeClassND", "NEWLINE", "WS"
	};


	public Elan_Lexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public Elan_Lexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, "'#'", "'as'", "'abstract'", "'call'", "'case'", "'catch'", 
		"'class'", "'constant'", "'constructor'", "'curry'", "'default'", "'else'", 
		"'end'", "'enum'", "'for'", "'foreach'", "'function'", "'global'", "'if'", 
		"'immutable'", "'in'", "'inherits'", "'lambda'", "'let'", "'main'", "'new'", 
		"'of'", "'out'", "'partial'", "'print'", "'private'", "'procedure'", "'property'", 
		"'repeat'", "'return'", "'self'", "'set'", "'step'", "'switch'", "'system'", 
		"'test'", "'then'", "'throw'", "'to'", "'try'", "'until'", "'var'", "'while'", 
		"'with'", null, null, "'Array'", "'List'", "'Dictionary'", "'Iter'", "'='", 
		"'->'", "'{'", "'}'", "'['", "']'", "'('", "')'", "'..'", "'.'", "','", 
		"':'", "'+'", "'-'", "'*'", "'/'", "'^'", "'mod'", "'div'", "'<'", "'>'", 
		"'and'", "'not'", "'or'", "'xor'", "'is'", null, "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "AS", "ABSTRACT", 
		"CALL", "CASE", "CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", 
		"DEFAULT", "ELSE", "END", "ENUM", "FOR", "FOREACH", "FUNCTION", "GLOBAL", 
		"IF", "IMMUTABLE", "IN", "INHERITS", "LAMBDA", "LET", "MAIN", "NEW", "OF", 
		"OUT", "PARTIAL", "PRINT", "PRIVATE", "PROCEDURE", "PROPERTY", "REPEAT", 
		"RETURN", "SELF", "SET", "STEP", "SWITCH", "SYSTEM", "TEST", "THEN", "THROW", 
		"TO", "TRY", "UNTIL", "VAR", "WHILE", "WITH", "BOOL_VALUE", "VALUE_TYPE", 
		"ARRAY", "LIST", "DICTIONARY", "ITERABLE", "ASSIGN", "ARROW", "OPEN_BRACE", 
		"CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", "OPEN_BRACKET", 
		"CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", "PLUS", "MINUS", 
		"MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", "OP_AND", "OP_NOT", 
		"OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", "TYPENAME", "IDENTIFIER", 
		"LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_CHAR", "LITERAL_STRING", 
		"WHITESPACES", "NEWLINE", "WS"
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

	public override string GrammarFileName { get { return "Elan_Lexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static Elan_Lexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,95,898,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		7,116,1,0,4,0,237,8,0,11,0,12,0,238,1,1,3,1,242,8,1,1,1,1,1,5,1,246,8,
		1,10,1,12,1,249,9,1,1,1,1,1,1,2,1,2,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,1,
		4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,
		1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,
		10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,
		11,1,11,1,11,1,11,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,13,1,13,1,
		13,1,13,1,13,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,16,1,16,1,
		16,1,16,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,
		18,1,18,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,
		20,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,
		23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,
		24,1,24,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,
		27,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,30,1,30,1,30,1,30,1,30,1,30,1,
		30,1,30,1,31,1,31,1,31,1,31,1,31,1,31,1,32,1,32,1,32,1,32,1,32,1,32,1,
		32,1,32,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,
		34,1,34,1,34,1,34,1,34,1,34,1,34,1,35,1,35,1,35,1,35,1,35,1,35,1,35,1,
		36,1,36,1,36,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,37,1,38,1,38,1,
		38,1,38,1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,
		41,1,41,1,41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,42,1,43,1,43,1,
		43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,45,1,45,1,46,1,46,1,
		46,1,46,1,47,1,47,1,47,1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,49,1,49,1,
		49,1,49,1,49,1,49,1,50,1,50,1,50,1,50,1,50,1,51,1,51,1,51,1,51,1,51,1,
		51,1,51,1,51,1,51,3,51,554,8,51,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,
		1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,52,
		3,52,578,8,52,1,53,1,53,1,53,1,53,1,53,1,53,1,54,1,54,1,54,1,54,1,54,1,
		55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,55,1,56,1,56,1,56,1,
		56,1,56,1,57,1,57,1,58,1,58,1,58,1,59,1,59,1,60,1,60,1,61,1,61,1,62,1,
		62,1,63,1,63,1,64,1,64,1,65,1,65,1,65,1,66,1,66,1,67,1,67,1,68,1,68,1,
		69,1,69,1,70,1,70,1,71,1,71,1,72,1,72,1,73,1,73,1,74,1,74,1,74,1,74,1,
		75,1,75,1,75,1,75,1,76,1,76,1,77,1,77,1,78,1,78,1,78,1,78,1,79,1,79,1,
		79,1,79,1,80,1,80,1,80,1,81,1,81,1,81,1,81,1,82,1,82,1,82,1,83,1,83,1,
		83,1,83,5,83,677,8,83,10,83,12,83,680,9,83,1,83,1,83,1,83,1,83,1,84,1,
		84,1,84,1,85,1,85,1,85,1,86,1,86,1,87,1,87,1,88,1,88,5,88,698,8,88,10,
		88,12,88,701,9,88,1,89,1,89,1,89,1,89,3,89,707,8,89,1,90,1,90,1,90,3,90,
		712,8,90,1,90,1,90,1,91,1,91,1,91,5,91,719,8,91,10,91,12,91,722,9,91,1,
		91,1,91,1,92,4,92,727,8,92,11,92,12,92,728,1,92,1,92,1,93,1,93,1,94,1,
		94,1,95,1,95,1,95,3,95,740,8,95,1,95,1,95,1,96,1,96,1,96,3,96,747,8,96,
		1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,
		1,97,1,97,1,97,1,97,1,97,1,97,1,97,1,97,3,97,771,8,97,1,98,1,98,1,98,1,
		98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,1,
		98,1,98,1,98,1,98,1,98,1,98,1,98,1,98,3,98,798,8,98,1,99,1,99,1,99,3,99,
		803,8,99,1,100,1,100,3,100,807,8,100,1,101,1,101,1,102,1,102,3,102,813,
		8,102,1,102,5,102,816,8,102,10,102,12,102,819,9,102,1,103,1,103,5,103,
		823,8,103,10,103,12,103,826,9,103,1,104,1,104,5,104,830,8,104,10,104,12,
		104,833,9,104,1,105,1,105,1,105,1,105,3,105,839,8,105,1,106,1,106,1,106,
		3,106,844,8,106,1,107,1,107,3,107,848,8,107,1,108,1,108,1,109,1,109,1,
		110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,
		1,110,1,110,1,110,1,110,1,110,1,110,1,110,1,110,3,110,874,8,110,1,111,
		3,111,877,8,111,1,112,1,112,1,113,1,113,1,114,1,114,1,115,3,115,886,8,
		115,1,115,1,115,3,115,890,8,115,1,116,4,116,893,8,116,11,116,12,116,894,
		1,116,1,116,0,0,117,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,
		23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,
		47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,
		71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,47,
		95,48,97,49,99,50,101,51,103,52,105,53,107,54,109,55,111,56,113,57,115,
		58,117,59,119,60,121,61,123,62,125,63,127,64,129,65,131,66,133,67,135,
		68,137,69,139,70,141,71,143,72,145,73,147,74,149,75,151,76,153,77,155,
		78,157,79,159,80,161,81,163,82,165,83,167,84,169,85,171,86,173,87,175,
		88,177,89,179,90,181,91,183,92,185,93,187,0,189,0,191,0,193,0,195,0,197,
		0,199,0,201,0,203,0,205,0,207,0,209,0,211,0,213,0,215,0,217,0,219,0,221,
		0,223,0,225,0,227,0,229,0,231,94,233,95,1,0,10,2,0,10,10,12,13,1,0,48,
		57,5,0,10,10,13,13,39,39,92,92,133,133,2,0,34,34,133,133,3,0,10,10,13,
		13,133,133,2,0,69,69,101,101,2,0,9,9,11,12,2,0,32,32,160,160,3,0,48,57,
		65,70,97,102,2,0,9,9,32,32,923,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,
		1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,
		0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,
		1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,
		0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,
		1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,
		0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,
		1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,
		0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,
		1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,
		0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,0,115,1,
		0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,0,0,0,125,1,
		0,0,0,0,127,1,0,0,0,0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,0,0,0,135,1,
		0,0,0,0,137,1,0,0,0,0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,0,0,0,145,1,
		0,0,0,0,147,1,0,0,0,0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,0,0,0,155,1,
		0,0,0,0,157,1,0,0,0,0,159,1,0,0,0,0,161,1,0,0,0,0,163,1,0,0,0,0,165,1,
		0,0,0,0,167,1,0,0,0,0,169,1,0,0,0,0,171,1,0,0,0,0,173,1,0,0,0,0,175,1,
		0,0,0,0,177,1,0,0,0,0,179,1,0,0,0,0,181,1,0,0,0,0,183,1,0,0,0,0,185,1,
		0,0,0,0,231,1,0,0,0,0,233,1,0,0,0,1,236,1,0,0,0,3,241,1,0,0,0,5,252,1,
		0,0,0,7,254,1,0,0,0,9,257,1,0,0,0,11,266,1,0,0,0,13,271,1,0,0,0,15,276,
		1,0,0,0,17,282,1,0,0,0,19,288,1,0,0,0,21,297,1,0,0,0,23,309,1,0,0,0,25,
		315,1,0,0,0,27,323,1,0,0,0,29,328,1,0,0,0,31,332,1,0,0,0,33,337,1,0,0,
		0,35,341,1,0,0,0,37,349,1,0,0,0,39,358,1,0,0,0,41,365,1,0,0,0,43,368,1,
		0,0,0,45,378,1,0,0,0,47,381,1,0,0,0,49,390,1,0,0,0,51,397,1,0,0,0,53,401,
		1,0,0,0,55,406,1,0,0,0,57,410,1,0,0,0,59,413,1,0,0,0,61,417,1,0,0,0,63,
		425,1,0,0,0,65,431,1,0,0,0,67,439,1,0,0,0,69,449,1,0,0,0,71,458,1,0,0,
		0,73,465,1,0,0,0,75,472,1,0,0,0,77,477,1,0,0,0,79,481,1,0,0,0,81,486,1,
		0,0,0,83,493,1,0,0,0,85,500,1,0,0,0,87,505,1,0,0,0,89,510,1,0,0,0,91,516,
		1,0,0,0,93,519,1,0,0,0,95,523,1,0,0,0,97,529,1,0,0,0,99,533,1,0,0,0,101,
		539,1,0,0,0,103,553,1,0,0,0,105,577,1,0,0,0,107,579,1,0,0,0,109,585,1,
		0,0,0,111,590,1,0,0,0,113,601,1,0,0,0,115,606,1,0,0,0,117,608,1,0,0,0,
		119,611,1,0,0,0,121,613,1,0,0,0,123,615,1,0,0,0,125,617,1,0,0,0,127,619,
		1,0,0,0,129,621,1,0,0,0,131,623,1,0,0,0,133,626,1,0,0,0,135,628,1,0,0,
		0,137,630,1,0,0,0,139,632,1,0,0,0,141,634,1,0,0,0,143,636,1,0,0,0,145,
		638,1,0,0,0,147,640,1,0,0,0,149,642,1,0,0,0,151,646,1,0,0,0,153,650,1,
		0,0,0,155,652,1,0,0,0,157,654,1,0,0,0,159,658,1,0,0,0,161,662,1,0,0,0,
		163,665,1,0,0,0,165,669,1,0,0,0,167,672,1,0,0,0,169,685,1,0,0,0,171,688,
		1,0,0,0,173,691,1,0,0,0,175,693,1,0,0,0,177,695,1,0,0,0,179,702,1,0,0,
		0,181,708,1,0,0,0,183,715,1,0,0,0,185,726,1,0,0,0,187,732,1,0,0,0,189,
		734,1,0,0,0,191,736,1,0,0,0,193,746,1,0,0,0,195,770,1,0,0,0,197,797,1,
		0,0,0,199,802,1,0,0,0,201,806,1,0,0,0,203,808,1,0,0,0,205,812,1,0,0,0,
		207,820,1,0,0,0,209,827,1,0,0,0,211,838,1,0,0,0,213,843,1,0,0,0,215,847,
		1,0,0,0,217,849,1,0,0,0,219,851,1,0,0,0,221,873,1,0,0,0,223,876,1,0,0,
		0,225,878,1,0,0,0,227,880,1,0,0,0,229,882,1,0,0,0,231,889,1,0,0,0,233,
		892,1,0,0,0,235,237,7,0,0,0,236,235,1,0,0,0,237,238,1,0,0,0,238,236,1,
		0,0,0,238,239,1,0,0,0,239,2,1,0,0,0,240,242,3,1,0,0,241,240,1,0,0,0,241,
		242,1,0,0,0,242,243,1,0,0,0,243,247,3,5,2,0,244,246,3,187,93,0,245,244,
		1,0,0,0,246,249,1,0,0,0,247,245,1,0,0,0,247,248,1,0,0,0,248,250,1,0,0,
		0,249,247,1,0,0,0,250,251,6,1,0,0,251,4,1,0,0,0,252,253,5,35,0,0,253,6,
		1,0,0,0,254,255,5,97,0,0,255,256,5,115,0,0,256,8,1,0,0,0,257,258,5,97,
		0,0,258,259,5,98,0,0,259,260,5,115,0,0,260,261,5,116,0,0,261,262,5,114,
		0,0,262,263,5,97,0,0,263,264,5,99,0,0,264,265,5,116,0,0,265,10,1,0,0,0,
		266,267,5,99,0,0,267,268,5,97,0,0,268,269,5,108,0,0,269,270,5,108,0,0,
		270,12,1,0,0,0,271,272,5,99,0,0,272,273,5,97,0,0,273,274,5,115,0,0,274,
		275,5,101,0,0,275,14,1,0,0,0,276,277,5,99,0,0,277,278,5,97,0,0,278,279,
		5,116,0,0,279,280,5,99,0,0,280,281,5,104,0,0,281,16,1,0,0,0,282,283,5,
		99,0,0,283,284,5,108,0,0,284,285,5,97,0,0,285,286,5,115,0,0,286,287,5,
		115,0,0,287,18,1,0,0,0,288,289,5,99,0,0,289,290,5,111,0,0,290,291,5,110,
		0,0,291,292,5,115,0,0,292,293,5,116,0,0,293,294,5,97,0,0,294,295,5,110,
		0,0,295,296,5,116,0,0,296,20,1,0,0,0,297,298,5,99,0,0,298,299,5,111,0,
		0,299,300,5,110,0,0,300,301,5,115,0,0,301,302,5,116,0,0,302,303,5,114,
		0,0,303,304,5,117,0,0,304,305,5,99,0,0,305,306,5,116,0,0,306,307,5,111,
		0,0,307,308,5,114,0,0,308,22,1,0,0,0,309,310,5,99,0,0,310,311,5,117,0,
		0,311,312,5,114,0,0,312,313,5,114,0,0,313,314,5,121,0,0,314,24,1,0,0,0,
		315,316,5,100,0,0,316,317,5,101,0,0,317,318,5,102,0,0,318,319,5,97,0,0,
		319,320,5,117,0,0,320,321,5,108,0,0,321,322,5,116,0,0,322,26,1,0,0,0,323,
		324,5,101,0,0,324,325,5,108,0,0,325,326,5,115,0,0,326,327,5,101,0,0,327,
		28,1,0,0,0,328,329,5,101,0,0,329,330,5,110,0,0,330,331,5,100,0,0,331,30,
		1,0,0,0,332,333,5,101,0,0,333,334,5,110,0,0,334,335,5,117,0,0,335,336,
		5,109,0,0,336,32,1,0,0,0,337,338,5,102,0,0,338,339,5,111,0,0,339,340,5,
		114,0,0,340,34,1,0,0,0,341,342,5,102,0,0,342,343,5,111,0,0,343,344,5,114,
		0,0,344,345,5,101,0,0,345,346,5,97,0,0,346,347,5,99,0,0,347,348,5,104,
		0,0,348,36,1,0,0,0,349,350,5,102,0,0,350,351,5,117,0,0,351,352,5,110,0,
		0,352,353,5,99,0,0,353,354,5,116,0,0,354,355,5,105,0,0,355,356,5,111,0,
		0,356,357,5,110,0,0,357,38,1,0,0,0,358,359,5,103,0,0,359,360,5,108,0,0,
		360,361,5,111,0,0,361,362,5,98,0,0,362,363,5,97,0,0,363,364,5,108,0,0,
		364,40,1,0,0,0,365,366,5,105,0,0,366,367,5,102,0,0,367,42,1,0,0,0,368,
		369,5,105,0,0,369,370,5,109,0,0,370,371,5,109,0,0,371,372,5,117,0,0,372,
		373,5,116,0,0,373,374,5,97,0,0,374,375,5,98,0,0,375,376,5,108,0,0,376,
		377,5,101,0,0,377,44,1,0,0,0,378,379,5,105,0,0,379,380,5,110,0,0,380,46,
		1,0,0,0,381,382,5,105,0,0,382,383,5,110,0,0,383,384,5,104,0,0,384,385,
		5,101,0,0,385,386,5,114,0,0,386,387,5,105,0,0,387,388,5,116,0,0,388,389,
		5,115,0,0,389,48,1,0,0,0,390,391,5,108,0,0,391,392,5,97,0,0,392,393,5,
		109,0,0,393,394,5,98,0,0,394,395,5,100,0,0,395,396,5,97,0,0,396,50,1,0,
		0,0,397,398,5,108,0,0,398,399,5,101,0,0,399,400,5,116,0,0,400,52,1,0,0,
		0,401,402,5,109,0,0,402,403,5,97,0,0,403,404,5,105,0,0,404,405,5,110,0,
		0,405,54,1,0,0,0,406,407,5,110,0,0,407,408,5,101,0,0,408,409,5,119,0,0,
		409,56,1,0,0,0,410,411,5,111,0,0,411,412,5,102,0,0,412,58,1,0,0,0,413,
		414,5,111,0,0,414,415,5,117,0,0,415,416,5,116,0,0,416,60,1,0,0,0,417,418,
		5,112,0,0,418,419,5,97,0,0,419,420,5,114,0,0,420,421,5,116,0,0,421,422,
		5,105,0,0,422,423,5,97,0,0,423,424,5,108,0,0,424,62,1,0,0,0,425,426,5,
		112,0,0,426,427,5,114,0,0,427,428,5,105,0,0,428,429,5,110,0,0,429,430,
		5,116,0,0,430,64,1,0,0,0,431,432,5,112,0,0,432,433,5,114,0,0,433,434,5,
		105,0,0,434,435,5,118,0,0,435,436,5,97,0,0,436,437,5,116,0,0,437,438,5,
		101,0,0,438,66,1,0,0,0,439,440,5,112,0,0,440,441,5,114,0,0,441,442,5,111,
		0,0,442,443,5,99,0,0,443,444,5,101,0,0,444,445,5,100,0,0,445,446,5,117,
		0,0,446,447,5,114,0,0,447,448,5,101,0,0,448,68,1,0,0,0,449,450,5,112,0,
		0,450,451,5,114,0,0,451,452,5,111,0,0,452,453,5,112,0,0,453,454,5,101,
		0,0,454,455,5,114,0,0,455,456,5,116,0,0,456,457,5,121,0,0,457,70,1,0,0,
		0,458,459,5,114,0,0,459,460,5,101,0,0,460,461,5,112,0,0,461,462,5,101,
		0,0,462,463,5,97,0,0,463,464,5,116,0,0,464,72,1,0,0,0,465,466,5,114,0,
		0,466,467,5,101,0,0,467,468,5,116,0,0,468,469,5,117,0,0,469,470,5,114,
		0,0,470,471,5,110,0,0,471,74,1,0,0,0,472,473,5,115,0,0,473,474,5,101,0,
		0,474,475,5,108,0,0,475,476,5,102,0,0,476,76,1,0,0,0,477,478,5,115,0,0,
		478,479,5,101,0,0,479,480,5,116,0,0,480,78,1,0,0,0,481,482,5,115,0,0,482,
		483,5,116,0,0,483,484,5,101,0,0,484,485,5,112,0,0,485,80,1,0,0,0,486,487,
		5,115,0,0,487,488,5,119,0,0,488,489,5,105,0,0,489,490,5,116,0,0,490,491,
		5,99,0,0,491,492,5,104,0,0,492,82,1,0,0,0,493,494,5,115,0,0,494,495,5,
		121,0,0,495,496,5,115,0,0,496,497,5,116,0,0,497,498,5,101,0,0,498,499,
		5,109,0,0,499,84,1,0,0,0,500,501,5,116,0,0,501,502,5,101,0,0,502,503,5,
		115,0,0,503,504,5,116,0,0,504,86,1,0,0,0,505,506,5,116,0,0,506,507,5,104,
		0,0,507,508,5,101,0,0,508,509,5,110,0,0,509,88,1,0,0,0,510,511,5,116,0,
		0,511,512,5,104,0,0,512,513,5,114,0,0,513,514,5,111,0,0,514,515,5,119,
		0,0,515,90,1,0,0,0,516,517,5,116,0,0,517,518,5,111,0,0,518,92,1,0,0,0,
		519,520,5,116,0,0,520,521,5,114,0,0,521,522,5,121,0,0,522,94,1,0,0,0,523,
		524,5,117,0,0,524,525,5,110,0,0,525,526,5,116,0,0,526,527,5,105,0,0,527,
		528,5,108,0,0,528,96,1,0,0,0,529,530,5,118,0,0,530,531,5,97,0,0,531,532,
		5,114,0,0,532,98,1,0,0,0,533,534,5,119,0,0,534,535,5,104,0,0,535,536,5,
		105,0,0,536,537,5,108,0,0,537,538,5,101,0,0,538,100,1,0,0,0,539,540,5,
		119,0,0,540,541,5,105,0,0,541,542,5,116,0,0,542,543,5,104,0,0,543,102,
		1,0,0,0,544,545,5,116,0,0,545,546,5,114,0,0,546,547,5,117,0,0,547,554,
		5,101,0,0,548,549,5,102,0,0,549,550,5,97,0,0,550,551,5,108,0,0,551,552,
		5,115,0,0,552,554,5,101,0,0,553,544,1,0,0,0,553,548,1,0,0,0,554,104,1,
		0,0,0,555,556,5,73,0,0,556,557,5,110,0,0,557,578,5,116,0,0,558,559,5,70,
		0,0,559,560,5,108,0,0,560,561,5,111,0,0,561,562,5,97,0,0,562,578,5,116,
		0,0,563,564,5,67,0,0,564,565,5,104,0,0,565,566,5,97,0,0,566,578,5,114,
		0,0,567,568,5,83,0,0,568,569,5,116,0,0,569,570,5,114,0,0,570,571,5,105,
		0,0,571,572,5,110,0,0,572,578,5,103,0,0,573,574,5,66,0,0,574,575,5,111,
		0,0,575,576,5,111,0,0,576,578,5,108,0,0,577,555,1,0,0,0,577,558,1,0,0,
		0,577,563,1,0,0,0,577,567,1,0,0,0,577,573,1,0,0,0,578,106,1,0,0,0,579,
		580,5,65,0,0,580,581,5,114,0,0,581,582,5,114,0,0,582,583,5,97,0,0,583,
		584,5,121,0,0,584,108,1,0,0,0,585,586,5,76,0,0,586,587,5,105,0,0,587,588,
		5,115,0,0,588,589,5,116,0,0,589,110,1,0,0,0,590,591,5,68,0,0,591,592,5,
		105,0,0,592,593,5,99,0,0,593,594,5,116,0,0,594,595,5,105,0,0,595,596,5,
		111,0,0,596,597,5,110,0,0,597,598,5,97,0,0,598,599,5,114,0,0,599,600,5,
		121,0,0,600,112,1,0,0,0,601,602,5,73,0,0,602,603,5,116,0,0,603,604,5,101,
		0,0,604,605,5,114,0,0,605,114,1,0,0,0,606,607,5,61,0,0,607,116,1,0,0,0,
		608,609,5,45,0,0,609,610,5,62,0,0,610,118,1,0,0,0,611,612,5,123,0,0,612,
		120,1,0,0,0,613,614,5,125,0,0,614,122,1,0,0,0,615,616,5,91,0,0,616,124,
		1,0,0,0,617,618,5,93,0,0,618,126,1,0,0,0,619,620,5,40,0,0,620,128,1,0,
		0,0,621,622,5,41,0,0,622,130,1,0,0,0,623,624,5,46,0,0,624,625,5,46,0,0,
		625,132,1,0,0,0,626,627,5,46,0,0,627,134,1,0,0,0,628,629,5,44,0,0,629,
		136,1,0,0,0,630,631,5,58,0,0,631,138,1,0,0,0,632,633,5,43,0,0,633,140,
		1,0,0,0,634,635,5,45,0,0,635,142,1,0,0,0,636,637,5,42,0,0,637,144,1,0,
		0,0,638,639,5,47,0,0,639,146,1,0,0,0,640,641,5,94,0,0,641,148,1,0,0,0,
		642,643,5,109,0,0,643,644,5,111,0,0,644,645,5,100,0,0,645,150,1,0,0,0,
		646,647,5,100,0,0,647,648,5,105,0,0,648,649,5,118,0,0,649,152,1,0,0,0,
		650,651,5,60,0,0,651,154,1,0,0,0,652,653,5,62,0,0,653,156,1,0,0,0,654,
		655,5,97,0,0,655,656,5,110,0,0,656,657,5,100,0,0,657,158,1,0,0,0,658,659,
		5,110,0,0,659,660,5,111,0,0,660,661,5,116,0,0,661,160,1,0,0,0,662,663,
		5,111,0,0,663,664,5,114,0,0,664,162,1,0,0,0,665,666,5,120,0,0,666,667,
		5,111,0,0,667,668,5,114,0,0,668,164,1,0,0,0,669,670,5,105,0,0,670,671,
		5,115,0,0,671,166,1,0,0,0,672,673,5,105,0,0,673,674,5,115,0,0,674,678,
		1,0,0,0,675,677,3,201,100,0,676,675,1,0,0,0,677,680,1,0,0,0,678,676,1,
		0,0,0,678,679,1,0,0,0,679,681,1,0,0,0,680,678,1,0,0,0,681,682,5,110,0,
		0,682,683,5,111,0,0,683,684,5,116,0,0,684,168,1,0,0,0,685,686,5,60,0,0,
		686,687,5,61,0,0,687,170,1,0,0,0,688,689,5,62,0,0,689,690,5,61,0,0,690,
		172,1,0,0,0,691,692,3,209,104,0,692,174,1,0,0,0,693,694,3,207,103,0,694,
		176,1,0,0,0,695,699,7,1,0,0,696,698,7,1,0,0,697,696,1,0,0,0,698,701,1,
		0,0,0,699,697,1,0,0,0,699,700,1,0,0,0,700,178,1,0,0,0,701,699,1,0,0,0,
		702,703,3,177,88,0,703,704,3,133,66,0,704,706,3,177,88,0,705,707,3,191,
		95,0,706,705,1,0,0,0,706,707,1,0,0,0,707,180,1,0,0,0,708,711,5,39,0,0,
		709,712,8,2,0,0,710,712,3,193,96,0,711,709,1,0,0,0,711,710,1,0,0,0,712,
		713,1,0,0,0,713,714,5,39,0,0,714,182,1,0,0,0,715,720,5,34,0,0,716,719,
		8,3,0,0,717,719,3,193,96,0,718,716,1,0,0,0,718,717,1,0,0,0,719,722,1,0,
		0,0,720,718,1,0,0,0,720,721,1,0,0,0,721,723,1,0,0,0,722,720,1,0,0,0,723,
		724,5,34,0,0,724,184,1,0,0,0,725,727,3,201,100,0,726,725,1,0,0,0,727,728,
		1,0,0,0,728,726,1,0,0,0,728,729,1,0,0,0,729,730,1,0,0,0,730,731,6,92,0,
		0,731,186,1,0,0,0,732,733,8,4,0,0,733,188,1,0,0,0,734,735,7,4,0,0,735,
		190,1,0,0,0,736,739,7,5,0,0,737,740,3,139,69,0,738,740,3,141,70,0,739,
		737,1,0,0,0,739,738,1,0,0,0,739,740,1,0,0,0,740,741,1,0,0,0,741,742,3,
		177,88,0,742,192,1,0,0,0,743,747,3,195,97,0,744,747,3,197,98,0,745,747,
		3,221,110,0,746,743,1,0,0,0,746,744,1,0,0,0,746,745,1,0,0,0,747,194,1,
		0,0,0,748,749,5,92,0,0,749,771,5,39,0,0,750,751,5,92,0,0,751,771,5,34,
		0,0,752,753,5,92,0,0,753,771,5,92,0,0,754,755,5,92,0,0,755,771,5,48,0,
		0,756,757,5,92,0,0,757,771,5,97,0,0,758,759,5,92,0,0,759,771,5,98,0,0,
		760,761,5,92,0,0,761,771,5,102,0,0,762,763,5,92,0,0,763,771,5,110,0,0,
		764,765,5,92,0,0,765,771,5,114,0,0,766,767,5,92,0,0,767,771,5,116,0,0,
		768,769,5,92,0,0,769,771,5,118,0,0,770,748,1,0,0,0,770,750,1,0,0,0,770,
		752,1,0,0,0,770,754,1,0,0,0,770,756,1,0,0,0,770,758,1,0,0,0,770,760,1,
		0,0,0,770,762,1,0,0,0,770,764,1,0,0,0,770,766,1,0,0,0,770,768,1,0,0,0,
		771,196,1,0,0,0,772,773,5,92,0,0,773,774,5,120,0,0,774,775,1,0,0,0,775,
		798,3,223,111,0,776,777,5,92,0,0,777,778,5,120,0,0,778,779,1,0,0,0,779,
		780,3,223,111,0,780,781,3,223,111,0,781,798,1,0,0,0,782,783,5,92,0,0,783,
		784,5,120,0,0,784,785,1,0,0,0,785,786,3,223,111,0,786,787,3,223,111,0,
		787,788,3,223,111,0,788,798,1,0,0,0,789,790,5,92,0,0,790,791,5,120,0,0,
		791,792,1,0,0,0,792,793,3,223,111,0,793,794,3,223,111,0,794,795,3,223,
		111,0,795,796,3,223,111,0,796,798,1,0,0,0,797,772,1,0,0,0,797,776,1,0,
		0,0,797,782,1,0,0,0,797,789,1,0,0,0,798,198,1,0,0,0,799,800,5,13,0,0,800,
		803,5,10,0,0,801,803,7,4,0,0,802,799,1,0,0,0,802,801,1,0,0,0,803,200,1,
		0,0,0,804,807,3,203,101,0,805,807,7,6,0,0,806,804,1,0,0,0,806,805,1,0,
		0,0,807,202,1,0,0,0,808,809,7,7,0,0,809,204,1,0,0,0,810,813,3,227,113,
		0,811,813,3,225,112,0,812,810,1,0,0,0,812,811,1,0,0,0,813,817,1,0,0,0,
		814,816,3,211,105,0,815,814,1,0,0,0,816,819,1,0,0,0,817,815,1,0,0,0,817,
		818,1,0,0,0,818,206,1,0,0,0,819,817,1,0,0,0,820,824,3,227,113,0,821,823,
		3,211,105,0,822,821,1,0,0,0,823,826,1,0,0,0,824,822,1,0,0,0,824,825,1,
		0,0,0,825,208,1,0,0,0,826,824,1,0,0,0,827,831,3,225,112,0,828,830,3,211,
		105,0,829,828,1,0,0,0,830,833,1,0,0,0,831,829,1,0,0,0,831,832,1,0,0,0,
		832,210,1,0,0,0,833,831,1,0,0,0,834,839,3,225,112,0,835,839,3,227,113,
		0,836,839,3,215,107,0,837,839,5,95,0,0,838,834,1,0,0,0,838,835,1,0,0,0,
		838,836,1,0,0,0,838,837,1,0,0,0,839,212,1,0,0,0,840,844,3,225,112,0,841,
		844,3,227,113,0,842,844,3,221,110,0,843,840,1,0,0,0,843,841,1,0,0,0,843,
		842,1,0,0,0,844,214,1,0,0,0,845,848,3,229,114,0,846,848,3,221,110,0,847,
		845,1,0,0,0,847,846,1,0,0,0,848,216,1,0,0,0,849,850,3,221,110,0,850,218,
		1,0,0,0,851,852,3,221,110,0,852,220,1,0,0,0,853,854,5,92,0,0,854,855,5,
		117,0,0,855,856,1,0,0,0,856,857,3,223,111,0,857,858,3,223,111,0,858,859,
		3,223,111,0,859,860,3,223,111,0,860,874,1,0,0,0,861,862,5,92,0,0,862,863,
		5,85,0,0,863,864,1,0,0,0,864,865,3,223,111,0,865,866,3,223,111,0,866,867,
		3,223,111,0,867,868,3,223,111,0,868,869,3,223,111,0,869,870,3,223,111,
		0,870,871,3,223,111,0,871,872,3,223,111,0,872,874,1,0,0,0,873,853,1,0,
		0,0,873,861,1,0,0,0,874,222,1,0,0,0,875,877,7,8,0,0,876,875,1,0,0,0,877,
		224,1,0,0,0,878,879,2,65,90,0,879,226,1,0,0,0,880,881,2,97,122,0,881,228,
		1,0,0,0,882,883,2,48,57,0,883,230,1,0,0,0,884,886,5,13,0,0,885,884,1,0,
		0,0,885,886,1,0,0,0,886,887,1,0,0,0,887,890,5,10,0,0,888,890,5,13,0,0,
		889,885,1,0,0,0,889,888,1,0,0,0,890,232,1,0,0,0,891,893,7,9,0,0,892,891,
		1,0,0,0,893,894,1,0,0,0,894,892,1,0,0,0,894,895,1,0,0,0,895,896,1,0,0,
		0,896,897,6,116,0,0,897,234,1,0,0,0,31,0,238,241,247,553,577,678,699,706,
		711,718,720,728,739,746,770,797,802,806,812,817,824,831,838,843,847,873,
		876,885,889,894,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
