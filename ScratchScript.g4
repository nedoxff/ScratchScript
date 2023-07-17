grammar ScratchScript;
/*
Parser
*/

program: topLevelStatement* EOF;
topLevelStatement: attributeStatement | procedureDeclarationStatement | eventStatement;
line: (statement | ifStatement | whileStatement | repeatStatement | comment);
statement: (assignmentStatement | procedureCallStatement | variableDeclarationStatement | importStatement | returnStatement | breakStatement | continueStatement) Semicolon;

eventStatement: Event Identifier (LeftParen (expression (Comma expression)*?) RightParen)? block;
assignmentStatement: Identifier assignmentOperators expression;
variableDeclarationStatement: VariableDeclare Identifier Assignment expression;
procedureCallStatement: Identifier LeftParen (procedureArgument (Comma procedureArgument)*?)? RightParen;
procedureDeclarationStatement: ProcedureDeclare Identifier LeftParen (Identifier (Comma Identifier)*?)? RightParen block; 
ifStatement: If expression block (Else elseIfStatement)?;
whileStatement: While expression block;
elseIfStatement: block | ifStatement;
importStatement: Import String;
attributeStatement: At Identifier;
returnStatement: Return expression;
repeatStatement: Repeat expression block;
breakStatement: Break;
continueStatement: Continue;

procedureArgument: (Identifier ':')? expression;

expression
    : constant #constantExpression
    | Identifier #identifierExpression
    | procedureCallStatement #procedureCallExpression
    | LeftParen expression RightParen #parenthesizedExpression
    | Not expression #notExpression
    | addOperators expression #unaryAddExpression
    | expression multiplyOperators expression #binaryMultiplyExpression
    | expression addOperators expression #binaryAddExpression
    | expression compareOperators expression #binaryCompareExpression
    | expression booleanOperators expression #binaryBooleanExpression
    ;

multiplyOperators: Multiply | Divide | Power | Modulus;
addOperators: Plus | Minus;
compareOperators: Equal | NotEqual | Greater | GreaterOrEqual | Lesser | LesserOrEqual;
booleanOperators: And | Or | Xor;
assignmentOperators: Assignment | AdditionAsignment | SubtractionAssignment | MultiplicationAssignment | DivisionAssignment | ModulusAssignment;

block: LeftBrace line* RightBrace;

constant: Number | String | boolean | Color | EmptyArray;
comment: Comment;
boolean: True | False;

/*
    Lexer fragments
*/

fragment Digit: [0-9];
fragment HexDigit: [0-9A-F];
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

EmptyArray: '[]';

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

While: 'while' Whitespace+;
VariableDeclare: 'var' Whitespace+;
Import: 'import' Whitespace+;
ProcedureDeclare: 'function' Whitespace+;
Return: 'return' Whitespace+;
Repeat: 'repeat' Whitespace+;
Event: 'on' Whitespace+;

/*
    Lexer rules
*/
Number: Digit+ ([.] Digit+)?; 
Identifier: [a-zA-Z_][a-zA-Z0-9_]*;
String: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
Color: Hashtag HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit;