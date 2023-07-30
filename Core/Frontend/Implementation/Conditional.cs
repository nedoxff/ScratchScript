using System.Diagnostics;
using Antlr4.Runtime.Tree.Xpath;
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
        AssertType(context, condition, ScratchType.Boolean, context.expression());

        var scope = CreateScope(context.block().line(), "if " + condition.Format());
        var result = scope.ToString();

        if (context.elseIfStatement() == null) return new(result);
        var elseOutput = Visit(context.elseIfStatement());
        //Assert<string>(context, elseOutput, context.elseIfStatement());
        var elseOutputStr = (string)elseOutput.Value.Value;
        result += $"else\n{elseOutputStr}";

        return new(result);
    }

    public override TypedValue? VisitRepeatStatement(ScratchScriptParser.RepeatStatementContext context)
    {
        var condition = Visit(context.expression());
        AssertType(context, condition, ScratchType.Number, context.expression());
        
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
        AssertType(context, condition, ScratchType.Boolean, context.expression());

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

    public override TypedValue? VisitSwitchStatement(ScratchScriptParser.SwitchStatementContext context)
    {
        var condition = Visit(context.expression());
        var caseCount = context.switchBlock().@case().Length;
        ScopeInfo defaultScope = null;
        if (caseCount == 0)
        {
            //TODO: warning about the switch block being empty
            return new("");
        }
        
        var cases = new List<(ScopeInfo, object)>();
        foreach (var caseContext in context.switchBlock().@case())
        {
            if (caseContext.defaultCase() == null)
            {
                var scope = CreateScope(caseContext.block().line());
                var expression = Visit(caseContext.constant());
                AssertType(context, expression, condition);
                cases.Add((scope, expression));
            }
            else
                defaultScope = CreateScope(caseContext.defaultCase().block().line());
        }

        var result = "";
        for (var i = 0; i < cases.Count; i++)
            result += $"{(i == 0 ? "": "else")} if {GetEquationExpression("==", condition, cases[i].Item2)}\n{cases[i].Item1}\n";
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