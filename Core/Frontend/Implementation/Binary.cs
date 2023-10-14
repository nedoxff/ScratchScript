using System.Diagnostics;
using Antlr4.Runtime;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Diagnostics;
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

        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        if (AssertType(context, first, second, context.expression(1))) return null;

        var isString = op is "==" or "!=" && first.Value.Type == ScratchType.String && second.Value.Type == ScratchType.String;
        var isBoolean = first.Value.Type == ScratchType.Boolean && second.Value.Type == ScratchType.Boolean;

        if (!isString && !isBoolean)
        {
            if (AssertType(context, first, ScratchType.Number, context.expression(0))) return null;
            if (AssertType(context, second, ScratchType.Number, context.expression(1))) return null;
        }

        var result = new TypedValue($"{op} {first.Format()} {second.Format()}", ScratchType.Boolean);
        if (isString) result = GetStringEquationExpression(op, first, second);
        if (!isString && !isBoolean) result = GetNumberEquationExpression(op, first, second);
        return new(result, ScratchType.Boolean);
    }

    private TypedValue GetStringEquationExpression(string op, object first, object second) =>
        _useUnicode ? Scope.CallFunction("__UnicodeCompare", new [] {first, second}, ScratchType.Boolean) : new TypedValue($"{op} {first.Format()} {second.Format()}", ScratchType.Boolean);

    private TypedValue GetNumberEquationExpression(string op, object first, object second) =>
        new TypedValue(_useFloatEquation
            ? $"< {Operators.Abs($"(- {first.Format()} {second.Format()})")} {_floatingPointPrecision.Format()}"
            : $"{op} {first.Format()} {second.Format()}", ScratchType.Boolean);

    public override TypedValue? VisitBinaryMultiplyExpression(
        ScratchScriptParser.BinaryMultiplyExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.multiplyOperators().GetText();

        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        if (AssertType(context, first, ScratchType.Number, context.expression(0))) return null;
        if (AssertType(context, second, ScratchType.Number, context.expression(1))) return null;

        if (op == "**")
            return Scope.CallFunction("__Exponent", new object[] { first, second }, ScratchType.Number);

        if (op == "/" && second.Value.Value is decimal and 0)
            DiagnosticReporter.Warning(ScratchScriptWarning.DivisionByZero, context, context.expression(1));

        var result = $"{op} {first.Format()} {second.Format()}";
        return new(result, ScratchType.Number);
    }

    public override TypedValue? VisitBinaryAddExpression(ScratchScriptParser.BinaryAddExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        var op = context.addOperators().GetText();

        var isString = op == "+" && first.Value.Type == ScratchType.String && second.Value.Type == ScratchType.String;

        if (!isString)
        {
            if (AssertType(context, first, ScratchType.Number, context.expression(0))) return null;
            if (AssertType(context, second, ScratchType.Number, context.expression(1))) return null;
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
        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        if (AssertType(context, first, ScratchType.Number, context.expression(0))) return null;
        if (AssertType(context, second, ScratchType.Number, context.expression(1))) return null;

        return Scope.CallFunction($"__{(context.shiftOperators().GetText() == "<<" ? "L" : "R")}Shift",
            new object[] { first, second }, ScratchType.Number);
    }

    private TypedValue? VisitGenericBitwiseExpression(ParserRuleContext context,
        ScratchScriptParser.ExpressionContext firstExpression, ScratchScriptParser.ExpressionContext secondExpression,
        string name)
    {
        var first = Visit(firstExpression);
        var second = Visit(secondExpression);
        if (AssertNotNull(context, first, firstExpression)) return null;
        if (AssertNotNull(context, second, secondExpression)) return null;
        if (AssertType(context, first, ScratchType.Number, firstExpression)) return null;
        if (AssertType(context, second, ScratchType.Number, secondExpression)) return null;

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

        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        if (AssertType(context, first, ScratchType.Boolean, context.expression(0))) return null;
        if (AssertType(context, second, ScratchType.Boolean, context.expression(1))) return null;

        var result = $"{op} {first.Format()} {second.Format()}";
        return new(result, ScratchType.Boolean);
    }
}