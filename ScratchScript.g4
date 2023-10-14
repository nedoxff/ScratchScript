grammar ScratchScript;
/*
Parser
*/

program: topLevelStatement* EOF;
topLevelStatement: procedureDeclarationStatement | attributeStatement | eventStatement | importStatement | namespaceStatement;
line: ((statement Semicolon) | ifStatement | whileStatement | repeatStatement | forStatement | switchStatement | returnStatement | breakStatement | throwStatement | comment);
statement: assignmentStatement | listAssignmentStatement | procedureCallStatement | memberProcedureCallStatement | variableDeclarationStatement | postIncrementStatement;

eventStatement: Event Identifier (LeftParen (expression (Comma expression)*?) RightParen)? block;
assignmentStatement: Identifier assignmentOperators expression;
listAssignmentStatement: Identifier LeftBracket expression RightBracket;
variableDeclarationStatement: VariableDeclare Identifier Assignment expression;
memberProcedureCallStatement: expression Dot procedureCallStatement;
procedureCallStatement: Identifier LeftParen (procedureArgument (Comma procedureArgument)*?)? RightParen; 
procedureDeclarationStatement: attributeStatement*? ProcedureDeclare Identifier LeftParen (typedIdentifier (Comma typedIdentifier)*?)? RightParen block; 
ifStatement: If LeftParen expression RightParen block (Else elseIfStatement)?;
whileStatement: While LeftParen expression RightParen block;
forStatement: For LeftParen statement? Semicolon expression? Semicolon statement? RightParen block;
elseIfStatement: block | ifStatement;
postIncrementStatement: Identifier postIncrementOperators;
importStatement: Import (LeftBrace Identifier (Comma Identifier)*? RightBrace From)? String Semicolon;
attributeStatement: At Identifier (LeftParen (constant (Comma constant)*?)? RightParen)?;
returnStatement: Return expression Semicolon;
repeatStatement: Repeat LeftParen expression RightParen block;
throwStatement: Throw String Semicolon;
breakStatement: Break Semicolon;
namespaceStatement: Namespace String Semicolon;
switchStatement: Switch LeftParen expression RightParen switchBlock;
typedIdentifier: Identifier Colon type;
procedureArgument: (Identifier ':')? expression;

expression
    : constant #constantExpression
    | Identifier #identifierExpression
    | procedureCallStatement #procedureCallExpression
    | expression Dot procedureCallStatement #memberProcedureCallExpression
    | LeftBracket (expression (Comma expression)*?)? RightBracket #arrayInitializeExpression
    | LeftParen expression RightParen #parenthesizedExpression
    | Not expression #notExpression
    | expression LeftBracket expression RightBracket #arrayAccessExpression
    | addOperators expression #unaryAddExpression
    | expression BitwiseOr expression #binaryBitwiseOrExpression
    | expression BitwiseXor expression #binaryBitwiseXorExpression
    | expression BitwiseAnd expression #binaryBitwiseAndExpression
    | expression shiftOperators expression #binaryBitwiseShiftExpression
    | expression multiplyOperators expression #binaryMultiplyExpression
    | expression addOperators expression #binaryAddExpression
    | expression compareOperators expression #binaryCompareExpression
    | expression booleanOperators expression #binaryBooleanExpression
    | expression Ternary expression Colon expression #ternaryExpression
    ;

multiplyOperators: Multiply | Divide | Modulus | Power;

shiftOperators: leftShift | rightShift;
leftShift: first=Lesser second=Lesser {$first.index + 1 == $second.index}?;
rightShift: first=Greater second=Greater {$first.index + 1 == $second.index}?;

addOperators: Plus | Minus;
compareOperators: Equal | NotEqual | Greater | GreaterOrEqual | Lesser | LesserOrEqual;
booleanOperators: And | Or;
assignmentOperators: Assignment | AdditionAsignment | SubtractionAssignment | MultiplicationAssignment | DivisionAssignment | ModulusAssignment | PowerAssignment;
postIncrementOperators: PostIncrement | PostDecrement;

case: (Case constant Colon block) | defaultCase;
block: LeftBrace line* RightBrace;
switchBlock: LeftBrace case* RightBrace;
defaultCase: Default Colon block;

constant: Number | String | boolean | Color | type;
comment: Comment;
boolean: True | False;
type: Type | 'List' '<' type '>';

/*
    Lexer fragments
*/

fragment Digit: [0-9];
fragment HexDigit: [0-9a-fA-F];
Whitespace: (' ' | '\t') -> channel(HIDDEN);
NewLine: ('\r'? '\n' | '\r' | '\n') -> skip;
Semicolon: ';';
LeftParen: '(';
RightParen: ')';
LeftBracket: '[';
RightBracket: ']';
LeftBrace: '{';
RightBrace: '}';
Assignment: '=';
Comma: ',';
Not: '!';
Arrow: '->';
Colon: ':';
Dot: '.';
Ternary: '?';

SingleLineCommentStart: '//';
MultiLineCommentStart: '/*';
MultiLineCommentEnd: '*/';

Comment: MultiLineCommentStart .*? MultiLineCommentEnd -> skip;
LineComment: SingleLineCommentStart ~[\r\n]* -> skip;

At: '@';
Hashtag: '#';

Multiply: '*';
Plus: '+';
Minus: '-';
Divide: '/';
Power: '**';
Modulus: '%';

And: '&&';
Or: '||';

BitwiseAnd: '&';
BitwiseOr: '|';
BitwiseXor: '^';

PostIncrement: '++';
PostDecrement: '--';

//<assoc=right>
Greater: '>';
Lesser: '<';
GreaterOrEqual: '>=';
LesserOrEqual: '<=';
Equal: '==';
NotEqual: '!=';

AdditionAsignment: '+=';
SubtractionAssignment: '-=';
MultiplicationAssignment: '*=';
DivisionAssignment: '/=';
ModulusAssignment: '%=';
PowerAssignment: '**=';

/*
    Keywords
*/
If: 'if';

/*Very important for newlines:

else
{
}
and
else if ...
{
}
and
else {
}

*/
Else: 'else';
True: 'true';
False: 'false';
Break: 'break';
Default: 'default';

For: 'for';
Case: 'case' Whitespace+;
Switch: 'switch';
While: 'while';
VariableDeclare: 'var' Whitespace+;
Import: 'import' Whitespace+;
ProcedureDeclare: 'function' Whitespace+;
Return: 'return' Whitespace+;
Throw: 'throw' Whitespace+;
Repeat: 'repeat';
Event: 'on' Whitespace+;
From: 'from' Whitespace+;
Namespace: 'namespace' Whitespace+;
Type: 'number' | 'string' | 'bool' | 'color' | 'any';

/*
    Lexer rules
*/
Number: Digit+ ([.] Digit+)?; 
Identifier: [a-zA-Z_][a-zA-Z0-9_]*;
String: ('"' (~('"' | '\\' | '\r' | '\n') | '\\' ('"' | '\\'))* '"') | ('\'' (~('\'' | '\\' | '\r' | '\n') | '\\' ('\'' | '\\'))* '\'');
Color: Hashtag HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit;