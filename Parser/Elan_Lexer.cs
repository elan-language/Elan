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
		NL=1, SINGLE_LINE_COMMENT=2, COMMENT_MARKER=3, ABSTRACT=4, CALL=5, CASE=6, 
		CATCH=7, CLASS=8, CONSTANT=9, CONSTRUCTOR=10, CURRY=11, DEFAULT=12, ELSE=13, 
		END=14, ENUM=15, FOR=16, FOREACH=17, FUNCTION=18, GLOBAL=19, IF=20, IMMUTABLE=21, 
		IN=22, INHERITS=23, LAMBDA=24, LET=25, MAIN=26, PARTIAL=27, PRINT=28, 
		PRIVATE=29, PROCEDURE=30, PROPERTY=31, REPEAT=32, RETURN=33, SELF=34, 
		SET=35, STEP=36, SWITCH=37, SYSTEM=38, TEST=39, THEN=40, THROW=41, TO=42, 
		TRY=43, UNTIL=44, VAR=45, WHILE=46, WITH=47, BOOL_VALUE=48, VALUE_TYPE=49, 
		ARRAY=50, LIST=51, DICTIONARY=52, ITERABLE=53, ASSIGN=54, ARROW=55, OPEN_BRACE=56, 
		CLOSE_BRACE=57, OPEN_SQ_BRACKET=58, CLOSE_SQ_BRACKET=59, OPEN_BRACKET=60, 
		CLOSE_BRACKET=61, DOUBLE_DOT=62, DOT=63, COMMA=64, COLON=65, PLUS=66, 
		MINUS=67, MULT=68, DIVIDE=69, POWER=70, MOD=71, INT_DIV=72, LT=73, GT=74, 
		OP_AND=75, OP_NOT=76, OP_OR=77, OP_XOR=78, OP_EQ=79, OP_NE=80, OP_LE=81, 
		OP_GE=82, TYPENAME=83, IDENTIFIER=84, LITERAL_INTEGER=85, LITERAL_FLOAT=86, 
		LITERAL_CHAR=87, LITERAL_STRING=88, WHITESPACES=89, NEWLINE=90, WS=91;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "ABSTRACT", "CALL", "CASE", 
		"CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", "DEFAULT", "ELSE", 
		"END", "ENUM", "FOR", "FOREACH", "FUNCTION", "GLOBAL", "IF", "IMMUTABLE", 
		"IN", "INHERITS", "LAMBDA", "LET", "MAIN", "PARTIAL", "PRINT", "PRIVATE", 
		"PROCEDURE", "PROPERTY", "REPEAT", "RETURN", "SELF", "SET", "STEP", "SWITCH", 
		"SYSTEM", "TEST", "THEN", "THROW", "TO", "TRY", "UNTIL", "VAR", "WHILE", 
		"WITH", "BOOL_VALUE", "VALUE_TYPE", "ARRAY", "LIST", "DICTIONARY", "ITERABLE", 
		"ASSIGN", "ARROW", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", 
		"OPEN_BRACKET", "CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", 
		"PLUS", "MINUS", "MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", 
		"OP_AND", "OP_NOT", "OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", 
		"TYPENAME", "IDENTIFIER", "LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_CHAR", 
		"LITERAL_STRING", "WHITESPACES", "InputCharacter", "NewLineCharacter", 
		"ExponentPart", "CommonCharacter", "SimpleEscapeSequence", "HexEscapeSequence", 
		"NewLine", "Whitespace", "UnicodeClassZS", "IdentifierStartingUCorLC", 
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
		null, null, null, "'#'", "'abstract'", "'call'", "'case'", "'catch'", 
		"'class'", "'constant'", "'constructor'", "'curry'", "'default'", "'else'", 
		"'end'", "'enum'", "'for'", "'foreach'", "'function'", "'global'", "'if'", 
		"'immutable'", "'in'", "'inherits'", "'lambda'", "'let'", "'main'", "'partial'", 
		"'print'", "'private'", "'procedure'", "'property'", "'repeat'", "'return'", 
		"'self'", "'set'", "'step'", "'switch'", "'system'", "'test'", "'then'", 
		"'throw'", "'to'", "'try'", "'until'", "'var'", "'while'", "'with'", null, 
		null, "'Array'", "'List'", "'Dictionary'", "'Iter'", "'='", "'->'", "'{'", 
		"'}'", "'['", "']'", "'('", "')'", "'..'", "'.'", "','", "':'", "'+'", 
		"'-'", "'*'", "'/'", "'^'", "'mod'", "'div'", "'<'", "'>'", "'and'", "'not'", 
		"'or'", "'xor'", "'is'", null, "'<='", "'>='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "NL", "SINGLE_LINE_COMMENT", "COMMENT_MARKER", "ABSTRACT", "CALL", 
		"CASE", "CATCH", "CLASS", "CONSTANT", "CONSTRUCTOR", "CURRY", "DEFAULT", 
		"ELSE", "END", "ENUM", "FOR", "FOREACH", "FUNCTION", "GLOBAL", "IF", "IMMUTABLE", 
		"IN", "INHERITS", "LAMBDA", "LET", "MAIN", "PARTIAL", "PRINT", "PRIVATE", 
		"PROCEDURE", "PROPERTY", "REPEAT", "RETURN", "SELF", "SET", "STEP", "SWITCH", 
		"SYSTEM", "TEST", "THEN", "THROW", "TO", "TRY", "UNTIL", "VAR", "WHILE", 
		"WITH", "BOOL_VALUE", "VALUE_TYPE", "ARRAY", "LIST", "DICTIONARY", "ITERABLE", 
		"ASSIGN", "ARROW", "OPEN_BRACE", "CLOSE_BRACE", "OPEN_SQ_BRACKET", "CLOSE_SQ_BRACKET", 
		"OPEN_BRACKET", "CLOSE_BRACKET", "DOUBLE_DOT", "DOT", "COMMA", "COLON", 
		"PLUS", "MINUS", "MULT", "DIVIDE", "POWER", "MOD", "INT_DIV", "LT", "GT", 
		"OP_AND", "OP_NOT", "OP_OR", "OP_XOR", "OP_EQ", "OP_NE", "OP_LE", "OP_GE", 
		"TYPENAME", "IDENTIFIER", "LITERAL_INTEGER", "LITERAL_FLOAT", "LITERAL_CHAR", 
		"LITERAL_STRING", "WHITESPACES", "NEWLINE", "WS"
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
		4,0,91,876,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		7,110,2,111,7,111,2,112,7,112,1,0,4,0,229,8,0,11,0,12,0,230,1,1,3,1,234,
		8,1,1,1,1,1,5,1,238,8,1,10,1,12,1,241,9,1,1,1,1,1,1,2,1,2,1,3,1,3,1,3,
		1,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,6,1,
		6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,8,
		1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,
		1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,
		1,12,1,12,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,
		1,15,1,16,1,16,1,16,1,16,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,
		1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,18,1,18,1,19,1,19,1,19,
		1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,21,1,21,1,21,1,22,
		1,22,1,22,1,22,1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,23,1,23,1,23,
		1,23,1,24,1,24,1,24,1,24,1,25,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,
		1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,1,27,1,27,1,28,1,28,1,28,1,28,
		1,28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,29,
		1,30,1,30,1,30,1,30,1,30,1,30,1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,
		1,31,1,31,1,32,1,32,1,32,1,32,1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,33,
		1,34,1,34,1,34,1,34,1,35,1,35,1,35,1,35,1,35,1,36,1,36,1,36,1,36,1,36,
		1,36,1,36,1,37,1,37,1,37,1,37,1,37,1,37,1,37,1,38,1,38,1,38,1,38,1,38,
		1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,40,1,40,1,41,1,41,1,41,
		1,42,1,42,1,42,1,42,1,43,1,43,1,43,1,43,1,43,1,43,1,44,1,44,1,44,1,44,
		1,45,1,45,1,45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,
		1,47,1,47,1,47,1,47,1,47,1,47,3,47,532,8,47,1,48,1,48,1,48,1,48,1,48,1,
		48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,48,1,
		48,1,48,1,48,3,48,556,8,48,1,49,1,49,1,49,1,49,1,49,1,49,1,50,1,50,1,50,
		1,50,1,50,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,51,1,52,
		1,52,1,52,1,52,1,52,1,53,1,53,1,54,1,54,1,54,1,55,1,55,1,56,1,56,1,57,
		1,57,1,58,1,58,1,59,1,59,1,60,1,60,1,61,1,61,1,61,1,62,1,62,1,63,1,63,
		1,64,1,64,1,65,1,65,1,66,1,66,1,67,1,67,1,68,1,68,1,69,1,69,1,70,1,70,
		1,70,1,70,1,71,1,71,1,71,1,71,1,72,1,72,1,73,1,73,1,74,1,74,1,74,1,74,
		1,75,1,75,1,75,1,75,1,76,1,76,1,76,1,77,1,77,1,77,1,77,1,78,1,78,1,78,
		1,79,1,79,1,79,1,79,5,79,655,8,79,10,79,12,79,658,9,79,1,79,1,79,1,79,
		1,79,1,80,1,80,1,80,1,81,1,81,1,81,1,82,1,82,1,83,1,83,1,84,1,84,5,84,
		676,8,84,10,84,12,84,679,9,84,1,85,1,85,1,85,1,85,3,85,685,8,85,1,86,1,
		86,1,86,3,86,690,8,86,1,86,1,86,1,87,1,87,1,87,5,87,697,8,87,10,87,12,
		87,700,9,87,1,87,1,87,1,88,4,88,705,8,88,11,88,12,88,706,1,88,1,88,1,89,
		1,89,1,90,1,90,1,91,1,91,1,91,3,91,718,8,91,1,91,1,91,1,92,1,92,1,92,3,
		92,725,8,92,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,
		1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,3,93,749,8,93,1,94,1,
		94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,
		94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,1,94,3,94,776,8,94,1,95,1,95,
		1,95,3,95,781,8,95,1,96,1,96,3,96,785,8,96,1,97,1,97,1,98,1,98,3,98,791,
		8,98,1,98,5,98,794,8,98,10,98,12,98,797,9,98,1,99,1,99,5,99,801,8,99,10,
		99,12,99,804,9,99,1,100,1,100,5,100,808,8,100,10,100,12,100,811,9,100,
		1,101,1,101,1,101,1,101,3,101,817,8,101,1,102,1,102,1,102,3,102,822,8,
		102,1,103,1,103,3,103,826,8,103,1,104,1,104,1,105,1,105,1,106,1,106,1,
		106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,
		1,106,1,106,1,106,1,106,1,106,1,106,3,106,852,8,106,1,107,3,107,855,8,
		107,1,108,1,108,1,109,1,109,1,110,1,110,1,111,3,111,864,8,111,1,111,1,
		111,3,111,868,8,111,1,112,4,112,871,8,112,11,112,12,112,872,1,112,1,112,
		0,0,113,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,
		27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,
		51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,
		75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,46,93,47,95,48,97,49,
		99,50,101,51,103,52,105,53,107,54,109,55,111,56,113,57,115,58,117,59,119,
		60,121,61,123,62,125,63,127,64,129,65,131,66,133,67,135,68,137,69,139,
		70,141,71,143,72,145,73,147,74,149,75,151,76,153,77,155,78,157,79,159,
		80,161,81,163,82,165,83,167,84,169,85,171,86,173,87,175,88,177,89,179,
		0,181,0,183,0,185,0,187,0,189,0,191,0,193,0,195,0,197,0,199,0,201,0,203,
		0,205,0,207,0,209,0,211,0,213,0,215,0,217,0,219,0,221,0,223,90,225,91,
		1,0,10,2,0,10,10,12,13,1,0,48,57,5,0,10,10,13,13,39,39,92,92,133,133,2,
		0,34,34,133,133,3,0,10,10,13,13,133,133,2,0,69,69,101,101,2,0,9,9,11,12,
		2,0,32,32,160,160,3,0,48,57,65,70,97,102,2,0,9,9,32,32,901,0,1,1,0,0,0,
		0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,
		0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,
		25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,
		0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,
		0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,
		1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,
		0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,
		1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,
		0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,
		1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,
		1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,
		1,0,0,0,0,123,1,0,0,0,0,125,1,0,0,0,0,127,1,0,0,0,0,129,1,0,0,0,0,131,
		1,0,0,0,0,133,1,0,0,0,0,135,1,0,0,0,0,137,1,0,0,0,0,139,1,0,0,0,0,141,
		1,0,0,0,0,143,1,0,0,0,0,145,1,0,0,0,0,147,1,0,0,0,0,149,1,0,0,0,0,151,
		1,0,0,0,0,153,1,0,0,0,0,155,1,0,0,0,0,157,1,0,0,0,0,159,1,0,0,0,0,161,
		1,0,0,0,0,163,1,0,0,0,0,165,1,0,0,0,0,167,1,0,0,0,0,169,1,0,0,0,0,171,
		1,0,0,0,0,173,1,0,0,0,0,175,1,0,0,0,0,177,1,0,0,0,0,223,1,0,0,0,0,225,
		1,0,0,0,1,228,1,0,0,0,3,233,1,0,0,0,5,244,1,0,0,0,7,246,1,0,0,0,9,255,
		1,0,0,0,11,260,1,0,0,0,13,265,1,0,0,0,15,271,1,0,0,0,17,277,1,0,0,0,19,
		286,1,0,0,0,21,298,1,0,0,0,23,304,1,0,0,0,25,312,1,0,0,0,27,317,1,0,0,
		0,29,321,1,0,0,0,31,326,1,0,0,0,33,330,1,0,0,0,35,338,1,0,0,0,37,347,1,
		0,0,0,39,354,1,0,0,0,41,357,1,0,0,0,43,367,1,0,0,0,45,370,1,0,0,0,47,379,
		1,0,0,0,49,386,1,0,0,0,51,390,1,0,0,0,53,395,1,0,0,0,55,403,1,0,0,0,57,
		409,1,0,0,0,59,417,1,0,0,0,61,427,1,0,0,0,63,436,1,0,0,0,65,443,1,0,0,
		0,67,450,1,0,0,0,69,455,1,0,0,0,71,459,1,0,0,0,73,464,1,0,0,0,75,471,1,
		0,0,0,77,478,1,0,0,0,79,483,1,0,0,0,81,488,1,0,0,0,83,494,1,0,0,0,85,497,
		1,0,0,0,87,501,1,0,0,0,89,507,1,0,0,0,91,511,1,0,0,0,93,517,1,0,0,0,95,
		531,1,0,0,0,97,555,1,0,0,0,99,557,1,0,0,0,101,563,1,0,0,0,103,568,1,0,
		0,0,105,579,1,0,0,0,107,584,1,0,0,0,109,586,1,0,0,0,111,589,1,0,0,0,113,
		591,1,0,0,0,115,593,1,0,0,0,117,595,1,0,0,0,119,597,1,0,0,0,121,599,1,
		0,0,0,123,601,1,0,0,0,125,604,1,0,0,0,127,606,1,0,0,0,129,608,1,0,0,0,
		131,610,1,0,0,0,133,612,1,0,0,0,135,614,1,0,0,0,137,616,1,0,0,0,139,618,
		1,0,0,0,141,620,1,0,0,0,143,624,1,0,0,0,145,628,1,0,0,0,147,630,1,0,0,
		0,149,632,1,0,0,0,151,636,1,0,0,0,153,640,1,0,0,0,155,643,1,0,0,0,157,
		647,1,0,0,0,159,650,1,0,0,0,161,663,1,0,0,0,163,666,1,0,0,0,165,669,1,
		0,0,0,167,671,1,0,0,0,169,673,1,0,0,0,171,680,1,0,0,0,173,686,1,0,0,0,
		175,693,1,0,0,0,177,704,1,0,0,0,179,710,1,0,0,0,181,712,1,0,0,0,183,714,
		1,0,0,0,185,724,1,0,0,0,187,748,1,0,0,0,189,775,1,0,0,0,191,780,1,0,0,
		0,193,784,1,0,0,0,195,786,1,0,0,0,197,790,1,0,0,0,199,798,1,0,0,0,201,
		805,1,0,0,0,203,816,1,0,0,0,205,821,1,0,0,0,207,825,1,0,0,0,209,827,1,
		0,0,0,211,829,1,0,0,0,213,851,1,0,0,0,215,854,1,0,0,0,217,856,1,0,0,0,
		219,858,1,0,0,0,221,860,1,0,0,0,223,867,1,0,0,0,225,870,1,0,0,0,227,229,
		7,0,0,0,228,227,1,0,0,0,229,230,1,0,0,0,230,228,1,0,0,0,230,231,1,0,0,
		0,231,2,1,0,0,0,232,234,3,1,0,0,233,232,1,0,0,0,233,234,1,0,0,0,234,235,
		1,0,0,0,235,239,3,5,2,0,236,238,3,179,89,0,237,236,1,0,0,0,238,241,1,0,
		0,0,239,237,1,0,0,0,239,240,1,0,0,0,240,242,1,0,0,0,241,239,1,0,0,0,242,
		243,6,1,0,0,243,4,1,0,0,0,244,245,5,35,0,0,245,6,1,0,0,0,246,247,5,97,
		0,0,247,248,5,98,0,0,248,249,5,115,0,0,249,250,5,116,0,0,250,251,5,114,
		0,0,251,252,5,97,0,0,252,253,5,99,0,0,253,254,5,116,0,0,254,8,1,0,0,0,
		255,256,5,99,0,0,256,257,5,97,0,0,257,258,5,108,0,0,258,259,5,108,0,0,
		259,10,1,0,0,0,260,261,5,99,0,0,261,262,5,97,0,0,262,263,5,115,0,0,263,
		264,5,101,0,0,264,12,1,0,0,0,265,266,5,99,0,0,266,267,5,97,0,0,267,268,
		5,116,0,0,268,269,5,99,0,0,269,270,5,104,0,0,270,14,1,0,0,0,271,272,5,
		99,0,0,272,273,5,108,0,0,273,274,5,97,0,0,274,275,5,115,0,0,275,276,5,
		115,0,0,276,16,1,0,0,0,277,278,5,99,0,0,278,279,5,111,0,0,279,280,5,110,
		0,0,280,281,5,115,0,0,281,282,5,116,0,0,282,283,5,97,0,0,283,284,5,110,
		0,0,284,285,5,116,0,0,285,18,1,0,0,0,286,287,5,99,0,0,287,288,5,111,0,
		0,288,289,5,110,0,0,289,290,5,115,0,0,290,291,5,116,0,0,291,292,5,114,
		0,0,292,293,5,117,0,0,293,294,5,99,0,0,294,295,5,116,0,0,295,296,5,111,
		0,0,296,297,5,114,0,0,297,20,1,0,0,0,298,299,5,99,0,0,299,300,5,117,0,
		0,300,301,5,114,0,0,301,302,5,114,0,0,302,303,5,121,0,0,303,22,1,0,0,0,
		304,305,5,100,0,0,305,306,5,101,0,0,306,307,5,102,0,0,307,308,5,97,0,0,
		308,309,5,117,0,0,309,310,5,108,0,0,310,311,5,116,0,0,311,24,1,0,0,0,312,
		313,5,101,0,0,313,314,5,108,0,0,314,315,5,115,0,0,315,316,5,101,0,0,316,
		26,1,0,0,0,317,318,5,101,0,0,318,319,5,110,0,0,319,320,5,100,0,0,320,28,
		1,0,0,0,321,322,5,101,0,0,322,323,5,110,0,0,323,324,5,117,0,0,324,325,
		5,109,0,0,325,30,1,0,0,0,326,327,5,102,0,0,327,328,5,111,0,0,328,329,5,
		114,0,0,329,32,1,0,0,0,330,331,5,102,0,0,331,332,5,111,0,0,332,333,5,114,
		0,0,333,334,5,101,0,0,334,335,5,97,0,0,335,336,5,99,0,0,336,337,5,104,
		0,0,337,34,1,0,0,0,338,339,5,102,0,0,339,340,5,117,0,0,340,341,5,110,0,
		0,341,342,5,99,0,0,342,343,5,116,0,0,343,344,5,105,0,0,344,345,5,111,0,
		0,345,346,5,110,0,0,346,36,1,0,0,0,347,348,5,103,0,0,348,349,5,108,0,0,
		349,350,5,111,0,0,350,351,5,98,0,0,351,352,5,97,0,0,352,353,5,108,0,0,
		353,38,1,0,0,0,354,355,5,105,0,0,355,356,5,102,0,0,356,40,1,0,0,0,357,
		358,5,105,0,0,358,359,5,109,0,0,359,360,5,109,0,0,360,361,5,117,0,0,361,
		362,5,116,0,0,362,363,5,97,0,0,363,364,5,98,0,0,364,365,5,108,0,0,365,
		366,5,101,0,0,366,42,1,0,0,0,367,368,5,105,0,0,368,369,5,110,0,0,369,44,
		1,0,0,0,370,371,5,105,0,0,371,372,5,110,0,0,372,373,5,104,0,0,373,374,
		5,101,0,0,374,375,5,114,0,0,375,376,5,105,0,0,376,377,5,116,0,0,377,378,
		5,115,0,0,378,46,1,0,0,0,379,380,5,108,0,0,380,381,5,97,0,0,381,382,5,
		109,0,0,382,383,5,98,0,0,383,384,5,100,0,0,384,385,5,97,0,0,385,48,1,0,
		0,0,386,387,5,108,0,0,387,388,5,101,0,0,388,389,5,116,0,0,389,50,1,0,0,
		0,390,391,5,109,0,0,391,392,5,97,0,0,392,393,5,105,0,0,393,394,5,110,0,
		0,394,52,1,0,0,0,395,396,5,112,0,0,396,397,5,97,0,0,397,398,5,114,0,0,
		398,399,5,116,0,0,399,400,5,105,0,0,400,401,5,97,0,0,401,402,5,108,0,0,
		402,54,1,0,0,0,403,404,5,112,0,0,404,405,5,114,0,0,405,406,5,105,0,0,406,
		407,5,110,0,0,407,408,5,116,0,0,408,56,1,0,0,0,409,410,5,112,0,0,410,411,
		5,114,0,0,411,412,5,105,0,0,412,413,5,118,0,0,413,414,5,97,0,0,414,415,
		5,116,0,0,415,416,5,101,0,0,416,58,1,0,0,0,417,418,5,112,0,0,418,419,5,
		114,0,0,419,420,5,111,0,0,420,421,5,99,0,0,421,422,5,101,0,0,422,423,5,
		100,0,0,423,424,5,117,0,0,424,425,5,114,0,0,425,426,5,101,0,0,426,60,1,
		0,0,0,427,428,5,112,0,0,428,429,5,114,0,0,429,430,5,111,0,0,430,431,5,
		112,0,0,431,432,5,101,0,0,432,433,5,114,0,0,433,434,5,116,0,0,434,435,
		5,121,0,0,435,62,1,0,0,0,436,437,5,114,0,0,437,438,5,101,0,0,438,439,5,
		112,0,0,439,440,5,101,0,0,440,441,5,97,0,0,441,442,5,116,0,0,442,64,1,
		0,0,0,443,444,5,114,0,0,444,445,5,101,0,0,445,446,5,116,0,0,446,447,5,
		117,0,0,447,448,5,114,0,0,448,449,5,110,0,0,449,66,1,0,0,0,450,451,5,115,
		0,0,451,452,5,101,0,0,452,453,5,108,0,0,453,454,5,102,0,0,454,68,1,0,0,
		0,455,456,5,115,0,0,456,457,5,101,0,0,457,458,5,116,0,0,458,70,1,0,0,0,
		459,460,5,115,0,0,460,461,5,116,0,0,461,462,5,101,0,0,462,463,5,112,0,
		0,463,72,1,0,0,0,464,465,5,115,0,0,465,466,5,119,0,0,466,467,5,105,0,0,
		467,468,5,116,0,0,468,469,5,99,0,0,469,470,5,104,0,0,470,74,1,0,0,0,471,
		472,5,115,0,0,472,473,5,121,0,0,473,474,5,115,0,0,474,475,5,116,0,0,475,
		476,5,101,0,0,476,477,5,109,0,0,477,76,1,0,0,0,478,479,5,116,0,0,479,480,
		5,101,0,0,480,481,5,115,0,0,481,482,5,116,0,0,482,78,1,0,0,0,483,484,5,
		116,0,0,484,485,5,104,0,0,485,486,5,101,0,0,486,487,5,110,0,0,487,80,1,
		0,0,0,488,489,5,116,0,0,489,490,5,104,0,0,490,491,5,114,0,0,491,492,5,
		111,0,0,492,493,5,119,0,0,493,82,1,0,0,0,494,495,5,116,0,0,495,496,5,111,
		0,0,496,84,1,0,0,0,497,498,5,116,0,0,498,499,5,114,0,0,499,500,5,121,0,
		0,500,86,1,0,0,0,501,502,5,117,0,0,502,503,5,110,0,0,503,504,5,116,0,0,
		504,505,5,105,0,0,505,506,5,108,0,0,506,88,1,0,0,0,507,508,5,118,0,0,508,
		509,5,97,0,0,509,510,5,114,0,0,510,90,1,0,0,0,511,512,5,119,0,0,512,513,
		5,104,0,0,513,514,5,105,0,0,514,515,5,108,0,0,515,516,5,101,0,0,516,92,
		1,0,0,0,517,518,5,119,0,0,518,519,5,105,0,0,519,520,5,116,0,0,520,521,
		5,104,0,0,521,94,1,0,0,0,522,523,5,116,0,0,523,524,5,114,0,0,524,525,5,
		117,0,0,525,532,5,101,0,0,526,527,5,102,0,0,527,528,5,97,0,0,528,529,5,
		108,0,0,529,530,5,115,0,0,530,532,5,101,0,0,531,522,1,0,0,0,531,526,1,
		0,0,0,532,96,1,0,0,0,533,534,5,73,0,0,534,535,5,110,0,0,535,556,5,116,
		0,0,536,537,5,70,0,0,537,538,5,108,0,0,538,539,5,111,0,0,539,540,5,97,
		0,0,540,556,5,116,0,0,541,542,5,67,0,0,542,543,5,104,0,0,543,544,5,97,
		0,0,544,556,5,114,0,0,545,546,5,83,0,0,546,547,5,116,0,0,547,548,5,114,
		0,0,548,549,5,105,0,0,549,550,5,110,0,0,550,556,5,103,0,0,551,552,5,66,
		0,0,552,553,5,111,0,0,553,554,5,111,0,0,554,556,5,108,0,0,555,533,1,0,
		0,0,555,536,1,0,0,0,555,541,1,0,0,0,555,545,1,0,0,0,555,551,1,0,0,0,556,
		98,1,0,0,0,557,558,5,65,0,0,558,559,5,114,0,0,559,560,5,114,0,0,560,561,
		5,97,0,0,561,562,5,121,0,0,562,100,1,0,0,0,563,564,5,76,0,0,564,565,5,
		105,0,0,565,566,5,115,0,0,566,567,5,116,0,0,567,102,1,0,0,0,568,569,5,
		68,0,0,569,570,5,105,0,0,570,571,5,99,0,0,571,572,5,116,0,0,572,573,5,
		105,0,0,573,574,5,111,0,0,574,575,5,110,0,0,575,576,5,97,0,0,576,577,5,
		114,0,0,577,578,5,121,0,0,578,104,1,0,0,0,579,580,5,73,0,0,580,581,5,116,
		0,0,581,582,5,101,0,0,582,583,5,114,0,0,583,106,1,0,0,0,584,585,5,61,0,
		0,585,108,1,0,0,0,586,587,5,45,0,0,587,588,5,62,0,0,588,110,1,0,0,0,589,
		590,5,123,0,0,590,112,1,0,0,0,591,592,5,125,0,0,592,114,1,0,0,0,593,594,
		5,91,0,0,594,116,1,0,0,0,595,596,5,93,0,0,596,118,1,0,0,0,597,598,5,40,
		0,0,598,120,1,0,0,0,599,600,5,41,0,0,600,122,1,0,0,0,601,602,5,46,0,0,
		602,603,5,46,0,0,603,124,1,0,0,0,604,605,5,46,0,0,605,126,1,0,0,0,606,
		607,5,44,0,0,607,128,1,0,0,0,608,609,5,58,0,0,609,130,1,0,0,0,610,611,
		5,43,0,0,611,132,1,0,0,0,612,613,5,45,0,0,613,134,1,0,0,0,614,615,5,42,
		0,0,615,136,1,0,0,0,616,617,5,47,0,0,617,138,1,0,0,0,618,619,5,94,0,0,
		619,140,1,0,0,0,620,621,5,109,0,0,621,622,5,111,0,0,622,623,5,100,0,0,
		623,142,1,0,0,0,624,625,5,100,0,0,625,626,5,105,0,0,626,627,5,118,0,0,
		627,144,1,0,0,0,628,629,5,60,0,0,629,146,1,0,0,0,630,631,5,62,0,0,631,
		148,1,0,0,0,632,633,5,97,0,0,633,634,5,110,0,0,634,635,5,100,0,0,635,150,
		1,0,0,0,636,637,5,110,0,0,637,638,5,111,0,0,638,639,5,116,0,0,639,152,
		1,0,0,0,640,641,5,111,0,0,641,642,5,114,0,0,642,154,1,0,0,0,643,644,5,
		120,0,0,644,645,5,111,0,0,645,646,5,114,0,0,646,156,1,0,0,0,647,648,5,
		105,0,0,648,649,5,115,0,0,649,158,1,0,0,0,650,651,5,105,0,0,651,652,5,
		115,0,0,652,656,1,0,0,0,653,655,3,193,96,0,654,653,1,0,0,0,655,658,1,0,
		0,0,656,654,1,0,0,0,656,657,1,0,0,0,657,659,1,0,0,0,658,656,1,0,0,0,659,
		660,5,110,0,0,660,661,5,111,0,0,661,662,5,116,0,0,662,160,1,0,0,0,663,
		664,5,60,0,0,664,665,5,61,0,0,665,162,1,0,0,0,666,667,5,62,0,0,667,668,
		5,61,0,0,668,164,1,0,0,0,669,670,3,201,100,0,670,166,1,0,0,0,671,672,3,
		199,99,0,672,168,1,0,0,0,673,677,7,1,0,0,674,676,7,1,0,0,675,674,1,0,0,
		0,676,679,1,0,0,0,677,675,1,0,0,0,677,678,1,0,0,0,678,170,1,0,0,0,679,
		677,1,0,0,0,680,681,3,169,84,0,681,682,3,125,62,0,682,684,3,169,84,0,683,
		685,3,183,91,0,684,683,1,0,0,0,684,685,1,0,0,0,685,172,1,0,0,0,686,689,
		5,39,0,0,687,690,8,2,0,0,688,690,3,185,92,0,689,687,1,0,0,0,689,688,1,
		0,0,0,690,691,1,0,0,0,691,692,5,39,0,0,692,174,1,0,0,0,693,698,5,34,0,
		0,694,697,8,3,0,0,695,697,3,185,92,0,696,694,1,0,0,0,696,695,1,0,0,0,697,
		700,1,0,0,0,698,696,1,0,0,0,698,699,1,0,0,0,699,701,1,0,0,0,700,698,1,
		0,0,0,701,702,5,34,0,0,702,176,1,0,0,0,703,705,3,193,96,0,704,703,1,0,
		0,0,705,706,1,0,0,0,706,704,1,0,0,0,706,707,1,0,0,0,707,708,1,0,0,0,708,
		709,6,88,0,0,709,178,1,0,0,0,710,711,8,4,0,0,711,180,1,0,0,0,712,713,7,
		4,0,0,713,182,1,0,0,0,714,717,7,5,0,0,715,718,3,131,65,0,716,718,3,133,
		66,0,717,715,1,0,0,0,717,716,1,0,0,0,717,718,1,0,0,0,718,719,1,0,0,0,719,
		720,3,169,84,0,720,184,1,0,0,0,721,725,3,187,93,0,722,725,3,189,94,0,723,
		725,3,213,106,0,724,721,1,0,0,0,724,722,1,0,0,0,724,723,1,0,0,0,725,186,
		1,0,0,0,726,727,5,92,0,0,727,749,5,39,0,0,728,729,5,92,0,0,729,749,5,34,
		0,0,730,731,5,92,0,0,731,749,5,92,0,0,732,733,5,92,0,0,733,749,5,48,0,
		0,734,735,5,92,0,0,735,749,5,97,0,0,736,737,5,92,0,0,737,749,5,98,0,0,
		738,739,5,92,0,0,739,749,5,102,0,0,740,741,5,92,0,0,741,749,5,110,0,0,
		742,743,5,92,0,0,743,749,5,114,0,0,744,745,5,92,0,0,745,749,5,116,0,0,
		746,747,5,92,0,0,747,749,5,118,0,0,748,726,1,0,0,0,748,728,1,0,0,0,748,
		730,1,0,0,0,748,732,1,0,0,0,748,734,1,0,0,0,748,736,1,0,0,0,748,738,1,
		0,0,0,748,740,1,0,0,0,748,742,1,0,0,0,748,744,1,0,0,0,748,746,1,0,0,0,
		749,188,1,0,0,0,750,751,5,92,0,0,751,752,5,120,0,0,752,753,1,0,0,0,753,
		776,3,215,107,0,754,755,5,92,0,0,755,756,5,120,0,0,756,757,1,0,0,0,757,
		758,3,215,107,0,758,759,3,215,107,0,759,776,1,0,0,0,760,761,5,92,0,0,761,
		762,5,120,0,0,762,763,1,0,0,0,763,764,3,215,107,0,764,765,3,215,107,0,
		765,766,3,215,107,0,766,776,1,0,0,0,767,768,5,92,0,0,768,769,5,120,0,0,
		769,770,1,0,0,0,770,771,3,215,107,0,771,772,3,215,107,0,772,773,3,215,
		107,0,773,774,3,215,107,0,774,776,1,0,0,0,775,750,1,0,0,0,775,754,1,0,
		0,0,775,760,1,0,0,0,775,767,1,0,0,0,776,190,1,0,0,0,777,778,5,13,0,0,778,
		781,5,10,0,0,779,781,7,4,0,0,780,777,1,0,0,0,780,779,1,0,0,0,781,192,1,
		0,0,0,782,785,3,195,97,0,783,785,7,6,0,0,784,782,1,0,0,0,784,783,1,0,0,
		0,785,194,1,0,0,0,786,787,7,7,0,0,787,196,1,0,0,0,788,791,3,219,109,0,
		789,791,3,217,108,0,790,788,1,0,0,0,790,789,1,0,0,0,791,795,1,0,0,0,792,
		794,3,203,101,0,793,792,1,0,0,0,794,797,1,0,0,0,795,793,1,0,0,0,795,796,
		1,0,0,0,796,198,1,0,0,0,797,795,1,0,0,0,798,802,3,219,109,0,799,801,3,
		203,101,0,800,799,1,0,0,0,801,804,1,0,0,0,802,800,1,0,0,0,802,803,1,0,
		0,0,803,200,1,0,0,0,804,802,1,0,0,0,805,809,3,217,108,0,806,808,3,203,
		101,0,807,806,1,0,0,0,808,811,1,0,0,0,809,807,1,0,0,0,809,810,1,0,0,0,
		810,202,1,0,0,0,811,809,1,0,0,0,812,817,3,217,108,0,813,817,3,219,109,
		0,814,817,3,207,103,0,815,817,5,95,0,0,816,812,1,0,0,0,816,813,1,0,0,0,
		816,814,1,0,0,0,816,815,1,0,0,0,817,204,1,0,0,0,818,822,3,217,108,0,819,
		822,3,219,109,0,820,822,3,213,106,0,821,818,1,0,0,0,821,819,1,0,0,0,821,
		820,1,0,0,0,822,206,1,0,0,0,823,826,3,221,110,0,824,826,3,213,106,0,825,
		823,1,0,0,0,825,824,1,0,0,0,826,208,1,0,0,0,827,828,3,213,106,0,828,210,
		1,0,0,0,829,830,3,213,106,0,830,212,1,0,0,0,831,832,5,92,0,0,832,833,5,
		117,0,0,833,834,1,0,0,0,834,835,3,215,107,0,835,836,3,215,107,0,836,837,
		3,215,107,0,837,838,3,215,107,0,838,852,1,0,0,0,839,840,5,92,0,0,840,841,
		5,85,0,0,841,842,1,0,0,0,842,843,3,215,107,0,843,844,3,215,107,0,844,845,
		3,215,107,0,845,846,3,215,107,0,846,847,3,215,107,0,847,848,3,215,107,
		0,848,849,3,215,107,0,849,850,3,215,107,0,850,852,1,0,0,0,851,831,1,0,
		0,0,851,839,1,0,0,0,852,214,1,0,0,0,853,855,7,8,0,0,854,853,1,0,0,0,855,
		216,1,0,0,0,856,857,2,65,90,0,857,218,1,0,0,0,858,859,2,97,122,0,859,220,
		1,0,0,0,860,861,2,48,57,0,861,222,1,0,0,0,862,864,5,13,0,0,863,862,1,0,
		0,0,863,864,1,0,0,0,864,865,1,0,0,0,865,868,5,10,0,0,866,868,5,13,0,0,
		867,863,1,0,0,0,867,866,1,0,0,0,868,224,1,0,0,0,869,871,7,9,0,0,870,869,
		1,0,0,0,871,872,1,0,0,0,872,870,1,0,0,0,872,873,1,0,0,0,873,874,1,0,0,
		0,874,875,6,112,0,0,875,226,1,0,0,0,31,0,230,233,239,531,555,656,677,684,
		689,696,698,706,717,724,748,775,780,784,790,795,802,809,816,821,825,851,
		854,863,867,872,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
