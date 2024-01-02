using System.Diagnostics;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Tree.Xpath;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private bool CheckBreakMode(IParseTree tree, string name)
    {
        var hasBreaks = XPath.FindAll(tree, "//breakStatement", _parser).Any();
        if (hasBreaks)
        {
            name = NameHelper.New(name);
            var procedure = new ScratchIrProcedure(name, new[] { StackIndexArgumentName });
            Procedures.Add(procedure);
        }

        return hasBreaks;
    }

    private TypedValue HandleBreakMode(ref ScopeInfo code)
    {
        var procedure = Procedures.Last();
        procedure.Code = code + "end\n";
        ProceduresSection += $"{procedure}\n";
        return Scope.CallFunction(procedure.Name, Array.Empty<object>(), ScratchType.Unknown);
    }

    public override TypedValue? VisitIfStatement(ScratchScriptParser.IfStatementContext context)
    {
        var stackCapture = CurrentStackLength;
        var condition = Visit(context.expression());
        if (AssertNotNull(context, condition, context.expression())) return null;
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        if(condition!.Value.IsVariable()) condition = condition!.Value with { Value = $"== {condition} \"true\"" };

        var scope = CreateScope(context.block().line(),
            @$"
{condition?.Before}
set var:__TempValue {condition}
{condition?.After}
{GetCleanupCode(stackCapture, false)}
if == var:__TempValue ""true""");
        var result = scope.ToString();

        if (context.elseIfStatement() != null)
        {
            var elseOutput = Visit(context.elseIfStatement());
            if (AssertNotNull(context, elseOutput, context.elseIfStatement())) return null;
            var elseOutputStr = (string)elseOutput!.Value.Value;
            result += $"else {elseOutputStr}";
        }

        Scope.PendingItemsCount -= Scope.PendingItemsCount - stackCapture;
        return new(result);
    }

    public override TypedValue? VisitRepeatStatement(ScratchScriptParser.RepeatStatementContext context)
    {
        var stackCapture = CurrentStackLength;
        var hasBreaks = CheckBreakMode(context, "Repeat");

        var condition = Visit(context.expression());
        if (AssertNotNull(context, condition, context.expression())) return null;
        if (AssertType(context, condition, ScratchType.Number, context.expression())) return null;

        var code = CreateScope(context.block().line(), @$"{condition?.Before}
set var:__TempValue {condition}
{condition?.After}
{GetCleanupCode(stackCapture)}
repeat var:__TempValue");

        if (hasBreaks) return HandleBreakMode(ref code);
        return new(code.ToString());
    }

    public override TypedValue? VisitWhileStatement(ScratchScriptParser.WhileStatementContext context)
    {
        var stackCapture = CurrentStackLength;
        var hasBreaks = CheckBreakMode(context, "While");

        var condition = Visit(context.expression());
        if (AssertNotNull(context, condition, context.expression())) return null;
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        if(condition!.Value.IsVariable()) condition = condition!.Value with { Value = $"== {condition} \"true\"" };

        var conditionCleanup = $@"{condition?.After}
{GetCleanupCode(stackCapture)}";
        var code = CreateScope(context.block().line(), @$"
{condition?.Before}
while {condition}
{conditionCleanup}",
            isFunctionScope: hasBreaks);

        code.Content.Add(condition?.After);
        code.Content.Add(condition?.Before);
        code.EndingLine += conditionCleanup;

        if (hasBreaks) return HandleBreakMode(ref code);
        return new(code.ToString());
    }

    public override TypedValue? VisitForStatement(ScratchScriptParser.ForStatementContext context)
    {
        var hasBreaks = CheckBreakMode(context, "For");

        var initializeStackCapture = CurrentStackLength;
        var initialize = context.statement(0) != null ? Visit(context.statement(0)) : null;
        var initializeString =
            initialize != null ? $"{initialize.Value.Before}\n{initialize}\n{initialize.Value.After}" : "";

        var conditionStackCapture = CurrentStackLength;
        var condition = context.expression() != null ? Visit(context.expression()) : new("true", ScratchType.Boolean);
        if (AssertNotNull(context, condition, context.expression())) return null;
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        if(condition!.Value.IsVariable()) condition = condition!.Value with { Value = $"== {condition} \"true\"" };

        var conditionCleanup = $@"{condition?.After}
{GetCleanupCode(conditionStackCapture)}";
        var code = CreateScope(context.block().line(), @$"{initializeString}
{condition?.Before}
while {condition}
{conditionCleanup}");

        var changeStackCapture = CurrentStackLength;
        var change = context.statement(1) != null ? Visit(context.statement(1)) : null;
        code.Content.Add($"{change}\n{GetCleanupCode(changeStackCapture)}");
        code.Content.Add(condition?.Before);
        code.EndingLine += conditionCleanup;

        Scope.PendingItemsCount -= Scope.PendingItemsCount - initializeStackCapture;
        if (hasBreaks) return HandleBreakMode(ref code);
        return new(code.ToString());
    }

    public override TypedValue? VisitSwitchStatement(ScratchScriptParser.SwitchStatementContext context)
    {
        var stackCapture = CurrentStackLength;
        var condition = Visit(context.expression());
        var caseCount = context.switchBlock().@case().Length;
        ScopeInfo defaultScope = null;
        if (caseCount == 0)
        {
            DiagnosticReporter.Warning(ScratchScriptWarning.SwitchStatementEmpty, context, context.switchBlock());
            return new("");
        }

        var cases = new List<(ScopeInfo, TypedValue?)>();
        foreach (var caseContext in context.switchBlock().@case())
        {
            if (caseContext.defaultCase() == null)
            {
                var scope = CreateScope(caseContext.block().line());
                var expression = Visit(caseContext.constant());
                if (AssertType(context, expression, condition, context.expression())) return null;
                cases.Add((scope, expression));
            }
            else
                defaultScope = CreateScope(caseContext.defaultCase().block().line());
        }

        var result = "";
        //TODO: should number equation be used here (or a string one? can we do strings in switch? probably)
        for (var i = 0; i < cases.Count; i++)
            result +=
                $"{(i == 0 ? "" : "else")} if {GetNumberEquationExpression("==", condition, cases[i].Item2)}\n{GetCleanupCode(stackCapture, false)}\n{cases[i].Item1}\n";
        if (defaultScope != null)
            result += $"else\n{GetCleanupCode(stackCapture, false)}\n{defaultScope}";

        Scope.PendingItemsCount -= Scope.PendingItemsCount - stackCapture;

        return new(result);
    }

    public override TypedValue? VisitElseIfStatement(ScratchScriptParser.ElseIfStatementContext context)
    {
        if (context.block() != null)
            return VisitBlock(context.block());
        if (context.ifStatement() != null)
            return VisitIfStatement(context.ifStatement());
        return null;
    }

    public override TypedValue? VisitBreakStatement(ScratchScriptParser.BreakStatementContext context) =>
        new("\nraw control_stop f:STOP_OPTION:\"this script\"\n");
}