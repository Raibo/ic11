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

whileStatement: 'while' '(' ')' block;

ifStatement: 'if' '(' expression ')' block ( 'else' block )?;

assignment: IDENTIFIER '.' IDENTIFIER '=' expression ';';

yieldStatement: 'yield' ';';

returnStatement: 'return' ';';

variableDeclaration: 'var' IDENTIFIER '=' expression ';';

negation: '!' expression;

expression
    : IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER
    | IDENTIFIER '.' IDENTIFIER '(' ')'
    | negation
    | expression ('+' | '-' | '*' | '/' | '<' | '>' | '<=' | '>=' | '&&') expression
    | '(' expression ')'
    | literal
    ;

literal
    : INTEGER
    | BOOLEAN
    ;

// Lexer rules
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;
BOOLEAN: 'true' | 'false';

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;