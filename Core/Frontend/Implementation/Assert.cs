using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public bool AssertType(ParserRuleContext context, object first, object second, ParserRuleContext conflicting)
    {
        var error = AssertTypeInternal(first, second);
        if (error)
        {
            DiagnosticReporter.Error(ScratchScriptError.TypeMismatch, context, conflicting,
                TypeHelper.GetType(first), TypeHelper.GetType(second));
        }

        return error;
    }
    
    public bool AssertType(ParserRuleContext context, object first, object second, IToken conflicting)
    {
        var error = AssertTypeInternal(first, second);
        if (error)
        {
            DiagnosticReporter.Error(ScratchScriptError.TypeMismatch, context, conflicting,
                TypeHelper.GetType(first), TypeHelper.GetType(second));
        }

        return error;
    }

    private bool AssertTypeInternal(object first, object second)
    {
        if (first is TypedValue { Value: string } firstTyped && firstTyped.Data.ContainsKey("ARGUMENT_NAME") &&
            TypeHelper.GetType(first) == ScratchType.Unknown)
        {
            Procedures.Last().Arguments[firstTyped.Data["ARGUMENT_NAME"]] = TypeHelper.GetType(second);
            return false;
        }

        if ((first.IsVariable() && TypeHelper.GetType(second) == ScratchType.Variable) ||
            (second.IsVariable() && TypeHelper.GetType(first) == ScratchType.Variable))
            return false;

        if (TypeHelper.GetType(first) != TypeHelper.GetType(second))
            return true;

        return false;
    }

    public bool AssertNotNull(ParserRuleContext context, object obj, ParserRuleContext conflicting)
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
    
    public bool AssertNotNull(ParserRuleContext context, object obj, IToken conflicting)
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
}