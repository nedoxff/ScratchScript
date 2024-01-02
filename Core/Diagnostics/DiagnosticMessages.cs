namespace ScratchScript.Core.Diagnostics;

public enum ScratchScriptError
{
    VariableNotDefined,
    IceBlockExpected,
    TypeMismatch,
    IceVisitorTypeMismatch,
    IdentifierAlreadyUsed,
    ProcedureNotDefined,
    ProcedureArgumentCountDifferent,
    ProcedureExpressionDoesNotReturn,
    NamespacePlacedIncorrectly,
    UnknownIdentifier,
    ValueNotAllowed,
    UnknownNamespace,
    VariableIdentifierExpected,
    IceStdFileContainsErrors,
    IceImportedFunctionIsNotDefined,
    FunctionDoesNotExistInImportedFile,
    FunctionNamespaceDoesNotIncludeFunction
}

public enum ScratchScriptWarning
{
    SwitchStatementEmpty,
    TopLevelAttributeNotAtTop,
    DivisionByZero
}

public enum ScratchScriptNote
{
    FileAlreadyImported,
    FunctionNamespaceDeclared
}

public class DiagnosticMessages
{
    public static readonly List<string> Errors = new()
    {
        "variable with name {0} is not defined",
        "(ICE) expected Block, got {0}",
        "expected {1}, got {0}",
        "(ICE) expected {0}, got {1}",
        "member (function or variable) with the same name ({0}) is already defined",
        "function with name {0} is not defined",
        "function {0} requires {1} arguments, but got {2}.",
        "function {0} does not return a value",
        "namespace must be declared at the top of the file",
        "unknown identifier \"{0}\"",
        "invalid argument value. expected to be one of the following: {0}",
        "unknown namespace \"{0}\"",
        "expected a variable identifier",
        "(ICE) the imported std file contains errors",
        "(ICE) required imported function {0} was not defined",
        "function {0} is not defined in the imported file",
        "{0} does not have the function {1}"
    };
    public static readonly List<string> Warnings = new()
    {
        "switch statement contains no cases",
        "top-level attributes should be declared before any function/block to avoid issues",
        "dividing by 0 is not recommended and can lead to issues"
    };

    public static readonly List<string> Notes = new()
    {
        "{0} is already imported by {1}",
        "{0} is declared in this import"
    };
}