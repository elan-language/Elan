lexer grammar Elan_Lexer;

NL: [\r\n\f]+ ;
SINGLE_LINE_COMMENT: NL? COMMENT_MARKER InputCharacter*    -> skip; 

COMMENT_MARKER: '#';

// Keywords
ABSTRACT:      'abstract';
CALL:		   'call';
CASE: 		   'case';
CATCH:         'catch';
CLASS:         'class';
CONSTANT:      'constant';
CONSTRUCTOR:   'constructor';
CURRY:   	   'curry';
DEFAULT: 	   'default'; 
ELSE:          'else';
END:		   'end'; 
ENUM:          'enum';
FOR:           'for';
FOREACH:       'foreach';
FUNCTION:	   'function';
GLOBAL:			'global';
IF:            'if'; 
IMMUTABLE:	   'immutable';
IN:            'in';
INHERITS:      'inherits';
LAMBDA:		   'lambda';
LET:           'let';
MAIN:		   'main';
PARTIAL: 	   'partial';
PRINT:		   'print';
PRIVATE:       'private';
PROCEDURE:	   'procedure';
PROPERTY:      'property';
REPEAT:		   'repeat';
RETURN:        'return';
SELF:		   'self';
SET:	 	   'set';
STEP:		   'step';
SWITCH:        'switch';
SYSTEM:		   'system';
TEST:		   'test';
THEN:		   'then';
THROW:		   'throw';
TO:			   'to';
TRY:           'try';
UNTIL:         'until';
VAR:		   'var';
WHILE:         'while';
WITH: 		   'with';

BOOL_VALUE: 'true' | 'false';

// Types
VALUE_TYPE:   'Int' | 'Float' | 'Char' | 'String' | 'Bool' ;
ARRAY: 'Array';
LIST:  'List';
DICTIONARY: 'Dictionary';
ITERABLE: 'Iter';

//Operators And Punctuators
ASSIGN:               	  '=';
ARROW:					  '->'; 
OPEN_BRACE:               '{';
CLOSE_BRACE:              '}';
OPEN_SQ_BRACKET:          '[';
CLOSE_SQ_BRACKET:         ']';
OPEN_BRACKET:              '(';
CLOSE_BRACKET:             ')';
DOUBLE_DOT:               '..';
DOT:                      '.';
COMMA:                    ','; 
COLON:                    ':';
PLUS:                     '+';
MINUS:                    '-';
MULT:                     '*';
DIVIDE:                   '/';
POWER:                    '^';
MOD:                      'mod';
INT_DIV:				  'div';
LT:                       '<';
GT:                       '>';
OP_AND:                   'and';
OP_NOT:                   'not';
OP_OR:                    'or';
OP_XOR:                   'xor';
OP_EQ:                    'is';
OP_NE:                    'is' (Whitespace)* 'not';
OP_LE:                    '<=';
OP_GE:                    '>=';

TYPENAME:           IdentifierStartingUC;
IDENTIFIER:         IdentifierStartingLC;

LITERAL_INTEGER:     [0-9] [0-9]*;
LITERAL_FLOAT:        LITERAL_INTEGER DOT LITERAL_INTEGER ExponentPart?;

LITERAL_CHAR:                   '\'' (~['\\\r\n\u0085] | CommonCharacter) '\'';
LITERAL_STRING:                      '"'  (~["\u0085] | CommonCharacter)* '"';

WHITESPACES:   (Whitespace)+  -> skip;

fragment InputCharacter: ~[\r\n\u0085];

fragment NewLineCharacter
	: '\u000D' // Carriage Return
	| '\u000A' // Line Feed 
	| '\u0085' // Next Line 
	;

fragment ExponentPart:   [eE] (PLUS | MINUS)? LITERAL_INTEGER;

fragment CommonCharacter
	: SimpleEscapeSequence
	| HexEscapeSequence
	| UnicodeEscapeSequence
	;

fragment SimpleEscapeSequence
	: '\\\''
	| '\\"'
	| '\\\\'
	| '\\0'
	| '\\a'
	| '\\b'
	| '\\f'
	| '\\n'
	| '\\r'
	| '\\t'
	| '\\v'
	;

fragment HexEscapeSequence
	: '\\x' HexDigit
	| '\\x' HexDigit HexDigit
	| '\\x' HexDigit HexDigit HexDigit
	| '\\x' HexDigit HexDigit HexDigit HexDigit
	;

fragment NewLine: '\r\n' | '\r' | '\n'| '\u0085';

fragment Whitespace
	: UnicodeClassZS //'<Any Character With Unicode Class Zs>'
	| '\u0009' // Horizontal Tab 
	| '\u000B' // Vertical Tab
	| '\u000C' // Form Feed
	;

fragment UnicodeClassZS
	: '\u0020' // SPACE
	| '\u00A0' // NO_BREAK SPACE
	;

fragment IdentifierStartingUCorLC: (UnicodeClassLL|UnicodeClassLU) IdentifierPartCharacter*;

fragment IdentifierStartingLC: UnicodeClassLL IdentifierPartCharacter*;

fragment IdentifierStartingUC: UnicodeClassLU IdentifierPartCharacter*;

fragment IdentifierPartCharacter
	: UnicodeClassLU
	| UnicodeClassLL
	| DecimalDigitCharacter
	| '_'
	;

fragment LetterCharacter
	: UnicodeClassLU
	| UnicodeClassLL
	| UnicodeEscapeSequence
	;

fragment DecimalDigitCharacter
	: UnicodeClassND
	| UnicodeEscapeSequence
	;

fragment ConnectingCharacter:  UnicodeEscapeSequence;

fragment FormattingCharacter: UnicodeEscapeSequence;

fragment UnicodeEscapeSequence
	: '\\u' HexDigit HexDigit HexDigit HexDigit
	| '\\U' HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit
	;

fragment HexDigit : [0-9] | [A-F] | [a-f];

fragment UnicodeClassLU: '\u0041'..'\u005a';
fragment UnicodeClassLL	: '\u0061'..'\u007A';
fragment UnicodeClassND	: '\u0030'..'\u0039';
	
NEWLINE
  : '\r'? '\n'
  | '\r'
  ;

WS  :   [ \t]+ -> skip ;