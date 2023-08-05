using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public bool AssertType(ParserRuleContext context, object first, object second, ParserRuleContext conflicting = null)
    {
        if (first is TypedValue { Value: string } firstTyped && firstTyped.Data.ContainsKey("ARGUMENT_NAME") &&
            TypeHelper.GetType(first) == ScratchType.Unknown)
        {
            Procedures.Last().Arguments[firstTyped.Data["ARGUMENT_NAME"]] = TypeHelper.GetType(second);
            return false;
        }

        if ((TypeHelper.GetType(first) != TypeHelper.GetType(second)) && (first.IsVariable() && TypeHelper.GetType(second) != ScratchType.Variable))
        {
            DiagnosticReporter.Error(ScratchScriptError.TypeMismatch, context, conflicting ?? ParserRuleContext.EMPTY,
                first, second);
            return true;
        }

        return false;
    }

    public bool AssertNotNull(ParserRuleContext context, object obj, ParserRuleContext conflicting = null)
    {
        if (obj is null)
        {
            //DiagnosticReporter.Error(ScratchScriptError., context, conflicting ?? ParserRuleContext.EMPTY,
            //                first, second);
            //TODO: error
            return true;
        }

        return false;
    }

    public void Assert<T>(ParserRuleContext context, object o, ParserRuleContext conflicting = null)
    {
        if (o is not T)
            DiagnosticReporter.Error(ScratchScriptError.IceVisitorTypeMismatch, context,
                conflicting ?? ParserRuleContext.EMPTY, typeof(T).Name, o is null ? "null" : o.GetType().Name);
    }
}