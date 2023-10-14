using System.Net.Mime;
using System.Security.Principal;
using Antlr4.Runtime;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;
using Serilog;

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
        if (AssertType(context, variableType, expression.Value.Type, context.expression())) return null;

        if (opString == "**")
        {
            var call = Scope.CallFunction("__Exponent", new object[] { Scope.GetVariable(name), expression },
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
        var expression = Visit(context.expression());

        var procedure = Procedures.Last();
        var index = procedure.Arguments.Keys.ToList().FindIndex(s => s == name);

        var opString = op.Format();
        if (variable.Value.Type == ScratchType.String && opString == "+") opString = "~";

        if (AssertType(context, variable, expression, context.expression())) return null;

        var shift = procedure.Arguments.Count - (index + 1);
        var stackIndex = shift == 0 ? ":si:" : $"(- :si: {shift})";
        var newItem =
            $"({opString} {(string.IsNullOrEmpty(opString) ? "" : variable.Format())} {expression.Format(rawColor: false)})";
        if (opString == "**")
            newItem = Scope.CallFunction("__Exponent", new object[] { variable, expression }, ScratchType.Number)
                .Format();

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

        /*if (expression.Value.Type is ScratchType.Color or ScratchType.Variable or ScratchType.Unknown)
        {
            //TODO: ERROR
        }*/

        var variable = new ScratchVariable(name, expression.Value.Type);
        Scope.Variables.Add(variable);
        Scope.Content.Add($"load:{GetIRType(expression.Value.Type)} {variable.Id}\n");

        if (expression.Value.Type.Kind == ScratchTypeKind.List)
        {
            var push = HandleListAssignment(context, expression.Value, ref variable);
            Scope.Variables[Scope.Variables.FindIndex(x => x.Id == variable.Id)] = variable;
            return push;
        }

        return new($"set var:{Scope.GetVariable(name).Id} {expression.Format(rawColor: false)}\n");
    }

    private string GetIRType(ScratchType type)
    {
        if (type.Kind == ScratchTypeKind.Boolean) return "string";
        return type.Name;
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

    public override TypedValue? VisitPostIncrementStatement(ScratchScriptParser.PostIncrementStatementContext context)
    {
        var identifier = VisitIdentifierInternal(context.Identifier().GetText());
        if (AssertNotNull(context, identifier, context.Identifier().Symbol)) return null;
        if (AssertVariable(context, identifier, context.Identifier().Symbol)) return null;
        var op = context.postIncrementOperators().GetText()[0];
        return new($"set {identifier} {op} {identifier} 1");
    }

    private void EnableUnicode()
    {
        var global = GlobalScope;
        global.Content.Insert(0, "flag UNICODE");

        if (StdLoader.Functions.TryGetValue("std/string/unicode", out var functions))
        {
            _imports.Add("std/string/unicode");
            Functions.AddRange(functions);
            _proceduresSection += functions.Select(x => x.Code)
                .Aggregate("", (current, next) => current + "\n" + next);
        }

        Log.Information("Unicode (enhanced string handling) mode enabled");
    }
}