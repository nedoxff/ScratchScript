using System.Net.Mime;
using System.Security.Principal;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override TypedValue? VisitAssignmentStatement(ScratchScriptParser.AssignmentStatementContext context)
    {
        var name = context.Identifier().GetText();

        if (!Scope.IdentifierUsed(name))
        {
            DiagnosticReporter.Error(ScratchScriptError.VariableNotDefined, context, context.Identifier().Symbol, name);
            return null;
        }

        if (Procedures.LastOrDefault()?.Arguments.ContainsKey(name) ?? false)
            return HandleProcedureArgumentAssignment(context);

        var op = Visit(context.assignmentOperators());
        //Assert<string>(context, op);
        var variableType = Scope.GetVariable(name).Type;

        var opString = (string)op.Value.Value;
        if (variableType == ScratchType.String && opString == "+") opString = "~";

        var expression = Visit(context.expression());

        if (expression.Value.Type is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }

        AssertType(context, variableType, expression.Value.Type, context.expression());

        return new(
            $"set var:{name} {opString} {(string.IsNullOrEmpty(opString) ? "" : $"var:{name}")} {expression.Format(rawColor: false)}\n");
    }

    private TypedValue HandleProcedureArgumentAssignment(ScratchScriptParser.AssignmentStatementContext context)
    {
        var name = context.Identifier().GetText();
        var variable = VisitIdentifierInternal(name);
        var op = Visit(context.assignmentOperators());
        var opString = op.Format();
        var expression = Visit(context.expression());

        var procedure = Procedures.Last();
        var index = procedure.Arguments.Keys.ToList().FindIndex(s => s == name);

        AssertType(context, variable, expression, context.expression());

        var shift = procedure.Arguments.Count - (index + 1);
        var ir =
            $"raw data_replaceitemoflist f:LIST:\"{StackName}\" i:INDEX:{(shift == 0 ? ":si:": $"(- :si: {shift})")} i:ITEM:({opString} {(string.IsNullOrEmpty(opString) ? "" : variable.Format())} {expression.Format(rawColor: false)})\n";
        return new(ir);
    }

    public override TypedValue? VisitVariableDeclarationStatement(
        ScratchScriptParser.VariableDeclarationStatementContext context)
    {
        var name = context.Identifier().GetText();
        var expression = Visit(context.expression());


        if (Scope.IdentifierUsed(name))
            return new($"set var:{name} {expression.Format(rawColor: false)}\n"); //TODO: warning
        
        if (expression.Value.Type is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }

        Scope.Variables.Add(new ScratchVariable(name, expression.Value.Type));
        _loadSection += $"load:{TypeHelper.ScratchTypeToString(expression.Value.Type)} {name}\n";
        return new($"set var:{name} {expression.Format(rawColor: false)}\n");
    }

    public override TypedValue? VisitAssignmentOperators(ScratchScriptParser.AssignmentOperatorsContext context)
    {
        if (context.Assignment() != null) return new("");
        if (context.AdditionAsignment() != null) return new("+");
        if (context.SubtractionAssignment() != null) return new("-");
        if (context.MultiplicationAssignment() != null) return new("*");
        if (context.DivisionAssignment() != null) return new("/");
        if (context.ModulusAssignment() != null) return new("%");
        //TODO: add **
        return null;
    }

    public override TypedValue? VisitArrayAccessExpression(ScratchScriptParser.ArrayAccessExpressionContext context)
    {
        var obj = Visit(context.expression(0));
        var index = Visit(context.expression(1));
        Assert<string>(context, obj);
        AssertType(context, index, ScratchType.Number);

        var objectString = (string)obj.Value.Value;
        if (objectString.IsList())
        {
            return new($"{obj}#{index}");
        }

        if (obj.Value.Type == ScratchType.String)
        {
            var result = $"rawshadow operator_letter_of i:LETTER:{index} i:STRING:{obj} endshadow";
            return new(result, ScratchType.String);
        }

        return null;
    }
}