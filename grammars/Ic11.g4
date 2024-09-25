grammar Ic11;

options {
	language = CSharp;
}

// Parser rules
program: (declaration | function)* EOF;

declaration: 'pin' IDENTIFIER PINID ';';

function: retType=('void' | 'real') IDENTIFIER '(' (IDENTIFIER (',' IDENTIFIER)*)? ')' block;

block: '{' statement* '}';

statement: delimitedStatement | undelimitedStatement;

delimitedStatement: (
        deviceWithIdAssignment
		| assignment
		| yieldStatement
        | returnValueStatement
		| returnStatement
		| continueStatement
		| variableDeclaration
        | functionCallStatement
	) ';';

yieldStatement: YIELD;
returnStatement: RETURN;
returnValueStatement: RETURN expression;
continueStatement: CONTINUE;
functionCallStatement: IDENTIFIER '(' (expression (',' expression)*)? ')';

undelimitedStatement: whileStatement | ifStatement;

whileStatement: WHILE '(' expression ')' (block | statement);

ifStatement: IF '(' expression ')' (block | statement) ( ELSE (block | statement))?;

deviceWithIdAssignment: DEVICE_WITH_ID '(' idExpr=expression ')' '.' IDENTIFIER '=' valueExpr=expression;
assignment: IDENTIFIER ('.' IDENTIFIER)* '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;

expression:
    op=(NEGATION | SUB) operand=expression # UnaryOp
    | left=expression op=(MUL | DIV | MOD) right=expression # BinaryOp
    | left=expression op=(ADD | SUB) right=expression # BinaryOp
    | left=expression op=(LT | GT | LE | GE | EQ | NE) right=expression # BinaryOp
    | left=expression op=AND right=expression # BinaryOp
    | left=expression op=OR right=expression # BinaryOp
    | '(' expression ')' # Parenthesis
    | type=(INTEGER | BOOLEAN | REAL) # Literal
    | IDENTIFIER '(' (expression (',' expression)*)? ')' # FunctionCall
    | IDENTIFIER # Identifier
    | IDENTIFIER '.' IDENTIFIER # MemberAccess
    | DEVICE_WITH_ID '(' expression ')' '.' IDENTIFIER # DeviceIdAccess
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
MOD: '%';
LT: '<';
GT: '>';
LE: '<=';
GE: '>=';
AND: '&&';
OR: '||';
EQ: '==';
NE: '!=';
NEGATION: '!';
DEVICE_WITH_ID: 'DeviceWithId';

BOOLEAN: 'true' | 'false';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;

REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;