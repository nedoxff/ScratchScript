using System.Net.Mime;
using System.Security.Principal;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private Dictionary<string, ScratchType> _variables = new();

    public override object VisitAssignmentStatement(ScratchScriptParser.AssignmentStatementContext context)
    {
        var name = context.Identifier().GetText();

        if (!_currentScope.IdentifierUsed(name))
        {
            DiagnosticReporter.Error(ScratchScriptError.VariableNotDefined, context, context.Identifier().Symbol, name);
            return null;
        }

        var op = Visit(context.assignmentOperators());
        Assert<string>(context, op);
        var variableType = _currentScope.GetVariable(name).Type;

        if (variableType == ScratchType.String && (string)op == "+") op = "~";
        
        var expression = Visit(context.expression());
        var expressionType = GetType(expression);
        
        if (expressionType is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }

        AssertType(context, variableType, expressionType, context.expression());

        return $"set var:{name} {op} {(string.IsNullOrEmpty(op as string) ? "": $"var:{name}")} {expression.Format(rawColor: false)}\n";
    }

    public override object VisitVariableDeclarationStatement(
        ScratchScriptParser.VariableDeclarationStatementContext context)
    {
        var name = context.Identifier().GetText();

        if (_currentScope.IdentifierUsed(name))
        {
            DiagnosticReporter.Error(ScratchScriptError.IdentifierAlreadyUsed, context, context.Identifier().Symbol,
                name);
            return null;
        }

        var expression = Visit(context.expression());
        var expressionType = GetType(expression);

        if (expressionType is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }
        
        _currentScope.Variables.Add(new ScratchVariable(name, expressionType));
        _loadSection += $"load:{TypeHelper.ScratchTypeToString(expressionType)} {name}\n";
        return $"set var:{name} {expression.Format(rawColor: false)}\n";
    }

    public override object VisitAssignmentOperators(ScratchScriptParser.AssignmentOperatorsContext context)
    {
        if (context.Assignment() != null) return "";
        if (context.AdditionAsignment() != null) return "+";
        if (context.SubtractionAssignment() != null) return "-";
        if (context.MultiplicationAssignment() != null) return "*";
        if (context.DivisionAssignment() != null) return "/";
        if (context.ModulusAssignment() != null) return "%";
        return null;
    }

    public override object VisitArrayAccessExpression(ScratchScriptParser.ArrayAccessExpressionContext context)
    {
        var obj = Visit(context.expression(0));
        var index = Visit(context.expression(1));
        Assert<string>(context, obj);
        AssertType(context, index, ScratchType.Number);
        
        var objectString = (string)obj;
        if (objectString.StartsWith("arr:"))
        {
            return $"{obj}#{index}";
        }

        if (GetType(obj) == ScratchType.String)
        {
            var result = $"rawshadow operator_letter_of i:LETTER:{index} i:STRING:{obj} endshadow";
            SaveType(result, ScratchType.String);
            return result;
        }
        return null;
    }
}