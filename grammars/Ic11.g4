grammar Ic11;

options {
    language = CSharp;
}

// Parser rules
program: (declaration | function)* EOF;

declaration: 'Pin' IDENTIFIER '=' IDENTIFIER ';';

function: 'void' IDENTIFIER '(' ')' block;

block: '{' statement* '}';

statement
    : whileStatement
    | ifStatement
    | assignment
    | yieldStatement
    | returnStatement
    | variableDeclaration
    ;

whileStatement: WHILE '(' ')' block;

ifStatement: IF '(' expression ')' block ( ELSE block )?;

assignment: IDENTIFIER '.' IDENTIFIER '=' expression ';';

yieldStatement: YIELD ';';

returnStatement: RETURN ';';

variableDeclaration: VAR IDENTIFIER '=' expression ';';

negation: '!' expression;

expression
    : primaryExpression (binaryOperator primaryExpression)*
    ;

primaryExpression
    : IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER '(' ')'
    | negation
    | '(' expression ')'
    | literal
    ;

binaryOperator
    : PLUS
    | MINUS
    | MUL
    | DIV
    | LT
    | GT
    | LE
    | GE
    | AND
    ;

literal
    : INTEGER
    | BOOLEAN
    ;

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

IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;
BOOLEAN: 'true' | 'false';

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;