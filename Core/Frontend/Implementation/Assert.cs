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
        return TypeHelper.GetType(first) != TypeHelper.GetType(second) && TypeHelper.GetType(first) != ScratchType.Any &&
               TypeHelper.GetType(second) != ScratchType.Any;
    }

    public bool AssertVariable(ParserRuleContext context, object obj, IToken identifier)
    {
        if (!obj.IsVariable())
        {
            DiagnosticReporter.Error(ScratchScriptError.VariableIdentifierExpected, context, identifier);
            return true;
        }

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