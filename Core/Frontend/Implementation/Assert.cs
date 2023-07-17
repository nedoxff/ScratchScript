using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Models;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public void AssertType(ParserRuleContext context, object first, object second, ParserRuleContext conflicting = null)
    {
        if (first is string firstString && firstString.Contains(":argr") && GetType(first) == ScratchType.Unknown)
        {
            var argumentName = firstString.Split(":")[2];
            _procedures.Last().Arguments[argumentName] = GetType(second);
            SaveType(firstString, GetType(second));
            return;
        }
        if (GetType(first) != GetType(second))
            DiagnosticReporter.Error(ScratchScriptError.TypeMismatch, context, conflicting ?? ParserRuleContext.EMPTY, first, second);
    }

    public void Assert<T>(ParserRuleContext context, object o, ParserRuleContext conflicting = null)
    {
        if (o is not T)
            DiagnosticReporter.Error(ScratchScriptError.IceVisitorTypeMismatch, context,
                conflicting ?? ParserRuleContext.EMPTY, typeof(T).Name, o is null ? "null" : o.GetType().Name);
    }
}