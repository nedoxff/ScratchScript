using System.Diagnostics;
using Antlr4.Runtime.Tree.Xpath;
using ScratchScript.Core.Frontend.Scope;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public override object VisitIfStatement(ScratchScriptParser.IfStatementContext context)
    {
        var condition = Visit(context.expression());
        AssertType(context, condition, ScratchType.Boolean, context.expression());

        var scope = CreateScope(context.block().line(), "if " + condition.Format());
        var result = scope.ToString();

        if (context.elseIfStatement() == null) return result;
        var elseOutput = Visit(context.elseIfStatement());
        Assert<string>(context, elseOutput, context.elseIfStatement());
        var elseOutputStr = (string)elseOutput;
        result += $"else\n{elseOutputStr}";

        return result;
    }

    public override object VisitRepeatStatement(ScratchScriptParser.RepeatStatementContext context)
    {
        var condition = Visit(context.expression());
        AssertType(context, condition, ScratchType.Number, context.expression());
        
        var hasBreaks = XPath.FindAll(context, "//breakStatement", _parser).Any();

        if (hasBreaks)
        {
            var name = NameHelper.New("__Repeat");
            var procedure = new ScratchIrProcedure(name, Array.Empty<string>());
            _procedures.Add(procedure);
        }

        var code = CreateScope(context.block().line(), $"repeat {condition}");

        if (hasBreaks)
        {
            var procedure = _procedures.Last();
            procedure.Code = code + "end\n";
            _proceduresSection += $"{procedure}\n";

            return $"call {procedure.Name}\n";
        }

        return code.ToString();
    }

    public override object VisitWhileStatement(ScratchScriptParser.WhileStatementContext context)
    {
        var condition = Visit(context.expression());
        AssertType(context, condition, ScratchType.Boolean, context.expression());

        var hasBreaks = XPath.FindAll(context, "//breakStatement", _parser).Any();
        
        if (hasBreaks)
        {
            var name = NameHelper.New("__While");
            var procedure = new ScratchIrProcedure(name, Array.Empty<string>());
            _procedures.Add(procedure);
        }

        var code = CreateScope(context.block().line(), $"while {condition}");
        
        code.Content.Add(_currentScope.Append); // Used for function return values
        code.Content.Add(_currentScope.Prepend);

        if (hasBreaks)
        {
            var procedure = _procedures.Last();
            procedure.Code = code + "end\n";
            _proceduresSection += $"{procedure}\n";

            return $"call {procedure.Name}\n";
        }

        return code.ToString();
    }

    public override object VisitSwitchStatement(ScratchScriptParser.SwitchStatementContext context)
    {
        var condition = Visit(context.expression());
        var caseCount = context.switchBlock().@case().Length;
        ScopeInfo defaultScope = null;
        if (caseCount == 0)
        {
            //TODO: warning about the switch block being empty
            return "";
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

        return result;
    }

    public override object VisitElseIfStatement(ScratchScriptParser.ElseIfStatementContext context)
    {
        if (context.block() != null)
            return VisitBlock(context.block());
        if (context.ifStatement() != null)
            return VisitIfStatement(context.ifStatement());
        return null;
    }

    public override object VisitBreakStatement(ScratchScriptParser.BreakStatementContext context)
    {
        return
            $"{PopAllProcedureCache}\nraw control_stop f:STOP_OPTION:\"this script\"\n"; //TODO: change this to stop() when built-in functions are implemented
    }
}