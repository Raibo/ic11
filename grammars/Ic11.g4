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
		| returnStatement
		| continueStatement
		| variableDeclaration
	) ';';

yieldStatement: YIELD;
returnStatement: RETURN;
continueStatement: CONTINUE;

undelimitedStatement: whileStatement | ifStatement;

whileStatement: WHILE '(' expression ')' (block | statement);

ifStatement: IF '(' expression ')' (block | statement) ( ELSE (block | statement))?;

assignment: IDENTIFIER ('.' IDENTIFIER)* '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;

expression:
    op=(NEGATION | SUB) operand=expression # UnaryOp
    | left=expression op=(MUL | DIV) right=expression # BinaryOp
    | left=expression op=(ADD | SUB) right=expression # BinaryOp
    | left=expression op=(LT | GT | LE | GE) right=expression # BinaryOp
    | left=expression op=AND right=expression # BinaryOp
    | left=expression op=OR right=expression # BinaryOp
    | '(' expression ')' # Parenthesis
    | type=(INTEGER | BOOLEAN | REAL) # Literal
    | IDENTIFIER # Identifier
    | IDENTIFIER '.' IDENTIFIER # MemberAccess
    | IDENTIFIER '.' IDENTIFIER '(' ')' # FunctionCall
    ;

// Lexer rules
PINID: 'db' | 'd0' | 'd1' | 'd2' | 'd3' | 'd4' | 'd5';
WHILE: 'while';
IF: 'if';
ELSE: 'else';
YIELD: 'yield';
RETURN: 'return';
CONTINUE: 'continue';
VAR: 'var';
ADD: '+';
SUB: '-';
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