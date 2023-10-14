using System.Diagnostics;
using Antlr4.Runtime.Tree.Xpath;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override TypedValue? VisitIfStatement(ScratchScriptParser.IfStatementContext context)
    {
        var condition = Visit(context.expression());
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        condition = new($"== {condition} \"true\"", ScratchType.Boolean);

        var scope = CreateScope(context.block().line(), "if " + condition.Format());
        var result = scope.ToString();

        if (context.elseIfStatement() == null) return new(result);
        var elseOutput = Visit(context.elseIfStatement());
        if (AssertNotNull(context, elseOutput, context.elseIfStatement())) return null;
        var elseOutputStr = (string)elseOutput.Value.Value;
        result += $"else\n{elseOutputStr}";

        return new(result);
    }

    public override TypedValue? VisitRepeatStatement(ScratchScriptParser.RepeatStatementContext context)
    {
        var condition = Visit(context.expression());
        if (AssertType(context, condition, ScratchType.Number, context.expression())) return null;
        
        var hasBreaks = XPath.FindAll(context, "//breakStatement", _parser).Any();

        if (hasBreaks)
        {
            var name = NameHelper.New("__Repeat");
            var procedure = new ScratchIrProcedure(name, Array.Empty<string>());
            Procedures.Add(procedure);
        }

        var code = CreateScope(context.block().line(), $"repeat {condition}");

        if (hasBreaks)
        {
            var procedure = Procedures.Last();
            procedure.Code = code + "end\n";
            _proceduresSection += $"{procedure}\n";

            return new($"call {procedure.Name}\n");
        }

        return new(code.ToString());
    }

    public override TypedValue? VisitWhileStatement(ScratchScriptParser.WhileStatementContext context)
    {
        var condition = Visit(context.expression());
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        condition = new($"== {condition} \"true\"", ScratchType.Boolean);

        var hasBreaks = XPath.FindAll(context, "//breakStatement", _parser).Any();
        
        if (hasBreaks)
        {
            var name = NameHelper.New("__While");
            var procedure = new ScratchIrProcedure(name, Array.Empty<string>());
            Procedures.Add(procedure);
        }

        var code = CreateScope(context.block().line(), $"while {condition}");
        
        code.Content.Add(Scope.Append); // Used for function return values
        code.Content.Add(Scope.Prepend);

        if (hasBreaks)
        {
            var procedure = Procedures.Last();
            procedure.Code = code + "end\n";
            _proceduresSection += $"{procedure}\n";

            return new($"call {procedure.Name}\n");
        }

        return new(code.ToString());
    }

    public override TypedValue? VisitForStatement(ScratchScriptParser.ForStatementContext context)
    {
        var initialize = context.statement(0) != null ? Visit(context.statement(0)): null;
        var condition = context.expression() != null ? Visit(context.expression()): null;
        var change = context.statement(1) != null ? Visit(context.statement(1)): null;

        condition ??= new("true", ScratchType.Boolean);
        if (AssertType(context, condition, ScratchType.Boolean, context.expression())) return null;
        condition = new($"== {condition} \"true\"", ScratchType.Boolean);
        
        var code = CreateScope(context.block().line(), $"{(initialize == null ? "": $"{initialize}\n")}while {condition}");
        code.Content.Add(Scope.Append);
        code.Content.Add(Scope.Prepend);
        code.Content.Add(change.Format());

        return new(code.ToString());
    }

    public override TypedValue? VisitSwitchStatement(ScratchScriptParser.SwitchStatementContext context)
    {
        var condition = Visit(context.expression());
        var caseCount = context.switchBlock().@case().Length;
        ScopeInfo defaultScope = null;
        if (caseCount == 0)
        {
            DiagnosticReporter.Warning(ScratchScriptWarning.SwitchStatementEmpty, context, context.switchBlock());
            return new("");
        }
        
        var cases = new List<(ScopeInfo, object)>();
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
            result += $"{(i == 0 ? "": "else")} if {GetNumberEquationExpression("==", condition, cases[i].Item2)}\n{cases[i].Item1}\n";
        if (defaultScope != null)
            result += $"else\n{defaultScope}";

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

    public override TypedValue? VisitBreakStatement(ScratchScriptParser.BreakStatementContext context)
    {
        return new($"{PopAllProcedureCache}\nraw control_stop f:STOP_OPTION:\"this script\"\n"); //TODO: change this to stop() when built-in functions are implemented
    }
}