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
        if (AssertNotNull(context, op, context.assignmentOperators())) return null;
        var variableType = Scope.GetVariable(name).Type;

        var opString = (string)op.Value.Value;
        if (variableType == ScratchType.String && opString == "+") opString = "~";

        var expression = Visit(context.expression());

        if (expression.Value.Type is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }

        if (AssertType(context, variableType, expression.Value.Type, context.expression())) return null;

        if (opString == "**")
        {
            var call =  Scope.CallFunction("__Exponent", new object[] { Scope.GetVariable(name), expression },
                ScratchType.Number);
            return new(
                $"set var:{Scope.GetVariable(name).Id} {call.Format()}\n");
        }

        return new(
            $"set var:{Scope.GetVariable(name).Id} {opString} {(string.IsNullOrEmpty(opString) ? "" : $"var:{Scope.GetVariable(name).Id}")} {expression.Format(rawColor: false)}\n");
    }

    private TypedValue? HandleProcedureArgumentAssignment(ScratchScriptParser.AssignmentStatementContext context)
    {
        var name = context.Identifier().GetText();
        var variable = VisitIdentifierInternal(name);
        var op = Visit(context.assignmentOperators());
        var opString = op.Format();
        var expression = Visit(context.expression());

        var procedure = Procedures.Last();
        var index = procedure.Arguments.Keys.ToList().FindIndex(s => s == name);

        if (AssertType(context, variable, expression, context.expression())) return null;

        var shift = procedure.Arguments.Count - (index + 1);
        var stackIndex = shift == 0 ? ":si:": $"(- :si: {shift})";
        var newItem =
            $"({opString} {(string.IsNullOrEmpty(opString) ? "" : variable.Format())} {expression.Format(rawColor: false)})";
        if (opString == "**")
            newItem = Scope.CallFunction("__Exponent", new object[] { variable, expression }, ScratchType.Number).Format();
                
        var ir =
            $"raw data_replaceitemoflist f:LIST:\"{StackName}\" i:INDEX:{stackIndex} i:ITEM:{newItem}\n";
        return new(ir);
    }

    public override TypedValue? VisitVariableDeclarationStatement(
        ScratchScriptParser.VariableDeclarationStatementContext context)
    {
        var name = context.Identifier().GetText();
        var expression = Visit(context.expression());
        
        if (Scope.IdentifierUsed(name))
            return new($"set var:{Scope.GetVariable(name).Id} {expression.Format(rawColor: false)}\n");
        
        if (expression.Value.Type is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }

        Scope.Variables.Add(new ScratchVariable(name, expression.Value.Type));
        _loadSection += $"load:{TypeHelper.ScratchTypeToString(expression.Value.Type)} {name}\n";
        return new($"set var:{Scope.GetVariable(name).Id} {expression.Format(rawColor: false)}\n");
    }

    public override TypedValue? VisitAssignmentOperators(ScratchScriptParser.AssignmentOperatorsContext context)
    {
        if (context.Assignment() != null) return new("");
        if (context.AdditionAsignment() != null) return new("+");
        if (context.SubtractionAssignment() != null) return new("-");
        if (context.MultiplicationAssignment() != null) return new("*");
        if (context.DivisionAssignment() != null) return new("/");
        if (context.ModulusAssignment() != null) return new("%");
        if (context.PowerAssignment() != null) return new("**");
        return null;
    }

    public override TypedValue? VisitArrayAccessExpression(ScratchScriptParser.ArrayAccessExpressionContext context)
    {
        var obj = Visit(context.expression(0));
        var index = Visit(context.expression(1));
        if (AssertNotNull(context, obj, context.expression(0))) return null;
        if (AssertNotNull(context, index, context.expression(1))) return null;
        if (AssertType(context, index, ScratchType.Number, context.expression(1))) return null;

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

    public override TypedValue? VisitPostIncrementStatement(ScratchScriptParser.PostIncrementStatementContext context)
    {
        var identifier = VisitIdentifierInternal(context.Identifier().GetText());
        if (AssertNotNull(context, identifier, context.Identifier().Symbol)) return null;
        if (AssertType(context, identifier, ScratchType.Variable, context.Identifier().Symbol)) return null;
        var op = context.postIncrementOperators().GetText()[0];
        return new($"set {identifier} {op} {identifier} 1");
    }
}