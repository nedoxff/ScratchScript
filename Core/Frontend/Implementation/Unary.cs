using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override TypedValue? VisitNotExpression(ScratchScriptParser.NotExpressionContext context)
    {
        var expression = Visit(context.expression());
        if (AssertNotNull(context, expression, context.expression())) return null;
        if (AssertType(context, expression!.Value.Type, ScratchType.Boolean, context.expression())) return null;

        var result = $"!{expression.Format()}";
        return HydrateValue(new(result, ScratchType.Boolean), expression);
    }

    public override TypedValue? VisitUnaryAddExpression(ScratchScriptParser.UnaryAddExpressionContext context)
    {
        var expression = Visit(context.expression());
        if (AssertNotNull(context, expression, context.expression())) return null;
        if (AssertType(context, expression!.Value.Type, ScratchType.Number, context.expression())) return null;

        var op = context.addOperators().GetText();
        var multiplier = op == "-" ? -1 : 1;
        var result = $"* {multiplier} {expression}";
        return HydrateValue(new(result, ScratchType.Number), expression);
    }
}