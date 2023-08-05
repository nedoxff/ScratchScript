using System.Diagnostics;
using Antlr4.Runtime;
using ScratchScript.Core.Blocks;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override TypedValue? VisitBinaryCompareExpression(ScratchScriptParser.BinaryCompareExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.compareOperators().GetText();

        AssertType(context, first, second);

        var isString = op is "==" or "!=" && first.Value.Type == ScratchType.String && second.Value.Type == ScratchType.String;
        var isBoolean = first.Value.Type == ScratchType.Boolean && second.Value.Type == ScratchType.Boolean;

        if (!isString && !isBoolean)
        {
            AssertType(context, first, ScratchType.Number, context.expression(0));
            AssertType(context, second, ScratchType.Number, context.expression(1));
        }

        var result = isString || isBoolean
            ? $"{op} {first.Format()} {second.Format()}"
            : GetEquationExpression(op, first, second);
        return new(result, ScratchType.Boolean);
    }

    private string GetEquationExpression(string op, object first, object second) =>
        _useFloatEquation
            ? $"< {Operators.Abs($"(- {first.Format()} {second.Format()})")} {_floatingPointPrecision.Format()}"
            : $"{op} {first.Format()} {second.Format()}";

    public override TypedValue? VisitBinaryMultiplyExpression(
        ScratchScriptParser.BinaryMultiplyExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.multiplyOperators().GetText();

        AssertType(context, first, ScratchType.Number, context.expression(0));
        AssertType(context, second, ScratchType.Number, context.expression(1));

        if (op == "**")
            return Scope.CallFunction("__Exponent", new object[] { first, second }, ScratchType.Number);

        var result = $"{op} {first.Format()} {second.Format()}";
        return new(result, ScratchType.Number);
    }

    public override TypedValue? VisitBinaryAddExpression(ScratchScriptParser.BinaryAddExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.addOperators().GetText();

        var isString = op == "+" && first.Value.Type == ScratchType.String && second.Value.Type == ScratchType.String;

        if (!isString)
        {
            AssertType(context, first, ScratchType.Number, context.expression(0));
            AssertType(context, second, ScratchType.Number, context.expression(1));
        }
        else op = "~";

        var result = $"{op} {first.Format()} {second.Format()}";
        return new(result, isString ? ScratchType.String : ScratchType.Number);
    }

    public override TypedValue? VisitBinaryBitwiseShiftExpression(
        ScratchScriptParser.BinaryBitwiseShiftExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        AssertType(context, first, ScratchType.Number, context.expression(0));
        AssertType(context, second, ScratchType.Number, context.expression(1));

        return Scope.CallFunction($"__{(context.shiftOperators().GetText() == "<<" ? "L" : "R")}Shift",
            new object[] { first, second }, ScratchType.Number);
    }

    private TypedValue? VisitGenericBitwiseExpression(ParserRuleContext context,
        ScratchScriptParser.ExpressionContext firstExpression, ScratchScriptParser.ExpressionContext secondExpression,
        string name)
    {
        var first = Visit(firstExpression);
        var second = Visit(secondExpression);
        AssertType(context, first, ScratchType.Number, firstExpression);
        AssertType(context, second, ScratchType.Number, secondExpression);

        return Scope.CallFunction($"__Bitwise{name}", new object[] { first, second }, ScratchType.Number);
    }

    public override TypedValue?
        VisitBinaryBitwiseAndExpression(ScratchScriptParser.BinaryBitwiseAndExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "And");

    public override TypedValue? VisitBinaryBitwiseOrExpression(
        ScratchScriptParser.BinaryBitwiseOrExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "Or");

    public override TypedValue? VisitBinaryBitwiseXorExpression(
        ScratchScriptParser.BinaryBitwiseXorExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "Xor");

    public override TypedValue? VisitBinaryBooleanExpression(ScratchScriptParser.BinaryBooleanExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.booleanOperators().GetText();

        AssertType(context, first, ScratchType.Boolean, context.expression(0));
        AssertType(context, second, ScratchType.Boolean, context.expression(1));

        var result = $"{op} {first.Format()} {second.Format()}";
        return new(result, ScratchType.Boolean);
    }
}