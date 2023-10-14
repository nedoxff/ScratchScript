using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Information;

public static class Stack
{
    public static TypedValue GetArgument(ScratchVariable variable)
    {
        var procedure = ScratchScriptVisitor.Instance.Procedures.Last();
        var index = procedure.Arguments.Keys.ToList().FindIndex(x => x == variable.Name);
        var shift = procedure.Arguments.Count - (index + 1);
        return new(
            $"{ScratchScriptVisitor.StackName}#{(shift == 0 ? ":si:" : $"(- :si: {shift})")}",
            procedure.Arguments[variable.Name],
            new()
            {
                { "ARGUMENT_NAME", variable.Name }
            });
    }

    public static string PushArgument(object expression) =>
        $"push {ScratchScriptVisitor.StackName} {expression.Format()}\n";

    public static string PopArguments() =>
        $"repeat {ScratchScriptVisitor.Instance.Procedures.Last().Arguments.Count}\n{ScratchScriptVisitor.PopStackCommand}\nend";
    
    public static string PopFunctionArguments() => ScratchScriptVisitor.Instance.Scope.ProcedureIndex == 0 ? "": $"repeat {ScratchScriptVisitor.Instance.Scope.ProcedureIndex}\n{ScratchScriptVisitor.PopFunctionStackCommand}\nend";
}