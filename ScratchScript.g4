grammar ScratchScript;
/*
Parser
*/

program: topLevelStatement* EOF;
topLevelStatement: procedureDeclarationStatement | attributeStatement | eventStatement | importStatement | namespaceStatement;
line: (statement | ifStatement | whileStatement | repeatStatement | switchStatement | comment);
statement: (assignmentStatement | procedureCallStatement | memberProcedureCallStatement |  variableDeclarationStatement | returnStatement | breakStatement | continueStatement) Semicolon;

eventStatement: Event Identifier (LeftParen (expression (Comma expression)*?) RightParen)? block;
assignmentStatement: Identifier assignmentOperators expression;
variableDeclarationStatement: VariableDeclare Identifier Assignment expression;
memberProcedureCallStatement: expression Dot procedureCallStatement;
procedureCallStatement: Identifier LeftParen (procedureArgument (Comma procedureArgument)*?)? RightParen; 
procedureDeclarationStatement: attributeStatement*? ProcedureDeclare Identifier LeftParen (Identifier (Comma Identifier)*?)? RightParen block; 
ifStatement: If expression block (Else elseIfStatement)?;
whileStatement: While expression block;
elseIfStatement: block | ifStatement;
importStatement: Import (LeftBrace Identifier (Comma Identifier)*? RightBrace From)? String Semicolon;
attributeStatement: At Identifier (LeftParen (constant (Comma constant)*?)? RightParen)?;
returnStatement: Return expression;
repeatStatement: Repeat expression block;
breakStatement: Break;
continueStatement: Continue;
namespaceStatement: Namespace String Semicolon;
switchStatement: Switch expression switchBlock;

procedureArgument: (Identifier ':')? expression;

expression
    : constant #constantExpression
    | Identifier #identifierExpression
    | procedureCallStatement #procedureCallExpression
    | expression Dot procedureCallStatement #memberProcedureCallExpression
    | LeftBracket expression
    | LeftParen expression RightParen #parenthesizedExpression
    | Not expression #notExpression
    | expression LeftBracket expression RightBracket #arrayAccessExpression
    | addOperators expression #unaryAddExpression
    | expression multiplyOperators expression #binaryMultiplyExpression
    | expression addOperators expression #binaryAddExpression
    | expression compareOperators expression #binaryCompareExpression
    | expression booleanOperators expression #binaryBooleanExpression
    | expression Ternary expression Colon expression #ternaryExpression
    ;

multiplyOperators: Multiply | Divide | Modulus | Power;
addOperators: Plus | Minus;
compareOperators: Equal | NotEqual | Greater | GreaterOrEqual | Lesser | LesserOrEqual;
booleanOperators: And | Or | Xor;
assignmentOperators: Assignment | AdditionAsignment | SubtractionAssignment | MultiplicationAssignment | DivisionAssignment | ModulusAssignment;

case: (Case constant Colon block) | defaultCase;
block: LeftBrace line* RightBrace;
switchBlock: LeftBrace case* RightBrace;
defaultCase: Default Colon block;

constant: Number | String | boolean | Color;
comment: Comment;
boolean: True | False;

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

Comment
    :   ( SingleLineCommentStart (~[\r\n]|Whitespace)* 
        | MultiLineCommentStart .*? MultiLineCommentEnd
        )
    ;

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
Xor: '^';

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

/*
    Keywords
*/
If: 'if' Whitespace+;

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
Continue: 'continue';
Default: 'default';

Case: 'case' Whitespace+;
Switch: 'switch' Whitespace+;
While: 'while' Whitespace+;
VariableDeclare: 'var' Whitespace+;
Import: 'import' Whitespace+;
ProcedureDeclare: 'function' Whitespace+;
Return: 'return' Whitespace+;
Repeat: 'repeat' Whitespace+;
Event: 'on' Whitespace+;
From: 'from' Whitespace+;
Namespace: 'namespace' Whitespace+;

/*
    Lexer rules
*/
Number: Digit+ ([.] Digit+)?; 
Identifier: [a-zA-Z_][a-zA-Z0-9_]*;
String: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
Color: Hashtag HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit;