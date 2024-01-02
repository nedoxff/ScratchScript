using System.Diagnostics;
using Antlr4.Runtime;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private TypedValue HydrateValue(TypedValue original, params TypedValue?[] additional)
    {
        foreach (var value in additional.Reverse())
        {
            original.Before = original.Before.Insert(0, value?.Before ?? "");
            original.After = original.After.Insert(0, value?.After ?? "");
        }

        return original;
    }

    public override TypedValue? VisitBinaryCompareExpression(ScratchScriptParser.BinaryCompareExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.compareOperators().GetText();

        if (AssertNotNull(context, first, context.expression(0))) return null;
        if (AssertNotNull(context, second, context.expression(1))) return null;
        if (AssertType(context, first, second, context.expression(1))) return null;

        var isString = op is "==" or "!=" && first.Value.Type == ScratchType.String &&
                       second.Value.Type == ScratchType.String;
        var isBoolean = first.Value.Type == ScratchType.Boolean && second.Value.Type == ScratchType.Boolean;

        if (!isString && !isBoolean)
        {
            if (AssertType(context, first, ScratchType.Number, context.expression(0))) return null;
            if (AssertType(context, second, ScratchType.Number, context.expression(1))) return null;
        }

        var result = new TypedValue($"{op} {first.Format()} {second.Format()}", ScratchType.Boolean);
        if (isString) result = GetStringEquationExpression(context, op, first, second);
        if (!isString && !isBoolean) result = GetNumberEquationExpression(op, first, second);
        return HydrateValue(result, first, second);
    }

    private TypedValue GetStringEquationExpression(ParserRuleContext context, string op, TypedValue? first, TypedValue? second)
    {
        if (_useUnicode)
        {
            RequireFunction("__UnicodeCompare", context);
            return Scope.CallFunction("__UnicodeCompare", new[] { first, second as object }, ScratchType.Boolean);
        }

        return new TypedValue($"{op} {first.Format()} {second.Format()}", ScratchType.Boolean);
    }

    private TypedValue GetNumberEquationExpression(string op, TypedValue? first, TypedValue? second) =>
       new(_useFloatEquation && op is "==" or "!="
            ? $"{(op is "==" ? "<" : ">")} {Operators.Abs($"(- {first.Format()} {second.Format()})")} {_floatingPointPrecision.Format()}"
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
        {
            RequireFunction("__Exponent", context);
            return Scope.CallFunction("__Exponent", new object[] { first, second }, ScratchType.Number);
        }

        if (op == "/" && second?.Value is decimal and 0)
            DiagnosticReporter.Warning(ScratchScriptWarning.DivisionByZero, context, context.expression(1));

        var result = $"{op} {first.Format()} {second.Format()}";
        return HydrateValue(new(result, ScratchType.Number), first, second);
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
        return HydrateValue(new(result, isString ? ScratchType.String : ScratchType.Number), first, second);
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

        var function = $"__{(context.shiftOperators().GetText() == "<<" ? "L" : "R")}Shift";
        RequireFunction(function, context);
        return HydrateValue(Scope.CallFunction(function,
            new object[] { first, second }, ScratchType.Number), first, second);
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

        RequireFunction($"__Bitwise{name}", context);
        return HydrateValue(Scope.CallFunction($"__Bitwise{name}", new object[] { first, second }, ScratchType.Number), first, second);
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
        return HydrateValue(new(result, ScratchType.Boolean), first, second);
    }
}