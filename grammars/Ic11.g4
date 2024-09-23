grammar Ic11;

options {
	language = CSharp;
}

// Parser rules
program: (declaration | function)* EOF;

declaration: 'pin' IDENTIFIER PINID ';';

function: 'void' IDENTIFIER '(' ')' block;

block: '{' statement* '}';

statement: delimitedStatement | undelimitedStatement;

delimitedStatement: (
		assignment
		| yieldStatement
		| RETURN
		| variableDeclaration
	) ';';

yieldStatement: YIELD;

undelimitedStatement: whileStatement | ifStatement;

whileStatement: WHILE '(' expression ')' (block | statement);

ifStatement: IF '(' expression ')' (block | statement) ( ELSE (block | statement))?;

assignment: IDENTIFIER ('.' IDENTIFIER)* '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;

expression:
    op=unaryOperator operand=expression # Unary
    | operand1=expression op=(MUL | DIV) operand2=expression # MulDiv
    | operand1=expression op=(PLUS | MINUS) operand2=expression # AddSub
    | operand1=expression op=(LT | GT | LE | GE) operand2=expression # Relational
    | operand1=expression op=AND operand2=expression # And
    | operand1=expression op=OR operand2=expression # Or
    | '(' expression ')' # Parenthesis
    | literal # TheLiteral
    | IDENTIFIER # Identifier
    | IDENTIFIER '.' IDENTIFIER # MemberAccess
    | IDENTIFIER '.' IDENTIFIER '(' ')' # FunctionCall
    ;

unaryOperator: NEGATION | MINUS;

literal: INTEGER | BOOLEAN | REAL;

// Lexer rules
PINID: 'db' | 'd0' | 'd1' | 'd2' | 'd3' | 'd4' | 'd5';
WHILE: 'while';
IF: 'if';
ELSE: 'else';
YIELD: 'yield';
RETURN: 'return';
VAR: 'var';
PLUS: '+';
MINUS: '-';
MUL: '*';
DIV: '/';
LT: '<';
GT: '>';
LE: '<=';
GE: '>=';
AND: '&&';
OR: '||';
NEGATION: '!';

BOOLEAN: 'true' | 'false';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;

REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;