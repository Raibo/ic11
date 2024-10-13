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
        | deviceWithIndexExtendedAssignment
        | deviceWithIndexAssignment
        | memberExtendedAssignment
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

memberExtendedAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' prop=(SLOTS | REAGENTS | STACK) '[' slotIdxExpr=expression ']' ('.' member=IDENTIFIER)? '=' valueExpr=expression;
memberAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER '=' valueExpr=expression;

deviceWithIndexExtendedAssignment: PINS '[' pinIdxExpr=expression ']' '.' prop=(SLOTS | REAGENTS | STACK) '[' slotIdxExpr=expression ']' ('.' member=IDENTIFIER)? '=' valueExpr=expression;
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
    op=DIRECT_UNARY_OPERATOR '(' operand=expression ')' # UnaryOp
    | op=DIRECT_BINARY_OPERATOR '(' left=expression ',' right=expression ')' # BinaryOp
    | op=(NEGATION | SUB | BITWISE_NOT) operand=expression # UnaryOp
    | left=expression op=(SHIFTL | SHIFTR) right=expression # BinaryOp
    | left=expression op=(MUL | DIV | MOD) right=expression # BinaryOp
    | left=expression op=(ADD | SUB) right=expression # BinaryOp
    | left=expression op=(LT | GT | LE | GE | EQ | NE) right=expression # BinaryOp
    | left=expression op=AND right=expression # BinaryOp
    | left=expression op=(OR | XOR) right=expression # BinaryOp
    | '(' expression ')' # Parenthesis
    | type=(INTEGER | BOOLEAN | REAL) # Literal
    | IDENTIFIER '(' (expression (',' expression)*)? ')' # FunctionCall
    | IDENTIFIER # Identifier
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER # MemberAccess
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' prop=(SLOTS | REAGENTS | STACK) '[' slotIdxExpr=expression ']' ('.' member=IDENTIFIER)? # ExtendedMemberAccess
    | PINS '[' pinIdxExpr=expression ']' '.' member=IDENTIFIER # DeviceIndexAccess
    | PINS '[' pinIdxExpr=expression ']' '.' prop=(SLOTS | REAGENTS | STACK) '[' slotIdxExpr=expression ']' ('.' member=IDENTIFIER)? # ExtendedDeviceIndexAccess
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
BITWISE_NOT: '~';
SHIFTL: '<<';
SHIFTR: '>>';
LT: '<';
GT: '>';
LE: '<=';
GE: '>=';
AND: '&';
OR: '|';
XOR: '^';
EQ: '==';
NE: '!=';
NEGATION: '!';
PINS: 'Pins';
SLOTS: 'Slots';
REAGENTS: 'Reagents';
STACK: 'Stack';
DEVICE_WITH_ID: 'DeviceWithId';

DIRECT_UNARY_OPERATOR:
    'not'
    | 'round'
    | 'ceil'
    | 'floor'
    | 'trunc'
    | 'abs'
    | 'sqrt'
    | 'exp'
    | 'log'
    | 'sin'
    | 'asin'
    | 'cos'
    | 'acos'
    | 'tan'
    | 'atan'
    | 'seqz'
    | 'snez'
    | 'sapz'
    | 'snaz'
    | 'sgez'
    | 'sgtz'
    | 'slez'
    | 'sltz'
    | 'snan'
    | 'snanz';

DIRECT_BINARY_OPERATOR:
    'add'
    | 'sub'
    | 'mul'
    | 'div'
    | 'mod'
    | 'max'
    | 'min'
    | 'atan2'
    | 'and'
    | 'or'
    | 'xor'
    | 'sll'
    | 'srl'
    | 'sla'
    | 'sra'
    | 'nor'
    | 'seq'
    | 'sne'
    | 'sap'
    | 'sna'
    | 'sgt'
    | 'sge'
    | 'slt'
    | 'sle';

BOOLEAN: 'true' | 'false';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;

REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;
MULTILINE_COMMENT : '/*' ( MULTILINE_COMMENT | . )*? '*/'  -> skip;