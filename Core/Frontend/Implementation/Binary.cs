using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{

    public override object VisitBinaryCompareExpression(ScratchScriptParser.BinaryCompareExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.compareOperators().GetText();

        AssertType(context, first, second);
        
        var isString = op == "==" && GetType(first) == ScratchType.String && GetType(second) == ScratchType.String;
        var isBoolean = op == "==" && GetType(first) == ScratchType.Boolean && GetType(second) == ScratchType.Boolean;

        if (!isString && !isBoolean)
        {
            AssertType(context, first, ScratchType.Number, context.expression(0));
            AssertType(context, second, ScratchType.Number, context.expression(1));
        }

        var result = $"{op} {FormatString(first)} {FormatString(second)}";
        SaveType(result, ScratchType.Boolean);
        return result;
    }

    public override object VisitBinaryMultiplyExpression(ScratchScriptParser.BinaryMultiplyExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.multiplyOperators().GetText();
        
        AssertType(context, first, ScratchType.Number, context.expression(0));
        AssertType(context, second, ScratchType.Number, context.expression(1));
        
        var result = $"{op} {FormatString(first)} {FormatString(second)}";
        SaveType(result, ScratchType.Number);
        return result;
    }

    public override object VisitBinaryAddExpression(ScratchScriptParser.BinaryAddExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.addOperators().GetText();

        var isString = op == "+" && GetType(first) == ScratchType.String && GetType(second) == ScratchType.String;

        if (!isString)
        {
            AssertType(context, first, ScratchType.Number, context.expression(0));
            AssertType(context, second, ScratchType.Number, context.expression(1));
        }
        else op = "~"; 

        var result = $"{op} {FormatString(first)} {FormatString(second)}";
        SaveType(result, isString ? ScratchType.String: ScratchType.Number);
        return result;
    }
    
    public override object VisitBinaryBooleanExpression(ScratchScriptParser.BinaryBooleanExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.booleanOperators().GetText();
        
        AssertType(context, first, ScratchType.Boolean, context.expression(0));
        AssertType(context, second, ScratchType.Boolean, context.expression(1));
        
        var result = $"{op} {FormatString(first)} {FormatString(second)}";
        SaveType(result, ScratchType.Boolean);
        return result;
    }
}