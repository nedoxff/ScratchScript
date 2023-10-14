using ScratchScript.Core.Diagnostics;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private decimal _floatingPointPrecision = new(0.00001);
    private bool _useFloatEquation;
    private bool _useUnicode;

    private Dictionary<string, string> _forbiddenImports = new();

    private void HandleProcedureAttribute(ScratchScriptParser.AttributeStatementContext context,
        ref ScratchIrProcedure procedure)
    {
        switch (context.Identifier().GetText())
        {
            case "__DisableTypeCheck":
            {
                procedure.Attributes.Add("DISABLE_TYPE_CHECK", "");
                return;
            }
            case "warp":
            {
                procedure.Warp = true;
                return;
            }
            case "extension":
            {
                var type = Visit(context.constant(0));
                if (AssertNotNull(context, type, context.constant(0))) return;
                if (AssertType(context, type, ScratchType.Type, context.constant(0))) return;
                procedure.CallerType = type.Value.Value as ScratchType;
                //TODO: add a check that the function has the argument
                return;
            }
        }
    }

    private void HandleTopLevelAttribute(ScratchScriptParser.AttributeStatementContext context)
    {
        switch (context.Identifier().GetText())
        {
            case "useFloatEquation":
            {
                _useFloatEquation = true;
                if (context.constant(0) != null)
                    _floatingPointPrecision = (decimal)Visit(context.constant(0)).Value.Value;
                break;
            }
            case "unicode":
            {
                _useUnicode = true;
                EnableUnicode();
                break;
            }
            case "forbidImport":
            {
                if (AssertNotNull(context, context.constant(0), context.constant(0))) return;
                var message = Visit(context.constant(0));
                if (AssertType(context, message, ScratchType.String, context.constant(0))) return;

                _forbiddenImports[Path.GetFileNameWithoutExtension(InputFile)!] = (string)message?.Value;
                break;
            }
        }
    }

    public override TypedValue? VisitAttributeStatement(ScratchScriptParser.AttributeStatementContext context)
    {
        var shouldWarn = _imports.Count != 0 || Namespace != "global" || Procedures.Count != 0;
        if (shouldWarn)
            DiagnosticReporter.Warning(ScratchScriptWarning.TopLevelAttributeNotAtTop, context, context);

        HandleTopLevelAttribute(context);
        return null;
    }
}