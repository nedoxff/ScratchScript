using Antlr4.Runtime;
using ScratchScript.Core.Blocks;
using ScratchScript.Extensions;
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
        var isBoolean = GetType(first) == ScratchType.Boolean && GetType(second) == ScratchType.Boolean;

        if (!isString && !isBoolean)
        {
            AssertType(context, first, ScratchType.Number, context.expression(0));
            AssertType(context, second, ScratchType.Number, context.expression(1));
        }

        var result = isString || isBoolean ? $"{op} {first.Format()} {second.Format()}": GetEquationExpression(op, first, second);
        SaveType(result, ScratchType.Boolean);
        return result;
    }

    private string GetEquationExpression(string op, object first, object second) =>
        _useFloatEquation ? $"< {Operators.Abs($"(- {first.Format()} {second.Format()})")} {_floatingPointPrecision.Format()}": $"{op} {first.Format()} {second.Format()}"; 

    public override object VisitBinaryMultiplyExpression(ScratchScriptParser.BinaryMultiplyExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.multiplyOperators().GetText();
        
        AssertType(context, first, ScratchType.Number, context.expression(0));
        AssertType(context, second, ScratchType.Number, context.expression(1));

        string result;
        if (op == "**")
        {
            _currentScope.Prepend += $"call __Exponent i:base:{first} i:exponent:{second}\n";
            _currentScope.Append += PopFunctionStackCommand;
            _currentScope.ProcedureIndex++;
            result = $"__FunctionReturnValues#{_currentScope.ProcedureIndex}";
        }
        else result = $"{op} {first.Format()} {second.Format()}";
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

        var result = $"{op} {first.Format()} {second.Format()}";
        SaveType(result, isString ? ScratchType.String: ScratchType.Number);
        return result;
    }

    public override object VisitBinaryBitwiseShiftExpression(ScratchScriptParser.BinaryBitwiseShiftExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        AssertType(context, first, ScratchType.Number, context.expression(0));
        AssertType(context, second, ScratchType.Number, context.expression(1));

        _currentScope.Prepend += $"call __{(context.shiftOperators().GetText() == "<<" ? "L": "R")}Shift i:n:{first} i:shift:{second}\n";
        _currentScope.Append += PopFunctionStackCommand;
        _currentScope.ProcedureIndex++;
        var result = $"__FunctionReturnValues#{_currentScope.ProcedureIndex}";
        SaveType(result, ScratchType.Number);
        return result;
    }

    private string VisitGenericBitwiseExpression(ParserRuleContext context, ScratchScriptParser.ExpressionContext firstExpression, ScratchScriptParser.ExpressionContext secondExpression, string name)
    {
        var first = Visit(firstExpression);
        var second = Visit(secondExpression);
        AssertType(context, first, ScratchType.Number, firstExpression);
        AssertType(context, second, ScratchType.Number, secondExpression);

        _currentScope.Prepend += $"call __Bitwise{name} i:x:{first} i:y:{second}\n";
        _currentScope.Append += PopFunctionStackCommand;
        _currentScope.ProcedureIndex++;
        var result = $"__FunctionReturnValues#{_currentScope.ProcedureIndex}";
        SaveType(result, ScratchType.Number);
        return result;
    }

    public override object
        VisitBinaryBitwiseAndExpression(ScratchScriptParser.BinaryBitwiseAndExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "And");

    public override object VisitBinaryBitwiseOrExpression(ScratchScriptParser.BinaryBitwiseOrExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "Or");

    public override object VisitBinaryBitwiseXorExpression(ScratchScriptParser.BinaryBitwiseXorExpressionContext context) =>
        VisitGenericBitwiseExpression(context, context.expression(0), context.expression(1), "Xor");

    public override object VisitBinaryBooleanExpression(ScratchScriptParser.BinaryBooleanExpressionContext context)
    {
        var first = Visit(context.expression(0));
        var second = Visit(context.expression(1));
        var op = context.booleanOperators().GetText();
        
        AssertType(context, first, ScratchType.Boolean, context.expression(0));
        AssertType(context, second, ScratchType.Boolean, context.expression(1));
        
        var result = $"{op} {first.Format()} {second.Format()}";
        SaveType(result, ScratchType.Boolean);
        return result;
    }
}