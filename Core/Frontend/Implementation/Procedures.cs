using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;
using Spectre.Console;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    private const string PopFunctionStackCommand = "pop __FunctionReturnValues\n";

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
        foreach (var attributeStatementContext in context.attributeStatement())
            HandleProcedureAttribute(attributeStatementContext, ref procedure);
        _procedures.Add(procedure);

        var scope = CreateScope(context.block().line(), reporters: argumentNames);
        procedure.Code = scope.ToString();
        DefineFunction(procedure);
        return procedure.ToString();
    }

    public override object VisitReturnStatement(ScratchScriptParser.ReturnStatementContext context)
    {
        var expression = Visit(context.expression());
        if(_procedures.Last().ReturnType == ScratchType.Unknown)
            _procedures.Last().ReturnType = GetType(expression);
        return
            $"pushat __FunctionReturnValues 1 {expression}\n{_currentScope.Append}\nraw control_stop f:STOP_OPTION:\"this script\"\n";
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

        return HandleProcedureCall(context, function, context.procedureArgument());
    }

    private string HandleProcedureCall(ParserRuleContext context, ScratchFunction function,
        ScratchScriptParser.ProcedureArgumentContext[] procedureArguments, object caller = null)
    {
        var isMember = caller != null ? 1 : 0;
        if (function.Arguments.Count != procedureArguments.Length + isMember)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureArgumentCountDifferent, context,
                ParserRuleContext.EmptyContext, function.BlockInformation.Name, function.Arguments.Count,
                procedureArguments.Length);
            return null;
        }

        switch (function)
        {
            case DefinedScratchFunction:
            {
                var result = $"call {function.BlockInformation.Name} ";
                
                for (var index = 0; index < procedureArguments.Length; index++)
                {
                    var argument = procedureArguments[index];
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

                if (caller != null)
                    arguments[0] = caller.Format();
                
                for (var index = 0; index < procedureArguments.Length; index++)
                {
                    var argument = procedureArguments[index];
                    var argumentName = argument.Identifier() == null
                        ? function.Arguments.ElementAt(index + isMember).Name
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

                    arguments[argumentIndex] = expression.Format();
                }

                var result = nativeFunction.NativeMethod.Invoke(null, arguments);
                if (result != null)
                {
                    var resultString = result as string + "\n";
                    SaveType(resultString, nativeFunction.BlockInformation.ReturnType);
                    return resultString;
                }
                 return null;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override object VisitMemberProcedureCallExpression(
        ScratchScriptParser.MemberProcedureCallExpressionContext context)
    {
        var member = Visit(context.expression());
        var functionName = context.procedureCallStatement().Identifier().GetText();
        var function = FindMemberFunction(member, functionName);

        if (function == null)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureNotDefined, context,
                context.procedureCallStatement().Identifier().Symbol,
                functionName);
            return null;
        }

        return HandleProcedureCall(context, function, context.procedureCallStatement().procedureArgument(), member);
    }

    private ScratchFunction FindMemberFunction(object member, string functionName)
    {
        ScratchFunction function;
        if (member.IsVariable() && GetFunction(functionName, false, ScratchType.Variable) != null)
            function = GetFunction(functionName, false, ScratchType.Variable);
        else if (member.IsList() && GetFunction(functionName, false, ScratchType.List) != null)
            function = GetFunction(functionName, false, ScratchType.List);
        else
            function = GetFunction(functionName, false, GetType(member));
        return function;
    }

    public override object VisitMemberProcedureCallStatement(ScratchScriptParser.MemberProcedureCallStatementContext context)
    {
        var member = Visit(context.expression());
        var functionName = context.procedureCallStatement().Identifier().GetText();
        var function = FindMemberFunction(member, functionName);

        if (function == null)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureNotDefined, context,
                context.procedureCallStatement().Identifier().Symbol,
                functionName);
            return null;
        }

        return HandleProcedureCall(context, function, context.procedureCallStatement().procedureArgument(), member);
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
        var result = isNative ? statement.ToString() : $"__FunctionReturnValues#{_currentScope.ProcedureIndex + 1}";
        if (!isNative) _currentScope.ProcedureIndex++;
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
                .ToList(),
            Code = procedure.ToString(),
            Imported = false
        };
        _functions.Add(function);
    }

    private ScratchFunction
        GetFunction(string name, bool isStatic = true, ScratchType callerType = ScratchType.Unknown) =>
        _functions.FirstOrDefault(x =>
            x.BlockInformation.Name == name && x.BlockInformation.IsStatic == isStatic &&
            x.BlockInformation.CallerType == callerType);
}