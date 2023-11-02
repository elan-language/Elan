lexer grammar Elan_Lexer;

BYTE_ORDER_MARK: '\u00EF\u00BB\u00BF';

NL: [\r\n\f]+ ;

SINGLE_LINE_COMMENT: NL? COMMENT_MARKER  InputCharacter*    -> skip; 

COMMENT_MARKER: '#';

// Keywords
ABSTRACT:      'abstract';
CASE: 		   'case';
CATCH:         'catch';
CLASS:         'class';
CONSTANT:      'constant';
CONSTRUCTOR:   'constructor';
CURRY:   	   'curry';
DEFAULT: 	   'default'; //for use with switch...case and as default for a type
ELSE:          'else';
END:		   'end'; //used with another keyword to delimit a statement block
ENUMERATION:   'enumeration';
FOR:           'for';
FOREACH:       'foreach';
FUNCTION:	   'function';
GLOBAL:			'global';
IF:            'if'; 
IMMUTABLE:	   'immutable';
IN:            'in'; // foreach...in
INHERITS:      'inherits';
LAMBDA:		   'lambda';
LET:           'let';
MAIN:		   'main';
PARTIAL: 	   'partial'; // partial function application
PRIVATE:       'private';
PROCEDURE:	   'procedure';
PROPERTY:      'property';
REPEAT:		   'repeat';
RETURN:        'return';
SELF:		   'self';
STEP:		   'step';
SWITCH:        'switch';
THEN:		   'then';
TO:			   'to';
TRY:           'try';
UNTIL:         'until';
VAR:		   'var';
WHILE:         'while';
WITH: 		   'with';

BOOL_VALUE: 'true' | 'false';

// Types
VALUE_TYPE:  'Value' | 'Int' | 'Float' | 'Decimal' | 'Number' | 'Char' | 'String' | 'Bool' ;
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
DOUBLE_DOT:               '..'; // used for ranges
DOT:                      '.';
COMMA:                    ',' [ \t]* NL?;  //Any comma-separated list (of data, params etc) should be allowed to be split over more than one line.
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
OP_EQ:                    '==' | 'is';
OP_NE:                    '<>' | ('is' [ \t]* 'not');
OP_LE:                    '<=';
OP_GE:                    '>=';

//Identifiers
// must be defined after all keywords
// https://msdn.microsoft.com/en-us/library/aa664670(v=vs.71).aspx

TYPENAME:           IdentifierStartingUC;
IDENTIFIER:         IdentifierStartingLC;

//B.1.8 Literals
LITERAL_INTEGER:     [0-9] ('_'* [0-9])*;
LITERAL_FLOAT:        ([0-9] ('_'* [0-9])*)? '.' [0-9] ('_'* [0-9])* ExponentPart? [FfDdMm]? | [0-9] ('_'* [0-9])* ([FfDdMm] | ExponentPart [FfDdMm]?);
LITERAL_DECIMAL:	 LITERAL_FLOAT 'D';

LITERAL_CHAR:                   '\'' (~['\\\r\n\u0085\u2028\u2029] | CommonCharacter) '\'';
LITERAL_STRING:                      '"'  (~["\u0085\u2028\u2029] | CommonCharacter)* '"'; //Elan regular string is interpolated and verbatim
VERBATIM_ONLY_STRING:                    '"""' (~'"' | '""')* '"';

//Must be defined after other uses of it

WHITESPACES:   (Whitespace)+  -> skip;


// https://msdn.microsoft.com/en-us/library/dn961160.aspx
//mode INTERPOLATION_STRING;

/* DOUBLE_CURLY_INSIDE:           '{{';
OPEN_BRACE_INSIDE:             '{' { this.OpenBraceInside(); } -> skip, pushMode(DEFAULT_MODE);
REGULAR_CHAR_INSIDE:           { this.IsRegularCharInside() }? SimpleEscapeSequence;
VERBATIM_DOUBLE_QUOTE_INSIDE: { this.IsVerbatimDoubleQuoteInside() }? '""';
DOUBLE_QUOTE_INSIDE:           '"' { this.OnDoubleQuoteInside(); } -> popMode;
REGULAR_STRING_INSIDE:         { this.IsRegularCharInside() }? ~('{' | '\\' | '"')+;
VERBATIM_INSIDE_STRING:       { this.IsVerbatimDoubleQuoteInside() }? ~('{' | '"')+;

//mode INTERPOLATION_FORMAT;

DOUBLE_CURLY_CLOSE_INSIDE:      '}}' -> type(FORMAT_STRING);
CLOSE_BRACE_INSIDE:             '}' { this.OnCloseBraceInside(); }   -> skip, popMode;
FORMAT_STRING:                  ~'}'+;
 */

// Fragments

fragment InputCharacter:       ~[\r\n\u0085\u2028\u2029];

fragment NewLineCharacter
	: '\u000D' //'<Carriage Return CHARACTER (U+000D)>'
	| '\u000A' //'<Line Feed CHARACTER (U+000A)>'
	| '\u0085' //'<Next Line CHARACTER (U+0085)>'
	| '\u2028' //'<Line Separator CHARACTER (U+2028)>'
	| '\u2029' //'<Paragraph Separator CHARACTER (U+2029)>'
	;

fragment ExponentPart:              [eE] ('+' | '-')? [0-9] ('_'* [0-9])*;

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

fragment NewLine
	: '\r\n' | '\r' | '\n'
	| '\u0085' // <Next Line CHARACTER (U+0085)>'
	;

fragment Whitespace
	: UnicodeClassZS //'<Any Character With Unicode Class Zs>'
	| '\u0009' //'<Horizontal Tab Character (U+0009)>'
	| '\u000B' //'<Vertical Tab Character (U+000B)>'
	| '\u000C' //'<Form Feed Character (U+000C)>'
	;

fragment UnicodeClassZS
	: '\u0020' // SPACE
	| '\u00A0' // NO_BREAK SPACE
	;

fragment IdentifierStartingUCorLC
	: (UnicodeClassLL|UnicodeClassLU) IdentifierPartCharacter*
	;

fragment IdentifierStartingLC
	: UnicodeClassLL IdentifierPartCharacter*
	;

fragment IdentifierStartingUC
	: UnicodeClassLU IdentifierPartCharacter*
	;

fragment IdentifierPartCharacter
	: UnicodeClassLU
	| UnicodeClassLL
	| DecimalDigitCharacter
	| '_'
	;

//'<A Unicode Character Of Classes Lu, Ll, Lt, Lm, Lo, Or Nl>'
// WARNING: ignores UnicodeEscapeSequence
fragment LetterCharacter
	: UnicodeClassLU
	| UnicodeClassLL
	| UnicodeEscapeSequence
	;

//'<A Unicode Character Of The Class Nd>'
// WARNING: ignores UnicodeEscapeSequence
fragment DecimalDigitCharacter
	: UnicodeClassND
	| UnicodeEscapeSequence
	;

//'<A Unicode Character Of The Class Pc>'
// WARNING: ignores UnicodeEscapeSequence
fragment ConnectingCharacter:  UnicodeEscapeSequence;


//'<A Unicode Character Of The Class Cf>'
// WARNING: ignores UnicodeEscapeSequence
fragment FormattingCharacter: UnicodeEscapeSequence;

//B.1.5 Unicode Character Escape Sequences
fragment UnicodeEscapeSequence
	: '\\u' HexDigit HexDigit HexDigit HexDigit
	| '\\U' HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit
	;

fragment HexDigit : [0-9] | [A-F] | [a-f];

// Unicode character classes
fragment UnicodeClassLU: '\u0041'..'\u005a';
fragment UnicodeClassLL	: '\u0061'..'\u007A';
fragment UnicodeClassND	: '\u0030'..'\u0039';
	
NEWLINE
  : '\r'? '\n'
  | '\r'
  ;


WS  :   [ \t]+ -> skip ;