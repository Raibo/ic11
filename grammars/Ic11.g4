grammar Ic11;

options {
	language = CSharp;
}

// Parser rules
program: (declaration | function)* EOF;

declaration: 'Pin' IDENTIFIER '=' IDENTIFIER ';';

function: 'void' IDENTIFIER '(' ')' block;

block: ('{' statement* '}') | statement;

statement: delimitedStatement | undelimitedStatement;

delimitedStatement: (
		assignment
		| YIELD
		| RETURN
		| variableDeclaration
	) ';';

undelimitedStatement: whileStatement | ifStatement;

whileStatement: WHILE '(' ')' block;

ifStatement: IF '(' expression ')' block ( ELSE block)?;

assignment: IDENTIFIER ('.' IDENTIFIER)* '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;

expression:
    unaryOperator expression
    | expression op=(MUL | DIV) expression
    | expression op=(PLUS | MINUS) expression 
    | expression op=(LT | GT | LE | GE) expression 
    | expression op=AND expression
    | expression op=OR expression
    | '(' expression ')'
    | literal
    | IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER '(' ')'
    ;

primaryExpression:
    ;

unaryOperator: NEGATION | MINUS;

literal: INTEGER | BOOLEAN | REAL;

// Lexer rules
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

IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;
BOOLEAN: 'true' | 'false';
REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;