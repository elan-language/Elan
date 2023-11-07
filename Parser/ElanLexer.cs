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
		NL=1, SINGLE_LINE_COMMENT=2, COMMENT_MARKER=3, ABSTRACT=4, CASE=5, CATCH=6, 
		CLASS=7, CONSTANT=8, CONSTRUCTOR=9, CURRY=10, DEFAULT=11, ELSE=12, END=13, 
		ENUMERATION=14, FOR=15, FOREACH=16, FUNCTION=17, GLOBAL=18, IF=19, IMMUTABLE=20, 
		IN=21, INHERITS=22, LAMBDA=23, LET=24, MAIN=25, PARTIAL=26, PRIVATE=27, 
		PROCEDURE=28, PROPERTY=29, REPEAT=30, RETURN=31, SELF=32, STEP=33, SWITCH=34, 
		TEST=35, THEN=36, TO=37, TRY=38, UNTIL=39, VAR=40, WHILE=41, WITH=42, 
		BOOL_VALUE=43, VALUE_TYPE=44, ARRAY=45, LIST=46, DICTIONARY=47, ITERABLE=48, 
		ASSIGN=49, ARROW=50, OPEN_BRACE=51, CLOSE_BRACE=52, OPEN_SQ_BRACKET=53, 
		CLOSE_SQ_BRACKET=54, OPEN_BRACKET=55, CLOSE_BRACKET=56, DOUBLE_DOT=57, 
		DOT=58, COMMA=59, COLON=60, PLUS=61, MINUS=62, MULT=63, DIVIDE=64, POWER=65, 
		MOD=66, INT_DIV=67, LT=68, GT=69, OP_AND=70, OP_NOT=71, OP_OR=72, OP_XOR=73, 
		OP_EQ=74, OP_NE=75, OP_LE=76, OP_GE=77, TYPENAME=78, IDENTIFIER=79, LITERAL_INTEGER=80, 
		LITERAL_FLOAT=81, LITERAL_CHAR=82, LITERAL_STRING=83, WHITESPACES=84, 
		NEWLINE=85, WS=86;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "ABSTRACT", "CASE", "CATCH", 
		"CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", "DEFAULT", "ELSE", "END", 
		"ENUMERATION", "FOR", "FOREACH", "FUNCTION", "GLOBAL", "IF", "IMMUTABLE", 
		"IN", "INHERITS", "LAMBDA", "LET", "MAIN", "PARTIAL", "PRIVATE", "PROCEDURE", 
		"PROPERTY", "REPEAT", "RETURN", "SELF", "STEP", "SWITCH", "TEST", "THEN", 
		"TO", "TRY", "UNTIL", "VAR", "WHILE", "WITH", "BOOL_VALUE", "VALUE_TYPE", 
		"ARRAY", "LIST", "DICTIONARY", "ITERABLE", "ASSIGN", "ARROW", "OPEN_BRACE", 
		"CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", "OPEN_BRACKET", 
		"CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", "PLUS", "MINUS", 
		"MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", "OP_AND", "OP_NOT", 
		"OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", "TYPENAME", "IDENTIFIER", 
		"LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_CHAR", "LITERAL_STRING", 
		"WHITESPACES", "InputCharacter", "NewLineCharacter", "ExponentPart", "CommonCharacter", 
		"SimpleEscapeSequence", "HexEscapeSequence", "NewLine", "Whitespace", 
		"UnicodeClassZS", "IdentifierStartingUCorLC", "IdentifierStartingLC", 
		"IdentifierStartingUC", "IdentifierPartCharacter", "LetterCharacter", 
		"DecimalDigitCharacter", "ConnectingCharacter", "FormattingCharacter", 
		"UnicodeEscapeSequence", "HexDigit", "UnicodeClassLU", "UnicodeClassLL", 
		"UnicodeClassND", "NEWLINE", "WS"
	};


	public ElanLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ElanLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, "'#'", "'abstract'", "'case'", "'catch'", "'class'", 
		"'constant'", "'constructor'", "'curry'", "'default'", "'else'", "'end'", 
		"'enumeration'", "'for'", "'foreach'", "'function'", "'global'", "'if'", 
		"'immutable'", "'in'", "'inherits'", "'lambda'", "'let'", "'main'", "'partial'", 
		"'private'", "'procedure'", "'property'", "'repeat'", "'return'", "'self'", 
		"'step'", "'switch'", "'test'", "'then'", "'to'", "'try'", "'until'", 
		"'var'", "'while'", "'with'", null, null, "'Array'", "'List'", "'Dictionary'", 
		"'Iter'", "'='", "'->'", "'{'", "'}'", "'['", "']'", "'('", "')'", "'..'", 
		"'.'", "','", "':'", "'+'", "'-'", "'*'", "'/'", "'^'", "'mod'", "'div'", 
		"'<'", "'>'", "'and'", "'not'", "'or'", "'xor'", null, null, "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "ABSTRACT", "CASE", 
		"CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", "DEFAULT", "ELSE", 
		"END", "ENUMERATION", "FOR", "FOREACH", "FUNCTION", "GLOBAL", "IF", "IMMUTABLE", 
		"IN", "INHERITS", "LAMBDA", "LET", "MAIN", "PARTIAL", "PRIVATE", "PROCEDURE", 
		"PROPERTY", "REPEAT", "RETURN", "SELF", "STEP", "SWITCH", "TEST", "THEN", 
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
		4,0,86,851,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		104,2,105,7,105,2,106,7,106,2,107,7,107,1,0,4,0,219,8,0,11,0,12,0,220,
		1,1,3,1,224,8,1,1,1,1,1,5,1,228,8,1,10,1,12,1,231,9,1,1,1,1,1,1,2,1,2,
		1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,
		5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,7,
		1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,
		9,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,12,
		1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,13,
		1,13,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,16,
		1,16,1,16,1,16,1,16,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,17,
		1,17,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,19,1,19,
		1,20,1,20,1,20,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,22,1,22,
		1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,
		1,25,1,25,1,25,1,25,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,26,
		1,26,1,26,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,28,1,28,
		1,28,1,28,1,28,1,28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,29,1,29,1,29,
		1,30,1,30,1,30,1,30,1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,1,32,1,32,
		1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,
		1,34,1,35,1,35,1,35,1,35,1,35,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,38,
		1,38,1,38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,40,
		1,40,1,41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,42,1,42,1,42,1,42,
		1,42,3,42,501,8,42,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,
		43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,43,3,43,525,8,43,
		1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,45,1,45,1,45,1,45,1,46,1,46,1,46,
		1,46,1,46,1,46,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,1,47,1,47,1,48,
		1,48,1,49,1,49,1,49,1,50,1,50,1,51,1,51,1,52,1,52,1,53,1,53,1,54,1,54,
		1,55,1,55,1,56,1,56,1,56,1,57,1,57,1,58,1,58,1,59,1,59,1,60,1,60,1,61,
		1,61,1,62,1,62,1,63,1,63,1,64,1,64,1,65,1,65,1,65,1,65,1,66,1,66,1,66,
		1,66,1,67,1,67,1,68,1,68,1,69,1,69,1,69,1,69,1,70,1,70,1,70,1,70,1,71,
		1,71,1,71,1,72,1,72,1,72,1,72,1,73,1,73,1,73,1,73,3,73,621,8,73,1,74,1,
		74,1,74,1,74,1,74,1,74,5,74,629,8,74,10,74,12,74,632,9,74,1,74,1,74,1,
		74,3,74,637,8,74,1,75,1,75,1,75,1,76,1,76,1,76,1,77,1,77,1,78,1,78,1,79,
		1,79,5,79,651,8,79,10,79,12,79,654,9,79,1,80,1,80,1,80,1,80,3,80,660,8,
		80,1,81,1,81,1,81,3,81,665,8,81,1,81,1,81,1,82,1,82,1,82,5,82,672,8,82,
		10,82,12,82,675,9,82,1,82,1,82,1,83,4,83,680,8,83,11,83,12,83,681,1,83,
		1,83,1,84,1,84,1,85,1,85,1,86,1,86,1,86,3,86,693,8,86,1,86,1,86,1,87,1,
		87,1,87,3,87,700,8,87,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,
		1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,3,88,724,8,
		88,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,
		89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,1,89,3,89,751,8,89,
		1,90,1,90,1,90,3,90,756,8,90,1,91,1,91,3,91,760,8,91,1,92,1,92,1,93,1,
		93,3,93,766,8,93,1,93,5,93,769,8,93,10,93,12,93,772,9,93,1,94,1,94,5,94,
		776,8,94,10,94,12,94,779,9,94,1,95,1,95,5,95,783,8,95,10,95,12,95,786,
		9,95,1,96,1,96,1,96,1,96,3,96,792,8,96,1,97,1,97,1,97,3,97,797,8,97,1,
		98,1,98,3,98,801,8,98,1,99,1,99,1,100,1,100,1,101,1,101,1,101,1,101,1,
		101,1,101,1,101,1,101,1,101,1,101,1,101,1,101,1,101,1,101,1,101,1,101,
		1,101,1,101,1,101,1,101,3,101,827,8,101,1,102,3,102,830,8,102,1,103,1,
		103,1,104,1,104,1,105,1,105,1,106,3,106,839,8,106,1,106,1,106,3,106,843,
		8,106,1,107,4,107,846,8,107,11,107,12,107,847,1,107,1,107,0,0,108,1,1,
		3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,
		31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,
		55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,77,39,
		79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,47,95,48,97,49,99,50,101,
		51,103,52,105,53,107,54,109,55,111,56,113,57,115,58,117,59,119,60,121,
		61,123,62,125,63,127,64,129,65,131,66,133,67,135,68,137,69,139,70,141,
		71,143,72,145,73,147,74,149,75,151,76,153,77,155,78,157,79,159,80,161,
		81,163,82,165,83,167,84,169,0,171,0,173,0,175,0,177,0,179,0,181,0,183,
		0,185,0,187,0,189,0,191,0,193,0,195,0,197,0,199,0,201,0,203,0,205,0,207,
		0,209,0,211,0,213,85,215,86,1,0,10,2,0,10,10,12,13,1,0,48,57,5,0,10,10,
		13,13,39,39,92,92,133,133,2,0,34,34,133,133,3,0,10,10,13,13,133,133,2,
		0,69,69,101,101,2,0,9,9,11,12,2,0,32,32,160,160,3,0,48,57,65,70,97,102,
		2,0,9,9,32,32,878,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,
		1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,
		0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,
		1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,
		0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,
		1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,
		0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,
		1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,
		0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,
		1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,
		0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,
		0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,0,0,0,125,1,0,0,0,0,127,1,
		0,0,0,0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,0,0,0,135,1,0,0,0,0,137,1,
		0,0,0,0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,0,0,0,145,1,0,0,0,0,147,1,
		0,0,0,0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,0,0,0,155,1,0,0,0,0,157,1,
		0,0,0,0,159,1,0,0,0,0,161,1,0,0,0,0,163,1,0,0,0,0,165,1,0,0,0,0,167,1,
		0,0,0,0,213,1,0,0,0,0,215,1,0,0,0,1,218,1,0,0,0,3,223,1,0,0,0,5,234,1,
		0,0,0,7,236,1,0,0,0,9,245,1,0,0,0,11,250,1,0,0,0,13,256,1,0,0,0,15,262,
		1,0,0,0,17,271,1,0,0,0,19,283,1,0,0,0,21,289,1,0,0,0,23,297,1,0,0,0,25,
		302,1,0,0,0,27,306,1,0,0,0,29,318,1,0,0,0,31,322,1,0,0,0,33,330,1,0,0,
		0,35,339,1,0,0,0,37,346,1,0,0,0,39,349,1,0,0,0,41,359,1,0,0,0,43,362,1,
		0,0,0,45,371,1,0,0,0,47,378,1,0,0,0,49,382,1,0,0,0,51,387,1,0,0,0,53,395,
		1,0,0,0,55,403,1,0,0,0,57,413,1,0,0,0,59,422,1,0,0,0,61,429,1,0,0,0,63,
		436,1,0,0,0,65,441,1,0,0,0,67,446,1,0,0,0,69,453,1,0,0,0,71,458,1,0,0,
		0,73,463,1,0,0,0,75,466,1,0,0,0,77,470,1,0,0,0,79,476,1,0,0,0,81,480,1,
		0,0,0,83,486,1,0,0,0,85,500,1,0,0,0,87,524,1,0,0,0,89,526,1,0,0,0,91,532,
		1,0,0,0,93,537,1,0,0,0,95,548,1,0,0,0,97,553,1,0,0,0,99,555,1,0,0,0,101,
		558,1,0,0,0,103,560,1,0,0,0,105,562,1,0,0,0,107,564,1,0,0,0,109,566,1,
		0,0,0,111,568,1,0,0,0,113,570,1,0,0,0,115,573,1,0,0,0,117,575,1,0,0,0,
		119,577,1,0,0,0,121,579,1,0,0,0,123,581,1,0,0,0,125,583,1,0,0,0,127,585,
		1,0,0,0,129,587,1,0,0,0,131,589,1,0,0,0,133,593,1,0,0,0,135,597,1,0,0,
		0,137,599,1,0,0,0,139,601,1,0,0,0,141,605,1,0,0,0,143,609,1,0,0,0,145,
		612,1,0,0,0,147,620,1,0,0,0,149,636,1,0,0,0,151,638,1,0,0,0,153,641,1,
		0,0,0,155,644,1,0,0,0,157,646,1,0,0,0,159,648,1,0,0,0,161,655,1,0,0,0,
		163,661,1,0,0,0,165,668,1,0,0,0,167,679,1,0,0,0,169,685,1,0,0,0,171,687,
		1,0,0,0,173,689,1,0,0,0,175,699,1,0,0,0,177,723,1,0,0,0,179,750,1,0,0,
		0,181,755,1,0,0,0,183,759,1,0,0,0,185,761,1,0,0,0,187,765,1,0,0,0,189,
		773,1,0,0,0,191,780,1,0,0,0,193,791,1,0,0,0,195,796,1,0,0,0,197,800,1,
		0,0,0,199,802,1,0,0,0,201,804,1,0,0,0,203,826,1,0,0,0,205,829,1,0,0,0,
		207,831,1,0,0,0,209,833,1,0,0,0,211,835,1,0,0,0,213,842,1,0,0,0,215,845,
		1,0,0,0,217,219,7,0,0,0,218,217,1,0,0,0,219,220,1,0,0,0,220,218,1,0,0,
		0,220,221,1,0,0,0,221,2,1,0,0,0,222,224,3,1,0,0,223,222,1,0,0,0,223,224,
		1,0,0,0,224,225,1,0,0,0,225,229,3,5,2,0,226,228,3,169,84,0,227,226,1,0,
		0,0,228,231,1,0,0,0,229,227,1,0,0,0,229,230,1,0,0,0,230,232,1,0,0,0,231,
		229,1,0,0,0,232,233,6,1,0,0,233,4,1,0,0,0,234,235,5,35,0,0,235,6,1,0,0,
		0,236,237,5,97,0,0,237,238,5,98,0,0,238,239,5,115,0,0,239,240,5,116,0,
		0,240,241,5,114,0,0,241,242,5,97,0,0,242,243,5,99,0,0,243,244,5,116,0,
		0,244,8,1,0,0,0,245,246,5,99,0,0,246,247,5,97,0,0,247,248,5,115,0,0,248,
		249,5,101,0,0,249,10,1,0,0,0,250,251,5,99,0,0,251,252,5,97,0,0,252,253,
		5,116,0,0,253,254,5,99,0,0,254,255,5,104,0,0,255,12,1,0,0,0,256,257,5,
		99,0,0,257,258,5,108,0,0,258,259,5,97,0,0,259,260,5,115,0,0,260,261,5,
		115,0,0,261,14,1,0,0,0,262,263,5,99,0,0,263,264,5,111,0,0,264,265,5,110,
		0,0,265,266,5,115,0,0,266,267,5,116,0,0,267,268,5,97,0,0,268,269,5,110,
		0,0,269,270,5,116,0,0,270,16,1,0,0,0,271,272,5,99,0,0,272,273,5,111,0,
		0,273,274,5,110,0,0,274,275,5,115,0,0,275,276,5,116,0,0,276,277,5,114,
		0,0,277,278,5,117,0,0,278,279,5,99,0,0,279,280,5,116,0,0,280,281,5,111,
		0,0,281,282,5,114,0,0,282,18,1,0,0,0,283,284,5,99,0,0,284,285,5,117,0,
		0,285,286,5,114,0,0,286,287,5,114,0,0,287,288,5,121,0,0,288,20,1,0,0,0,
		289,290,5,100,0,0,290,291,5,101,0,0,291,292,5,102,0,0,292,293,5,97,0,0,
		293,294,5,117,0,0,294,295,5,108,0,0,295,296,5,116,0,0,296,22,1,0,0,0,297,
		298,5,101,0,0,298,299,5,108,0,0,299,300,5,115,0,0,300,301,5,101,0,0,301,
		24,1,0,0,0,302,303,5,101,0,0,303,304,5,110,0,0,304,305,5,100,0,0,305,26,
		1,0,0,0,306,307,5,101,0,0,307,308,5,110,0,0,308,309,5,117,0,0,309,310,
		5,109,0,0,310,311,5,101,0,0,311,312,5,114,0,0,312,313,5,97,0,0,313,314,
		5,116,0,0,314,315,5,105,0,0,315,316,5,111,0,0,316,317,5,110,0,0,317,28,
		1,0,0,0,318,319,5,102,0,0,319,320,5,111,0,0,320,321,5,114,0,0,321,30,1,
		0,0,0,322,323,5,102,0,0,323,324,5,111,0,0,324,325,5,114,0,0,325,326,5,
		101,0,0,326,327,5,97,0,0,327,328,5,99,0,0,328,329,5,104,0,0,329,32,1,0,
		0,0,330,331,5,102,0,0,331,332,5,117,0,0,332,333,5,110,0,0,333,334,5,99,
		0,0,334,335,5,116,0,0,335,336,5,105,0,0,336,337,5,111,0,0,337,338,5,110,
		0,0,338,34,1,0,0,0,339,340,5,103,0,0,340,341,5,108,0,0,341,342,5,111,0,
		0,342,343,5,98,0,0,343,344,5,97,0,0,344,345,5,108,0,0,345,36,1,0,0,0,346,
		347,5,105,0,0,347,348,5,102,0,0,348,38,1,0,0,0,349,350,5,105,0,0,350,351,
		5,109,0,0,351,352,5,109,0,0,352,353,5,117,0,0,353,354,5,116,0,0,354,355,
		5,97,0,0,355,356,5,98,0,0,356,357,5,108,0,0,357,358,5,101,0,0,358,40,1,
		0,0,0,359,360,5,105,0,0,360,361,5,110,0,0,361,42,1,0,0,0,362,363,5,105,
		0,0,363,364,5,110,0,0,364,365,5,104,0,0,365,366,5,101,0,0,366,367,5,114,
		0,0,367,368,5,105,0,0,368,369,5,116,0,0,369,370,5,115,0,0,370,44,1,0,0,
		0,371,372,5,108,0,0,372,373,5,97,0,0,373,374,5,109,0,0,374,375,5,98,0,
		0,375,376,5,100,0,0,376,377,5,97,0,0,377,46,1,0,0,0,378,379,5,108,0,0,
		379,380,5,101,0,0,380,381,5,116,0,0,381,48,1,0,0,0,382,383,5,109,0,0,383,
		384,5,97,0,0,384,385,5,105,0,0,385,386,5,110,0,0,386,50,1,0,0,0,387,388,
		5,112,0,0,388,389,5,97,0,0,389,390,5,114,0,0,390,391,5,116,0,0,391,392,
		5,105,0,0,392,393,5,97,0,0,393,394,5,108,0,0,394,52,1,0,0,0,395,396,5,
		112,0,0,396,397,5,114,0,0,397,398,5,105,0,0,398,399,5,118,0,0,399,400,
		5,97,0,0,400,401,5,116,0,0,401,402,5,101,0,0,402,54,1,0,0,0,403,404,5,
		112,0,0,404,405,5,114,0,0,405,406,5,111,0,0,406,407,5,99,0,0,407,408,5,
		101,0,0,408,409,5,100,0,0,409,410,5,117,0,0,410,411,5,114,0,0,411,412,
		5,101,0,0,412,56,1,0,0,0,413,414,5,112,0,0,414,415,5,114,0,0,415,416,5,
		111,0,0,416,417,5,112,0,0,417,418,5,101,0,0,418,419,5,114,0,0,419,420,
		5,116,0,0,420,421,5,121,0,0,421,58,1,0,0,0,422,423,5,114,0,0,423,424,5,
		101,0,0,424,425,5,112,0,0,425,426,5,101,0,0,426,427,5,97,0,0,427,428,5,
		116,0,0,428,60,1,0,0,0,429,430,5,114,0,0,430,431,5,101,0,0,431,432,5,116,
		0,0,432,433,5,117,0,0,433,434,5,114,0,0,434,435,5,110,0,0,435,62,1,0,0,
		0,436,437,5,115,0,0,437,438,5,101,0,0,438,439,5,108,0,0,439,440,5,102,
		0,0,440,64,1,0,0,0,441,442,5,115,0,0,442,443,5,116,0,0,443,444,5,101,0,
		0,444,445,5,112,0,0,445,66,1,0,0,0,446,447,5,115,0,0,447,448,5,119,0,0,
		448,449,5,105,0,0,449,450,5,116,0,0,450,451,5,99,0,0,451,452,5,104,0,0,
		452,68,1,0,0,0,453,454,5,116,0,0,454,455,5,101,0,0,455,456,5,115,0,0,456,
		457,5,116,0,0,457,70,1,0,0,0,458,459,5,116,0,0,459,460,5,104,0,0,460,461,
		5,101,0,0,461,462,5,110,0,0,462,72,1,0,0,0,463,464,5,116,0,0,464,465,5,
		111,0,0,465,74,1,0,0,0,466,467,5,116,0,0,467,468,5,114,0,0,468,469,5,121,
		0,0,469,76,1,0,0,0,470,471,5,117,0,0,471,472,5,110,0,0,472,473,5,116,0,
		0,473,474,5,105,0,0,474,475,5,108,0,0,475,78,1,0,0,0,476,477,5,118,0,0,
		477,478,5,97,0,0,478,479,5,114,0,0,479,80,1,0,0,0,480,481,5,119,0,0,481,
		482,5,104,0,0,482,483,5,105,0,0,483,484,5,108,0,0,484,485,5,101,0,0,485,
		82,1,0,0,0,486,487,5,119,0,0,487,488,5,105,0,0,488,489,5,116,0,0,489,490,
		5,104,0,0,490,84,1,0,0,0,491,492,5,116,0,0,492,493,5,114,0,0,493,494,5,
		117,0,0,494,501,5,101,0,0,495,496,5,102,0,0,496,497,5,97,0,0,497,498,5,
		108,0,0,498,499,5,115,0,0,499,501,5,101,0,0,500,491,1,0,0,0,500,495,1,
		0,0,0,501,86,1,0,0,0,502,503,5,73,0,0,503,504,5,110,0,0,504,525,5,116,
		0,0,505,506,5,70,0,0,506,507,5,108,0,0,507,508,5,111,0,0,508,509,5,97,
		0,0,509,525,5,116,0,0,510,511,5,67,0,0,511,512,5,104,0,0,512,513,5,97,
		0,0,513,525,5,114,0,0,514,515,5,83,0,0,515,516,5,116,0,0,516,517,5,114,
		0,0,517,518,5,105,0,0,518,519,5,110,0,0,519,525,5,103,0,0,520,521,5,66,
		0,0,521,522,5,111,0,0,522,523,5,111,0,0,523,525,5,108,0,0,524,502,1,0,
		0,0,524,505,1,0,0,0,524,510,1,0,0,0,524,514,1,0,0,0,524,520,1,0,0,0,525,
		88,1,0,0,0,526,527,5,65,0,0,527,528,5,114,0,0,528,529,5,114,0,0,529,530,
		5,97,0,0,530,531,5,121,0,0,531,90,1,0,0,0,532,533,5,76,0,0,533,534,5,105,
		0,0,534,535,5,115,0,0,535,536,5,116,0,0,536,92,1,0,0,0,537,538,5,68,0,
		0,538,539,5,105,0,0,539,540,5,99,0,0,540,541,5,116,0,0,541,542,5,105,0,
		0,542,543,5,111,0,0,543,544,5,110,0,0,544,545,5,97,0,0,545,546,5,114,0,
		0,546,547,5,121,0,0,547,94,1,0,0,0,548,549,5,73,0,0,549,550,5,116,0,0,
		550,551,5,101,0,0,551,552,5,114,0,0,552,96,1,0,0,0,553,554,5,61,0,0,554,
		98,1,0,0,0,555,556,5,45,0,0,556,557,5,62,0,0,557,100,1,0,0,0,558,559,5,
		123,0,0,559,102,1,0,0,0,560,561,5,125,0,0,561,104,1,0,0,0,562,563,5,91,
		0,0,563,106,1,0,0,0,564,565,5,93,0,0,565,108,1,0,0,0,566,567,5,40,0,0,
		567,110,1,0,0,0,568,569,5,41,0,0,569,112,1,0,0,0,570,571,5,46,0,0,571,
		572,5,46,0,0,572,114,1,0,0,0,573,574,5,46,0,0,574,116,1,0,0,0,575,576,
		5,44,0,0,576,118,1,0,0,0,577,578,5,58,0,0,578,120,1,0,0,0,579,580,5,43,
		0,0,580,122,1,0,0,0,581,582,5,45,0,0,582,124,1,0,0,0,583,584,5,42,0,0,
		584,126,1,0,0,0,585,586,5,47,0,0,586,128,1,0,0,0,587,588,5,94,0,0,588,
		130,1,0,0,0,589,590,5,109,0,0,590,591,5,111,0,0,591,592,5,100,0,0,592,
		132,1,0,0,0,593,594,5,100,0,0,594,595,5,105,0,0,595,596,5,118,0,0,596,
		134,1,0,0,0,597,598,5,60,0,0,598,136,1,0,0,0,599,600,5,62,0,0,600,138,
		1,0,0,0,601,602,5,97,0,0,602,603,5,110,0,0,603,604,5,100,0,0,604,140,1,
		0,0,0,605,606,5,110,0,0,606,607,5,111,0,0,607,608,5,116,0,0,608,142,1,
		0,0,0,609,610,5,111,0,0,610,611,5,114,0,0,611,144,1,0,0,0,612,613,5,120,
		0,0,613,614,5,111,0,0,614,615,5,114,0,0,615,146,1,0,0,0,616,617,5,61,0,
		0,617,621,5,61,0,0,618,619,5,105,0,0,619,621,5,115,0,0,620,616,1,0,0,0,
		620,618,1,0,0,0,621,148,1,0,0,0,622,623,5,60,0,0,623,637,5,62,0,0,624,
		625,5,105,0,0,625,626,5,115,0,0,626,630,1,0,0,0,627,629,3,183,91,0,628,
		627,1,0,0,0,629,632,1,0,0,0,630,628,1,0,0,0,630,631,1,0,0,0,631,633,1,
		0,0,0,632,630,1,0,0,0,633,634,5,110,0,0,634,635,5,111,0,0,635,637,5,116,
		0,0,636,622,1,0,0,0,636,624,1,0,0,0,637,150,1,0,0,0,638,639,5,60,0,0,639,
		640,5,61,0,0,640,152,1,0,0,0,641,642,5,62,0,0,642,643,5,61,0,0,643,154,
		1,0,0,0,644,645,3,191,95,0,645,156,1,0,0,0,646,647,3,189,94,0,647,158,
		1,0,0,0,648,652,7,1,0,0,649,651,7,1,0,0,650,649,1,0,0,0,651,654,1,0,0,
		0,652,650,1,0,0,0,652,653,1,0,0,0,653,160,1,0,0,0,654,652,1,0,0,0,655,
		656,3,159,79,0,656,657,3,115,57,0,657,659,3,159,79,0,658,660,3,173,86,
		0,659,658,1,0,0,0,659,660,1,0,0,0,660,162,1,0,0,0,661,664,5,39,0,0,662,
		665,8,2,0,0,663,665,3,175,87,0,664,662,1,0,0,0,664,663,1,0,0,0,665,666,
		1,0,0,0,666,667,5,39,0,0,667,164,1,0,0,0,668,673,5,34,0,0,669,672,8,3,
		0,0,670,672,3,175,87,0,671,669,1,0,0,0,671,670,1,0,0,0,672,675,1,0,0,0,
		673,671,1,0,0,0,673,674,1,0,0,0,674,676,1,0,0,0,675,673,1,0,0,0,676,677,
		5,34,0,0,677,166,1,0,0,0,678,680,3,183,91,0,679,678,1,0,0,0,680,681,1,
		0,0,0,681,679,1,0,0,0,681,682,1,0,0,0,682,683,1,0,0,0,683,684,6,83,0,0,
		684,168,1,0,0,0,685,686,8,4,0,0,686,170,1,0,0,0,687,688,7,4,0,0,688,172,
		1,0,0,0,689,692,7,5,0,0,690,693,3,121,60,0,691,693,3,123,61,0,692,690,
		1,0,0,0,692,691,1,0,0,0,692,693,1,0,0,0,693,694,1,0,0,0,694,695,3,159,
		79,0,695,174,1,0,0,0,696,700,3,177,88,0,697,700,3,179,89,0,698,700,3,203,
		101,0,699,696,1,0,0,0,699,697,1,0,0,0,699,698,1,0,0,0,700,176,1,0,0,0,
		701,702,5,92,0,0,702,724,5,39,0,0,703,704,5,92,0,0,704,724,5,34,0,0,705,
		706,5,92,0,0,706,724,5,92,0,0,707,708,5,92,0,0,708,724,5,48,0,0,709,710,
		5,92,0,0,710,724,5,97,0,0,711,712,5,92,0,0,712,724,5,98,0,0,713,714,5,
		92,0,0,714,724,5,102,0,0,715,716,5,92,0,0,716,724,5,110,0,0,717,718,5,
		92,0,0,718,724,5,114,0,0,719,720,5,92,0,0,720,724,5,116,0,0,721,722,5,
		92,0,0,722,724,5,118,0,0,723,701,1,0,0,0,723,703,1,0,0,0,723,705,1,0,0,
		0,723,707,1,0,0,0,723,709,1,0,0,0,723,711,1,0,0,0,723,713,1,0,0,0,723,
		715,1,0,0,0,723,717,1,0,0,0,723,719,1,0,0,0,723,721,1,0,0,0,724,178,1,
		0,0,0,725,726,5,92,0,0,726,727,5,120,0,0,727,728,1,0,0,0,728,751,3,205,
		102,0,729,730,5,92,0,0,730,731,5,120,0,0,731,732,1,0,0,0,732,733,3,205,
		102,0,733,734,3,205,102,0,734,751,1,0,0,0,735,736,5,92,0,0,736,737,5,120,
		0,0,737,738,1,0,0,0,738,739,3,205,102,0,739,740,3,205,102,0,740,741,3,
		205,102,0,741,751,1,0,0,0,742,743,5,92,0,0,743,744,5,120,0,0,744,745,1,
		0,0,0,745,746,3,205,102,0,746,747,3,205,102,0,747,748,3,205,102,0,748,
		749,3,205,102,0,749,751,1,0,0,0,750,725,1,0,0,0,750,729,1,0,0,0,750,735,
		1,0,0,0,750,742,1,0,0,0,751,180,1,0,0,0,752,753,5,13,0,0,753,756,5,10,
		0,0,754,756,7,4,0,0,755,752,1,0,0,0,755,754,1,0,0,0,756,182,1,0,0,0,757,
		760,3,185,92,0,758,760,7,6,0,0,759,757,1,0,0,0,759,758,1,0,0,0,760,184,
		1,0,0,0,761,762,7,7,0,0,762,186,1,0,0,0,763,766,3,209,104,0,764,766,3,
		207,103,0,765,763,1,0,0,0,765,764,1,0,0,0,766,770,1,0,0,0,767,769,3,193,
		96,0,768,767,1,0,0,0,769,772,1,0,0,0,770,768,1,0,0,0,770,771,1,0,0,0,771,
		188,1,0,0,0,772,770,1,0,0,0,773,777,3,209,104,0,774,776,3,193,96,0,775,
		774,1,0,0,0,776,779,1,0,0,0,777,775,1,0,0,0,777,778,1,0,0,0,778,190,1,
		0,0,0,779,777,1,0,0,0,780,784,3,207,103,0,781,783,3,193,96,0,782,781,1,
		0,0,0,783,786,1,0,0,0,784,782,1,0,0,0,784,785,1,0,0,0,785,192,1,0,0,0,
		786,784,1,0,0,0,787,792,3,207,103,0,788,792,3,209,104,0,789,792,3,197,
		98,0,790,792,5,95,0,0,791,787,1,0,0,0,791,788,1,0,0,0,791,789,1,0,0,0,
		791,790,1,0,0,0,792,194,1,0,0,0,793,797,3,207,103,0,794,797,3,209,104,
		0,795,797,3,203,101,0,796,793,1,0,0,0,796,794,1,0,0,0,796,795,1,0,0,0,
		797,196,1,0,0,0,798,801,3,211,105,0,799,801,3,203,101,0,800,798,1,0,0,
		0,800,799,1,0,0,0,801,198,1,0,0,0,802,803,3,203,101,0,803,200,1,0,0,0,
		804,805,3,203,101,0,805,202,1,0,0,0,806,807,5,92,0,0,807,808,5,117,0,0,
		808,809,1,0,0,0,809,810,3,205,102,0,810,811,3,205,102,0,811,812,3,205,
		102,0,812,813,3,205,102,0,813,827,1,0,0,0,814,815,5,92,0,0,815,816,5,85,
		0,0,816,817,1,0,0,0,817,818,3,205,102,0,818,819,3,205,102,0,819,820,3,
		205,102,0,820,821,3,205,102,0,821,822,3,205,102,0,822,823,3,205,102,0,
		823,824,3,205,102,0,824,825,3,205,102,0,825,827,1,0,0,0,826,806,1,0,0,
		0,826,814,1,0,0,0,827,204,1,0,0,0,828,830,7,8,0,0,829,828,1,0,0,0,830,
		206,1,0,0,0,831,832,2,65,90,0,832,208,1,0,0,0,833,834,2,97,122,0,834,210,
		1,0,0,0,835,836,2,48,57,0,836,212,1,0,0,0,837,839,5,13,0,0,838,837,1,0,
		0,0,838,839,1,0,0,0,839,840,1,0,0,0,840,843,5,10,0,0,841,843,5,13,0,0,
		842,838,1,0,0,0,842,841,1,0,0,0,843,214,1,0,0,0,844,846,7,9,0,0,845,844,
		1,0,0,0,846,847,1,0,0,0,847,845,1,0,0,0,847,848,1,0,0,0,848,849,1,0,0,
		0,849,850,6,107,0,0,850,216,1,0,0,0,33,0,220,223,229,500,524,620,630,636,
		652,659,664,671,673,681,692,699,723,750,755,759,765,770,777,784,791,796,
		800,826,829,838,842,847,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
