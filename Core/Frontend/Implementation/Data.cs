using System.Net.Mime;
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

        return $"set v:{name} {op} {(string.IsNullOrEmpty(op as string) ? "": $"v:{name}")} {expression.Format(rawColor: false)}\n";
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
        return $"set v:{name} {expression.Format(rawColor: false)}\n";
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
}