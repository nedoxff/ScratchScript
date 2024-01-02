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

    public static string PopFunctionArguments()
    {
        return @$"set var:__CleanupCounter :si:
repeat {ScratchScriptVisitor.Instance.Procedures.Last().Arguments.Count}
popat __Stack var:__CleanupCounter
set var:__CleanupCounter (- var:__CleanupCounter 1)
end";
    }
}