using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Helpers;
using Spectre.Console;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private const string PopFunctionStackCommand = "popat __FunctionReturnValues 1\n";

    private string PopAllProcedureCache =>
        string.Concat(Enumerable.Repeat(PopFunctionStackCommand, _currentScope.ProcedureIndex));

    private class ScratchIrProcedure
    {
        public string Name;
        public Dictionary<string, ScratchType> Arguments = new();
        public ScratchType ReturnType = ScratchType.Unknown;
        public string Code;

        public ScratchIrProcedure(string name, IEnumerable<string> arguments)
        {
            Name = name;
            foreach (var argument in arguments)
                Arguments[argument] = ScratchType.Unknown;
        }

        public override string ToString()
        {
            var arguments = Arguments.Aggregate("",
                (current, pair) => current + $"{pair.Key}:{(pair.Value == ScratchType.Boolean ? "b" : "sn")} ");
            return $"\nproc {Name} {arguments}\n{Code}\n";
        }
    }

    private ScratchIrProcedure InitProcedure = new("__Init", Array.Empty<string>());
    private List<ScratchIrProcedure> _procedures = new();

    public override object VisitProcedureDeclarationStatement(
        ScratchScriptParser.ProcedureDeclarationStatementContext context)
    {
        var name = context.Identifier(0).GetText();
        if (_currentScope.IdentifierUsed(name))
        {
            DiagnosticReporter.Error(ScratchScriptError.IdentifierAlreadyUsed, context, context.Identifier(0).Symbol,
                name);
            return null;
        }

        var argumentNames = context.Identifier().Skip(1).Select(x => x.GetText()).ToList();
        for (var i = 0; i < argumentNames.Count; i++)
        {
            if (!_currentScope.IdentifierUsed(argumentNames[i])) continue;
            DiagnosticReporter.Error(ScratchScriptError.IdentifierAlreadyUsed, context,
                context.Identifier(i + 1).Symbol, argumentNames[i]);
            return null;
        }

        var procedure = new ScratchIrProcedure(name, argumentNames);
        _procedures.Add(procedure);

        var scope = CreateScope(context.block().line(), reporters: argumentNames);
        procedure.Code = scope.ToString();
        return procedure.ToString();
    }

    public override object VisitReturnStatement(ScratchScriptParser.ReturnStatementContext context)
    {
        var expression = Visit(context.expression());
        _procedures.Last().ReturnType = GetType(expression);
        return
            $"push __FunctionReturnValues {expression}\n{PopAllProcedureCache}\nraw control_stop f:STOP_OPTION:\"this script\"\n";
    }

    public override object VisitProcedureCallStatement(ScratchScriptParser.ProcedureCallStatementContext context)
    {
        var name = context.Identifier().GetText();
        if (_procedures.All(x => x.Name != name))
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureNotDefined, context, context.Identifier().Symbol,
                name);
            return null;
        }

        var procedure = _procedures.First(x => x.Name == name);
        if (procedure.Arguments.Count != context.procedureArgument().Length)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureArgumentCountDifferent, context,
                ParserRuleContext.EmptyContext, name, procedure.Arguments.Count, context.procedureArgument().Length);
            return null;
        }

        var result = $"call {name} ";
        var contexts = context.procedureArgument();
        for (var index = 0; index < contexts.Length; index++)
        {
            var argument = contexts[index];
            var argumentName = argument.Identifier() == null
                ? procedure.Arguments.Keys.ElementAt(index)
                : argument.Identifier().GetText();

            var expression = Visit(argument.expression());
            if (GetType(procedure.Arguments[argumentName]) != ScratchType.Unknown)
                AssertType(context, expression, procedure.Arguments[argumentName], argument.expression());

            result += $"i:{argumentName}:{expression} ";
        }

        result += "\n";
        if (context.Parent is ScratchScriptParser
                .ProcedureCallExpressionContext) // Avoid repeating the same line in a statement (instead of an expression)
            _currentScope.Prepend += result;
        _currentScope.Append += PopFunctionStackCommand;
        return result;
    }

    public override object VisitProcedureCallExpression(ScratchScriptParser.ProcedureCallExpressionContext context)
    {
        var statement = Visit(context.procedureCallStatement());
        var procedure = _procedures.First(x => x.Name == context.procedureCallStatement().Identifier().GetText());
        Assert<string>(context, statement, context.procedureCallStatement());
        if (procedure.ReturnType == ScratchType.Unknown)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureExpressionDoesNotReturn, context,
                ParserRuleContext.EmptyContext, procedure.Name);
            return null;
        }

        var result = $"__FunctionReturnValues#{_currentScope.ProcedureIndex + 1}";
        _currentScope.ProcedureIndex++;
        SaveType(result, procedure.ReturnType);
        return result;
    }
}