using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override object VisitNotExpression(ScratchScriptParser.NotExpressionContext context)
    {
        var expression = Visit(context.expression());
        AssertType(context, GetType(expression), ScratchType.Boolean, context.expression());

        var result = $"!{expression.Format()}";
        SaveType(result, ScratchType.Boolean);
        return result;
    }

    public override object VisitUnaryAddExpression(ScratchScriptParser.UnaryAddExpressionContext context)
    {
        var expression = Visit(context.expression());
        AssertType(context, GetType(expression), ScratchType.Number, context.expression());

        var op = context.addOperators().GetText();
        var multiplier = op == "-" ? -1 : 1;
        var result = $"* {multiplier} {expression}";
        SaveType(result, ScratchType.Number);
        return result;
    }
}