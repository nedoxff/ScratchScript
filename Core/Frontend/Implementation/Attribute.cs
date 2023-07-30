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
        }
    }

    private void HandleProcedureArgumentAttribute(ScratchScriptParser.AttributeStatementContext context, string name,
        ref ScratchIrProcedure procedure)
    {
        procedure.Arguments[name] = context.Identifier().GetText() switch
        {
            "num" => ScratchType.Number,
            "str" => ScratchType.String,
            "bool" => ScratchType.Boolean,
            "color" => ScratchType.Color,
            _ => procedure.Arguments[name]
        };
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
        {
            //TODO: warning
        }
        HandleTopLevelAttribute(context);
        return null;
    }
}