using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Reflection;
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
    private List<ScratchFunction> _functions = new();

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
        DefineFunction(procedure);
        return procedure.ToString();
    }

    public override object VisitReturnStatement(ScratchScriptParser.ReturnStatementContext context)
    {
        var expression = Visit(context.expression());
        _procedures.Last().ReturnType = GetType(expression);
        return
            $"push __FunctionReturnValues {expression}\nraw control_stop f:STOP_OPTION:\"this script\"\n";
    }

    public override object VisitProcedureCallStatement(ScratchScriptParser.ProcedureCallStatementContext context)
    {
        var name = context.Identifier().GetText();
        var function = GetFunction(name);
        if (function == null)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureNotDefined, context, context.Identifier().Symbol,
                name);
            return null;
        }

        if (function.Arguments.Count != context.procedureArgument().Length)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureArgumentCountDifferent, context,
                ParserRuleContext.EmptyContext, name, function.Arguments.Count, context.procedureArgument().Length);
            return null;
        }

        var contexts = context.procedureArgument();
        switch (function)
        {
            case DefinedScratchFunction:
            {
                var result = $"call {name} ";
                for (var index = 0; index < contexts.Length; index++)
                {
                    var argument = contexts[index];
                    var argumentName = argument.Identifier() == null
                        ? function.Arguments.ElementAt(index).Name
                        : argument.Identifier().GetText();

                    var expression = Visit(argument.expression());
                    var argumentType = function.Arguments.First(x => x.Name == argumentName).Type;
                    if (GetType(argumentType) != ScratchType.Unknown)
                        AssertType(context, expression, argumentType, argument.expression());

                    result += $"i:{argumentName}:{expression} ";
                }

                result += "\n";
                if (context.Parent is ScratchScriptParser
                        .ProcedureCallExpressionContext) // Avoid repeating the same line in a statement (instead of an expression)
                    _currentScope.Prepend += result;
                _currentScope.Append += PopFunctionStackCommand;
                return result;
            }
            case NativeScratchFunction nativeFunction:
            {
                var arguments = new object[nativeFunction.Arguments.Count];
                for (var index = 0; index < contexts.Length; index++)
                {
                    var argument = contexts[index];
                    var argumentName = argument.Identifier() == null
                        ? function.Arguments.ElementAt(index).Name
                        : argument.Identifier().GetText();

                    var expression = Visit(argument.expression());
                    var argumentIndex = nativeFunction.Arguments.FindIndex(x => x.Name == argumentName);
                    var argumentType = function.Arguments.First(x => x.Name == argumentName).Type;

                    if (!function.Arguments[argumentIndex].AllowedValues.Contains(expression))
                    {
                        //TODO: error
                    }
                    
                    if (GetType(argumentType) != ScratchType.Unknown)
                        AssertType(context, expression, argumentType, argument.expression());
                    
                    arguments[argumentIndex] = expression.ToString();
                }

                var result = nativeFunction.NativeMethod.Invoke(null, arguments) as string;
                return $"{result}\n";
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override object VisitProcedureCallExpression(ScratchScriptParser.ProcedureCallExpressionContext context)
    {
        var statement = Visit(context.procedureCallStatement());
        var name = context.procedureCallStatement().Identifier().GetText();
        var function = GetFunction(name);
        Assert<string>(context, statement, context.procedureCallStatement());
        if (function.BlockInformation.ReturnType == ScratchType.Unknown)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureExpressionDoesNotReturn, context,
                ParserRuleContext.EmptyContext, name);
            return null;
        }

        var isNative = function is NativeScratchFunction;
        var result = isNative ? statement.ToString(): $"__FunctionReturnValues#{_currentScope.ProcedureIndex + 1}";
        if(isNative) _currentScope.ProcedureIndex++;
        SaveType(result, function.BlockInformation.ReturnType);
        return result;
    }

    private void DefineFunction(ScratchIrProcedure procedure)
    {
        var function = new DefinedScratchFunction
        {
            BlockInformation = new ScratchBlockAttribute(string.IsNullOrEmpty(Namespace) ? "global" : Namespace,
                procedure.Name, false, true, ScratchType.Unknown, procedure.ReturnType),
            Arguments = procedure.Arguments.Select(arg => new ScratchArgumentAttribute(arg.Key, arg.Value))
                .ToList()
        };
        _functions.Add(function);
    }

    private ScratchFunction GetFunction(string name) => _functions.FirstOrDefault(x => x.BlockInformation.Name == name);
}