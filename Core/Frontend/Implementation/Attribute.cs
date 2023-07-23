using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private decimal _floatingPointPrecision = new(0.00001);
    private bool _useFloatEquation = false;
    private void HandleProcedureAttribute(ScratchScriptParser.AttributeStatementContext context,
        ref ScratchIrProcedure procedure)
    {
        switch (context.Identifier().GetText())
        {
            case "__ReturnType":
            {
                var type = (ScratchType)Convert.ToInt32(Visit(context.constant(0)));
                procedure.ReturnType = type;
                return;
            }
            case "__ArgumentType":
            {
                var name = ((string)Visit(context.constant(0)))[1..^1];
                var type = (ScratchType)Convert.ToInt32(Visit(context.constant(1)));
                procedure.Arguments[name] = type;
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
                    _floatingPointPrecision = (decimal)Visit(context.constant(0));
                break;
            }
        }
    }

    public override object VisitAttributeStatement(ScratchScriptParser.AttributeStatementContext context)
    {
        var shouldWarn = _imports.Count != 0 || Namespace != "global" || _procedures.Count != 0;
        if (shouldWarn)
        {
            //TODO: warning
        }
        HandleTopLevelAttribute(context);
        return null;
    }
}