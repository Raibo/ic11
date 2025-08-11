grammar Ic11;

options {
	language = CSharp;
}

// Parser rules
program: (declaration | ( constantDeclaration ';') | function)* EOF;

declaration: 'pin' IDENTIFIER PINID ';';

function: retType=('void' | 'real') IDENTIFIER '(' (IDENTIFIER (',' IDENTIFIER)*)? ')' block;

block: '{' statement* '}';

statement: delimetedStatmentWithDelimiter | undelimitedStatement;

delimetedStatmentWithDelimiter: delimitedStatement ';';

delimitedStatement: (
    deviceStackClear
    | deviceWithIdStackClear
    | deviceWithIdExtendedAssignment
    | deviceWithIdAssignment
    | batchAssignment
    | deviceWithIndexExtendedAssignment
    | deviceWithIndexAssignment
    | memberExtendedAssignment
    | memberAssignment
    | assignment
    | yieldStatement
    | hcfStatement
    | sleepStatement
    | returnValueStatement
    | returnStatement
    | continueStatement
    | breakStatement
    | variableDeclaration
    | constantDeclaration
    | functionCallStatement
    | arrayDeclaration
    | arrayAssignment
);

yieldStatement: YIELD;
hcfStatement: HCF;
sleepStatement: SLEEP '(' expression ')';
returnStatement: RETURN;
returnValueStatement: RETURN expression;
continueStatement: CONTINUE;
breakStatement: BREAK;
functionCallStatement: IDENTIFIER '(' (expression (',' expression)*)? ')';

undelimitedStatement: whileStatement | forStatement | ifStatement;

whileStatement: WHILE '(' expression ')' (block | statement);
forStatement: FOR '(' statement1=delimitedStatement? ';' expression? ';' statement2=delimitedStatement? ')' (block | innerStatement=statement);

ifStatement: IF '(' expression ')' (block | statement) ( ELSE (block | statement))?;

deviceWithIdAssignment: DEVICE_WITH_ID '(' deviceIdxExpr=expression ')' '.' member=IDENTIFIER '=' valueExpr=expression;
deviceWithIdExtendedAssignment: DEVICE_WITH_ID '(' deviceIdxExpr=expression ')' '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? '=' valueExpr=expression;

batchAssignment: DEVICES_OF_TYPE '(' deviceTypeHashExpr=expression ')'
    ('.' WITH_NAME '(' deviceNameHashExpr=expression ')')?
    ('.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']')?
    '.' member=IDENTIFIER '=' valueExpr=expression;

memberExtendedAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? '=' valueExpr=expression;
memberAssignment: identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER '=' valueExpr=expression;

deviceWithIndexExtendedAssignment: PINS '[' deviceIdxExpr=expression ']' '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? '=' valueExpr=expression;
deviceWithIndexAssignment: PINS '[' deviceIdxExpr=expression ']' '.' member=IDENTIFIER '=' valueExpr=expression;

deviceStackClear: identifier=(BASE_DEVICE | IDENTIFIER) '.' STACK '.' CLEAR '(' ')';
deviceWithIdStackClear: DEVICE_WITH_ID '(' deviceIdxExpr=expression ')' '.' STACK '.' CLEAR '(' ')';

assignment: IDENTIFIER '=' expression;

variableDeclaration: VAR IDENTIFIER '=' expression;
constantDeclaration: CONST IDENTIFIER '=' expression;

arrayDeclaration:
    VAR IDENTIFIER '=' '[' sizeExpr=expression ']' # arraySizeDeclaration
    | VAR IDENTIFIER '=' '{' (expression (',' expression)*)? ','? '}' # arrayListDeclaration
    ;

arrayAssignment: IDENTIFIER '[' indexExpr=expression ']' '=' valueExpr=expression;

expression:
    op=DIRECT_NULLARY_OPERATOR '(' ')' #NullaryOp
    | op=DIRECT_UNARY_OPERATOR '(' operand=expression ')' # UnaryOp
    | op=DIRECT_BINARY_OPERATOR '(' left=expression ',' right=expression ')' # BinaryOp
    | op=DIRECT_TERNARY_OPERATOR '(' a=expression ',' b=expression ',' c=expression ')' # TernaryOp
    | op=(NEGATION | SUB | BITWISE_NOT) operand=expression # UnaryOp
    | left=expression op=(SHIFTL | SHIFTR | SHIFTLA | SHIFTRA) right=expression # BinaryOp
    | left=expression op=(MUL | DIV | MOD) right=expression # BinaryOp
    | left=expression op=(ADD | SUB) right=expression # BinaryOp
    | left=expression op=(LT | GT | LE | GE | EQ | NE) right=expression # BinaryOp
    | a=expression op=(AEQ | ANE | SEL) b=expression ':' c=expression # TernaryOp
    | left=expression op=AND right=expression # BinaryOp
    | left=expression op=(OR | XOR) right=expression # BinaryOp
    | '(' expression ')' # Parenthesis
    | type=(INTEGER | BOOLEAN | REAL | STRING_LITERAL) # Literal
    | IDENTIFIER '(' (expression (',' expression)*)? ')' # FunctionCall
    | IDENTIFIER # Identifier
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' member=IDENTIFIER # MemberAccess
    | identifier=(BASE_DEVICE | IDENTIFIER) '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? # ExtendedMemberAccess
    | PINS '[' deviceIdxExpr=expression ']' '.' member=IDENTIFIER # DeviceIndexAccess
    | PINS '[' deviceIdxExpr=expression ']' '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? # ExtendedDeviceIndexAccess
    | DEVICE_WITH_ID '(' deviceIdxExpr=expression ')' '.' member=IDENTIFIER # DeviceIdAccess
    | DEVICE_WITH_ID '(' deviceIdxExpr=expression ')' '.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']' ('.' member=IDENTIFIER)? # ExtendedDeviceIdAccess
    | DEVICES_OF_TYPE '(' deviceTypeHashExpr=expression ')' ('.' WITH_NAME '(' deviceNameHashExpr=expression ')')? ('.' prop=(SLOTS | REAGENTS | STACK) '[' targetIdxExpr=expression ']')? '.' member=IDENTIFIER '.' batchMode=IDENTIFIER # BatchAccess
    | IDENTIFIER '[' indexExpr=expression ']' # ArrayElementAccess
    ;

// Lexer rules
PINID: 'db' | 'd0' | 'd1' | 'd2' | 'd3' | 'd4' | 'd5';
WHILE: 'while';
FOR: 'for';
IF: 'if';
ELSE: 'else';
YIELD: 'yield';
HCF: 'hcf';
SLEEP: 'sleep';
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
SHIFTL: '<<' | '<<l';
SHIFTR: '>>' | '>>l';
SHIFTLA: '<<a';
SHIFTRA: '>>a';
LT: '<';
GT: '>';
LE: '<=';
GE: '>=';
AND: '&';
OR: '|';
XOR: '^';
EQ: '==';
NE: '!=';
AEQ: '~=' | '~==';
ANE: '~!=';
SEL: '?';
NEGATION: '!';
PINS: 'Pins';
SLOTS: 'Slots';
REAGENTS: 'Reagent';
STACK: 'Stack';
CLEAR: 'Clear';
DEVICE_WITH_ID: 'DeviceWithId';
DEVICES_OF_TYPE: 'DevicesOfType';
WITH_NAME: 'WithName';

DIRECT_NULLARY_OPERATOR:
    'rand';

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
    | 'sgt'
    | 'sge'
    | 'slt'
    | 'sle'
    | 'sapz'
    | 'snaz';

DIRECT_TERNARY_OPERATOR:
    'select'
    | 'sap'
    | 'sna';

BOOLEAN: 'true' | 'false';
IDENTIFIER: [a-zA-Z_][a-zA-Z_0-9]*;
INTEGER: [0-9]+;
STRING_LITERAL: '"' ~[\r\n"]* '"';

REAL: [0-9]* '.' [0-9]+;

WS: [ \t\r\n]+ -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;
MULTILINE_COMMENT : '/*' ( MULTILINE_COMMENT | . )*? '*/'  -> skip;