grammar Elan;
import Elan_Lexer;

file: (main | procedureDef | functionDef | constantDef | enumDef | classDef | test | systemAccessor)* NL* EOF;

main: 
	NL MAIN 
    statementBlock
    NL END MAIN 
    ;

test: 
	NL TEST IDENTIFIER
    statementBlock
    NL END TEST 
    ;

// STATEMENTS
statementBlock:  (varDef | assignment | proceduralControlFlow | callStatement | throwException | printStatement)*;

callStatement: NL CALL (procedureCall | (assignableValue DOT procedureCall));

throwException: NL THROW (LITERAL_STRING | IDENTIFIER );

printStatement: NL PRINT expression?;
 
varDef: NL VAR assignableValue ASSIGN (expression | systemCall);

assignment: NL SET assignableValue TO (expression | systemCall);

inlineAsignment: assignableValue ASSIGN expression;

assignableValue: (scopeQualifier?  IDENTIFIER index?) | deconstructedTuple | listDecomp;

procedureCall: scopeQualifier? IDENTIFIER OPEN_BRACKET (argumentList)? CLOSE_BRACKET;

functionCall: scopeQualifier? IDENTIFIER OPEN_BRACKET (argumentList)? CLOSE_BRACKET;

systemCall: SYSTEM DOT IDENTIFIER OPEN_BRACKET (argumentList)? CLOSE_BRACKET;

argumentList: (expression |lambda) (COMMA (expression | lambda))*;

// PROCEDURES
procedureDef:
	NL PROCEDURE procedureSignature
	statementBlock 
    NL END PROCEDURE
	;

procedureSignature: IDENTIFIER OPEN_BRACKET procedureParameterList? CLOSE_BRACKET;

procedureParameterList: procedureParameter (COMMA procedureParameter)*;

parameterList: parameter (COMMA parameter)*;

parameter: IDENTIFIER type;

procedureParameter: OUT? IDENTIFIER type;

// FUNCTIONS
functionDef: functionWithBody | expressionFunction;

functionWithBody: 
	NL FUNCTION functionSignature
	statementBlock
	NL RETURN expression
    NL END FUNCTION
	;

expressionFunction: 
	NL FUNCTION functionSignature ARROW expression; 
   
functionSignature: IDENTIFIER OPEN_BRACKET parameterList? CLOSE_BRACKET AS type;

// System Accessor
systemAccessor: 
	NL SYSTEM accessorSignature
	statementBlock
	NL RETURN expression
    NL END SYSTEM
	;

accessorSignature: IDENTIFIER OPEN_BRACKET parameterList? CLOSE_BRACKET AS type;
// CONSTANTS
constantDef: NL CONSTANT IDENTIFIER ASSIGN (literal | newInstance);

// ENUMERATIONS
enumDef: 
	NL ENUM enumType
	  NL IDENTIFIER (COMMA IDENTIFIER)*  
	NL END ENUM
	;

enumType: TYPENAME;
enumValue:	enumType DOT IDENTIFIER;

// CLASSES
classDef: mutableClass | abstractClass| immutableClass | abstractImmutableClass;

mutableClass: 
	NL CLASS TYPENAME inherits?
	constructor
    (property | functionDef | procedureDef )*	
    NL END CLASS
	;

abstractClass:
	NL ABSTRACT CLASS TYPENAME inherits?
    (property | NL FUNCTION functionSignature | NL PROCEDURE procedureSignature)*
    NL END CLASS
	;

immutableClass: 
	NL IMMUTABLE CLASS TYPENAME inherits?
	constructor
    (property | functionDef )*
    NL END CLASS 
	;

abstractImmutableClass:
	NL ABSTRACT IMMUTABLE CLASS TYPENAME inherits?
    (property | NL FUNCTION functionSignature)*
    NL END CLASS
	;
 
inherits: INHERITS type (COMMA type)*;

property: NL PRIVATE? PROPERTY IDENTIFIER type; 

constructor: 
	NL CONSTRUCTOR OPEN_BRACKET parameterList? CLOSE_BRACKET
    statementBlock
	NL END CONSTRUCTOR
	;

// INSTANTIATION
newInstance:
	NEW type OPEN_BRACKET argumentList? CLOSE_BRACKET withClause?
	| IDENTIFIER withClause
	;

withClause: WITH OPEN_BRACE inlineAsignment (COMMA inlineAsignment)* CLOSE_BRACE;

// CONTROL FLOW STATEMENTS
proceduralControlFlow: if | for | foreach | while | repeat | try | switch;

if:
	NL IF expression THEN
    statementBlock
    (NL ELSE IF expression THEN
    statementBlock)*
    (NL ELSE
    statementBlock)?
    NL END IF
	;

for: 
	NL FOR IDENTIFIER ASSIGN expression TO expression (STEP MINUS? LITERAL_INTEGER)?
	statementBlock
	NL END FOR
	;

foreach: 
	NL FOREACH IDENTIFIER IN expression 
    statementBlock
    NL END FOREACH
	;
          
while: 
	NL WHILE expression 
    statementBlock
    NL END WHILE
	;
          
repeat: 
	NL (REPEAT)
    statementBlock
    NL UNTIL expression
	;

try: 
	NL TRY 
    statementBlock
    NL CATCH IDENTIFIER 
	statementBlock
    NL END TRY
	;

switch: 
	NL SWITCH expression
	  case+
      caseDefault
	NL END SWITCH
	;
	
case: 
	NL CASE MINUS? literalValue
    statementBlock
	;

caseDefault : 
	NL DEFAULT
    statementBlock
	;

// EXPRESSIONS
expression: 
	  bracketedExpression 
	| functionCall 
	| value
	| expression index
	| expression DOT functionCall
	| expression DOT IDENTIFIER 
	| unaryOp expression
	| expression POWER expression // so that ^ has higher priority (because implemented with function in CSharp)
	| expression binaryOp expression
	| newInstance
	| ifExpression
	| expression withClause
	| NL expression // so that any expression may be broken over multiple lines at its 'natural joints' i.e. before any sub-expression
	;

bracketedExpression: OPEN_BRACKET expression CLOSE_BRACKET ; //made into rule so that compiler can add the brackets explicitly

ifExpression: IF expression NL THEN expression NL ELSE expression;

lambda: LAMBDA argumentList ARROW expression;

index: OPEN_SQ_BRACKET (expression | expression COMMA expression | range) CLOSE_SQ_BRACKET;

range: expression DOUBLE_DOT expression | expression DOUBLE_DOT	| DOUBLE_DOT expression; 

// VALUES
value: literal | scopeQualifier? IDENTIFIER  |dataStructureDefinition | SELF | DEFAULT type;

scopeQualifier: (SELF | GLOBAL ) DOT; // might add 'namespace' as a further option in future
 
// LITERALS
literal: literalValue | literalDataStructure ; 

literalValue:  BOOL_VALUE | LITERAL_INTEGER | LITERAL_FLOAT | LITERAL_CHAR | enumValue ;

dataStructureDefinition:  listDefinition | arrayDefinition | tupleDefinition | dictionaryDefinition  ;
 
literalDataStructure: LITERAL_STRING | literalTuple | literalList | literalDictionary;

tupleDefinition:  OPEN_BRACKET expression COMMA expression (COMMA expression)* CLOSE_BRACKET; 

literalTuple:  OPEN_BRACKET literal COMMA literal (COMMA literal)* CLOSE_BRACKET; 

deconstructedTuple: OPEN_BRACKET IDENTIFIER (COMMA IDENTIFIER)+  CLOSE_BRACKET;
 
listDefinition: OPEN_BRACE (expression (COMMA expression)*) CLOSE_BRACE;

literalList: OPEN_BRACE (literal (COMMA literal)* ) CLOSE_BRACE;

listDecomp: OPEN_BRACE IDENTIFIER COLON IDENTIFIER CLOSE_BRACE;

arrayDefinition: ARRAY genericSpecifier OPEN_BRACKET LITERAL_INTEGER? CLOSE_BRACKET;

dictionaryDefinition: OPEN_BRACE (kvp (COMMA kvp)* ) CLOSE_BRACE;

literalDictionary: OPEN_BRACE (literalKvp (COMMA literalKvp)*) CLOSE_BRACE;

kvp: expression COLON expression;

literalKvp: literal COLON literal;

// OPERATIONS
unaryOp: MINUS | OP_NOT;

binaryOp: arithmeticOp | logicalOp | conditionalOp ;

arithmeticOp:  POWER | MULT | DIVIDE | MOD | INT_DIV | PLUS | MINUS;

logicalOp: OP_AND | OP_OR | OP_XOR;

conditionalOp: GT | LT | OP_GE | OP_LE | OP_EQ | OP_NE;

// TYPES
type:  VALUE_TYPE | dataStructureType | TYPENAME | TYPENAME genericSpecifier | tupleType |  funcType;

dataStructureType: (ARRAY | LIST | DICTIONARY | ITERABLE ) genericSpecifier;

genericSpecifier: LT OF type (COMMA type)* GT;

tupleType: OPEN_BRACKET type (COMMA type)+ CLOSE_BRACKET; 
    
funcType: OPEN_BRACKET type (COMMA type)*  ARROW type CLOSE_BRACKET; 

