grammar Elan;
import Elan_Lexer;

file: (main | constantDef | enumDef | classDef | functionDef | procedureDef)* NL* EOF;

main: 
	NL MAIN 
    statementBlock
    NL END MAIN 
    ;

constantDef: NL CONSTANT IDENTIFIER ASSIGN expression;

enumDef: 
	NL ENUMERATION TYPENAME
	(enumValue (COMMA enumValue)*)  
	NL END ENUMERATION
	;

enumValue: IDENTIFIER (ASSIGN LITERAL_INTEGER);

classDef: abstractClass | mutableClass | immutableClass;

mutableClass: 
	NL CLASS TYPENAME inherits?
    (constructor |property | functionDef | procedureDef | constantDef)*	
    NL END CLASS
	;

immutableClass: 
	NL IMMUTABLE CLASS TYPENAME inherits?
    (constructor |property | functionDef | constantDef)*
    NL END CLASS 
	;

abstractClass:
	NL ABSTRACT CLASS TYPENAME inherits?
    (property | NL FUNCTION functionSignature | NL PROCEDURE procedureSignature)*
    NL END CLASS
	;
 
inherits: INHERITS type (COMMA type)*;

constructor: 
	NL CONSTRUCTOR (OPEN_BRACKET NL? parameterList? NL? CLOSE_BRACKET)? 
    statementBlock
	NL END CONSTRUCTOR
	;

property: NL PRIVATE? PROPERTY IDENTIFIER (type | (ASSIGN expression)); 

functionDef: functionWithBody | expressionFunction;

functionWithBody: 
	NL FUNCTION functionSignature
	statementBlock
	NL RETURN expression
    NL END FUNCTION
	;

expressionFunction: 
	NL FUNCTION functionSignature NL? ARROW NL? letIn? expression; 

letIn: LET NL? assignableValue ASSIGN expression (COMMA assignableValue ASSIGN expression)* NL? IN NL?; 
   
functionSignature: IDENTIFIER OPEN_BRACKET NL? parameterList? NL? CLOSE_BRACKET NL? AS NL? type;

procedureDef:
	NL PROCEDURE procedureSignature
	statementBlock 
    NL END PROCEDURE
	;

procedureSignature: IDENTIFIER OPEN_BRACKET NL? parameterList? CLOSE_BRACKET;

statementBlock:  (constantDef | varDef | assignment | proceduralControlFlow | callStatement | freestandingException)*;

callStatement: NL expression; //Intended for a freestanding procedure/system call as a statement, 
// or expression terminated by a procedure or system call that consumes result'.
// Not possible to specify this as a syntax distinct from an expression. Compile rules will enforce that you can't use a non-consumed expression

freestandingException: NL throwException;

varDef: NL VAR assignableValue ASSIGN expression;

assignment: NL assignableValue (ASSIGN | assignmentOp)  expression;

assignableValue: ((SELF DOT)?  IDENTIFIER index?) | RESULT | tupleDecomp | listDecomp;

methodCall: (CURRY|PARTIAL)? IDENTIFIER genericSpecifier? OPEN_BRACKET (argumentList)? CLOSE_BRACKET;

argumentList: expression (COMMA expression)*;

parameterList: parameter  (COMMA parameter)*;

parameter: NL? IDENTIFIER type; 

proceduralControlFlow: if | for | forIn | while | repeat | try | switch;

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
	NL FOR IDENTIFIER ASSIGN expression TO expression 
	statementBlock
	NL END FOR
	;

forIn: 
	NL FOR IDENTIFIER IN expression 
    statementBlock
    NL END FOR
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
    (NL CATCH IDENTIFIER type 
	statementBlock)?
    NL END TRY
	;

switch: 
	NL SWITCH expression
	  case*
      caseDefault?
	END SWITCH
	;
	
case: 
	NL CASE literalValue
    statementBlock
	;

caseDefault : 
	NL DEFAULT
    statementBlock
	;

expression: 
	  bracketedExpression
	| methodCall
	| value
	| expression index
	| expression DOT methodCall
	| expression DOT IDENTIFIER 
	| unaryOp expression
	| expression binaryOp expression
	| newInstance
	| ifExpression
	| lambda
	| throwException
	| NL expression // so that any expression may be broken over multiple lines at its 'natural joints' i.e. before any sub-expression
	;

bracketedExpression: OPEN_BRACKET expression CLOSE_BRACKET ; //made into rule so that compiler can add the brackets explicitly

ifExpression: NL? IF expression NL? THEN expression NL? ELSE expression;

lambda: LAMBDA argumentList ARROW expression;

throwException: THROW type (OPEN_BRACKET argumentList CLOSE_BRACKET);

index: OPEN_SQ_BRACKET (expression | expression COMMA expression | range) CLOSE_SQ_BRACKET;

range: expression DOUBLE_DOT expression | expression DOUBLE_DOT	| DOUBLE_DOT expression; 

value: literalValue | ((SELF DOT)? IDENTIFIER) | literalDataStructure | SELF | RESULT;

literalDataStructure: tuple | literalList | literalDictionary;

tuple:  OPEN_BRACKET expression COMMA expression (COMMA expression)* CLOSE_BRACKET; 

tupleDecomp: OPEN_BRACKET IDENTIFIER (COMMA IDENTIFIER)+  CLOSE_BRACKET;
 
literalList: OPEN_BRACE (NL? expression (COMMA expression)* NL?)? CLOSE_BRACE;

listDecomp: OPEN_BRACE IDENTIFIER COLON IDENTIFIER CLOSE_BRACE;

literalDictionary: OPEN_BRACE (NL? kvp (COMMA kvp)* NL?)? CLOSE_BRACE;

kvp: expression COLON expression;

assignmentOp: ASSIGN_ADD | ASSIGN_SUBTRACT | ASSIGN_MULT | ASSIGN_DIV; 

unaryOp: MINUS | OP_NOT;

binaryOp: arithmeticOp | logicalOp | conditionalOp ;

arithmeticOp:  POWER | MULT | DIVIDE | MOD | INT_DIV | PLUS | MINUS;

logicalOp: OP_AND | OP_OR | OP_XOR;

conditionalOp: GT | LT | OP_GE | OP_LE | OP_EQ | OP_NE;

literalValue:  BOOL_VALUE | LITERAL_INTEGER | LITERAL_FLOAT | LITERAL_DECIMAL| LITERAL_CHAR | LITERAL_STRING;

newInstance:
	NEW type OPEN_BRACKET (argumentList)? CLOSE_BRACKET (withClause)?
	| IDENTIFIER withClause
	;

withClause: WITH OPEN_BRACE assignment (COMMA assignment)* CLOSE_BRACE;

type:  VALUE_TYPE | dataStructureType | TYPENAME | TYPENAME genericSpecifier | tupleType |  funcType;

dataStructureType: (ARRAY | LIST | DICTIONARY | ITERABLE ) genericSpecifier;

genericSpecifier: LT type (COMMA type)* GT;

tupleType: OPEN_BRACKET type (COMMA type)+ CLOSE_BRACKET; 
    
funcType: OPEN_BRACKET type (COMMA type)*  ARROW type CLOSE_BRACKET; 