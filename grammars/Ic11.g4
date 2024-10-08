grammar Ic11;

options {
	language = CSharp;
}

// Parser rules
program: (declaration | ( constantDeclaration ';') | function)* EOF;

declaration: 'pin' IDENTIFIER PINID ';';

function: retType=('void' | 'real') IDENTIFIER '(' (IDENTIFIER (',' IDENTIFIER)*)? ')' block;

block: '{' statement* '}';

statement: delimitedStatement | undelimitedStatement;

delimitedStatement: (
        deviceWithIdAssignment
        | deviceWithIndexSlotAssignment
        | deviceWithIndexAssignment
        | memberSlotAssignment
        | memberAssignment
        | assignment
        | yieldStatement
        | returnValueStatement
        | returnStatement
        | continueStatement
        | breakStatement
        | variableDeclaration
        | constantDeclaration
        | functionCallStatement
        | arrayDeclaration
        | arrayAssignment
    ) ';';

yieldStatement: YIELD;
returnStatement: RETURN;
returnValueStatement: RETURN expression;
continueStatement: CONTINUE;
breakStatement: BREAK;
functionCallStatement: IDENTIFIER '(' (expression (',' expression)*)? ')';

undelimitedStatement: whileStatement | ifStatement;

whileStatement: WHILE '(' expression ')' (block | statement);

ifStatement: IF '(' expression ')' (block | statement) ( ELSE (block | statement))?;

deviceWithIdAssignment: DEVICE_WITH_ID '(' idExpr=expression ')' '.' IDENTIFIER '=' valueExpr=expression;

memberSlotAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' SLOTS '[' slotIdxExpr=expression ']' '.' member=IDENTIFIER '=' valueExpr=expression;
memberAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER '=' valueExpr=expression;

deviceWithIndexSlotAssignment: PINS '[' pinIdxExpr=expression ']' '.' SLOTS '[' slotIdxExpr=expression ']' '.' member=IDENTIFIER '=' valueExpr=expression;
deviceWithIndexAssignment: PINS '[' pinIdxExpr=expression ']' '.' member=IDENTIFIER '=' valueExpr=expression;

assignment: IDENTIFIER '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;
constantDeclaration: CONST IDENTIFIER '=' expression;

arrayDeclaration:
    VAR IDENTIFIER '=' '[' sizeExpr=expression ']' # arraySizeDeclaration
    | VAR IDENTIFIER '=' '{' (expression (',' expression)*)? ','? '}' # arrayListDeclaration
    ;

arrayAssignment: IDENTIFIER '[' indexExpr=expression ']' '=' valueExpr=expression;

expression:
    op=(NEGATION | SUB) operand=expression # UnaryOp
    | op=ABS '(' operand=expression ')' # UnaryOp
    | left=expression op=(MUL | DIV | MOD) right=expression # BinaryOp
    | left=expression op=(ADD | SUB) right=expression # BinaryOp
    | left=expression op=(LT | GT | LE | GE | EQ | NE) right=expression # BinaryOp
    | left=expression op=AND right=expression # BinaryOp
    | left=expression op=OR right=expression # BinaryOp
    | '(' expression ')' # Parenthesis
    | type=(INTEGER | BOOLEAN | REAL) # Literal
    | IDENTIFIER '(' (expression (',' expression)*)? ')' # FunctionCall
    | IDENTIFIER # Identifier
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER # MemberAccess
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' SLOTS '[' slotIdxExpr=expression ']' '.' member=IDENTIFIER # SlotMemberAccess
    | PINS '[' pinIdxExpr=expression ']' '.' member=IDENTIFIER # DeviceIndexAccess
    | PINS '[' pinIdxExpr=expression ']' '.' SLOTS '[' slotIdxExpr=expression ']' '.' member=IDENTIFIER # SlotDeviceIndexAccess
    | DEVICE_WITH_ID '(' expression ')' '.' IDENTIFIER # DeviceIdAccess
    | IDENTIFIER '[' indexExpr=expression ']' # ArrayElementAccess
    ;

// Lexer rules
PINID: 'db' | 'd0' | 'd1' | 'd2' | 'd3' | 'd4' | 'd5';
WHILE: 'while';
IF: 'if';
ELSE: 'else';
YIELD: 'yield';
RETURN: 'return';
CONTINUE: 'continue';
BREAK: 'break';
BASE_DEVICE: 'Base';
VAR: 'var';
CONST: 'const';
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
ABS: 'Abs';
PINS: 'Pins';
SLOTS: 'Slots';
DEVICE_WITH_ID: 'DeviceWithId';

BOOLEAN: 'true' | 'false';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;

REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;
MULTILINE_COMMENT : '/*' ( MULTILINE_COMMENT | . )*? '*/'  -> skip;