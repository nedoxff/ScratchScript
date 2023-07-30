using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override TypedValue? VisitNotExpression(ScratchScriptParser.NotExpressionContext context)
    {
        var expression = Visit(context.expression());
        AssertType(context, expression.Value.Type, ScratchType.Boolean, context.expression());

        var result = $"!{expression.Format()}";
        return new(result, ScratchType.Boolean);
    }

    public override TypedValue? VisitUnaryAddExpression(ScratchScriptParser.UnaryAddExpressionContext context)
    {
        var expression = Visit(context.expression());
        AssertType(context, expression.Value.Type, ScratchType.Number, context.expression());

        var op = context.addOperators().GetText();
        var multiplier = op == "-" ? -1 : 1;
        var result = $"* {multiplier} {expression}";
        return new(result, ScratchType.Number);
    }
}