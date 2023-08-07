using ScratchScript.Core.Diagnostics;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private decimal _floatingPointPrecision = new(0.00001);
    private bool _useFloatEquation;

    private void HandleProcedureAttribute(ScratchScriptParser.AttributeStatementContext context,
        ref ScratchIrProcedure procedure)
    {
        switch (context.Identifier().GetText())
        {
            case "__ReturnType":
            {
                var type = (ScratchType)Convert.ToInt32(Visit(context.constant(0)).Value.Value);
                procedure.ReturnType = type;
                return;
            }
            case "warp":
            {
                procedure.Warp = true;
                return;
            }
            case "extension":
            {
                var type = TypeHelper.StringToScratchType(((string)Visit(context.constant(0)).Value.Value)[1..^1]);
                procedure.CallerType = type;
                //TODO: add a check that the function has the argument
                return;
            }
        }
    }

    private void HandleProcedureArgumentAttribute(ScratchScriptParser.AttributeStatementContext context, string name,
        ref ScratchIrProcedure procedure)
    {
        var attributeName = context.Identifier().GetText();
        if (TypeHelper.PossibleTypes.Contains(attributeName))
            procedure.Arguments[name] =
                TypeHelper.StringToScratchType(attributeName);
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