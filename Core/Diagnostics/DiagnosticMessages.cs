using Antlr4.Runtime;

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
    ProcedureExpressionDoesNotReturn
}

public enum ScratchScriptWarning
{
    
}

public class DiagnosticMessages
{
    public static readonly List<string> Errors = new()
    {
        "Variable with name {0} is not defined.",
        "ICE: Expected Block, got {0}.",
        "Type mismatch: {0} is not the same as {1}.",
        "ICE: Expected {0}, got {1}.",
        "Member (function or variable) with the same name ({0}) is already defined",
        "Function with name {0} is not defined.",
        "Function {0} requires {1} arguments, but got {2}.",
        "Function {0} does not return a value."
    };
    public static readonly List<string> Warnings = new() { };
}