using Antlr4.Runtime;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;
using Spectre.Console;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor
{
    public const string FunctionStackName = "__FunctionStack";
    public const string StackName = "__Stack";
    public const string PopFunctionStackCommand = $"pop {FunctionStackName}\n";
    public const string PopStackCommand = $"pop {StackName}\n";
    public const string StackIndexArgumentName = "si";
    public const string ProcedureIndexArgumentName = "pi";

    private string PopAllProcedureCache =>
        string.Concat(Enumerable.Repeat(PopFunctionStackCommand, Scope.ProcedureIndex));


    public class ScratchIrProcedure
    {
        public string Name;
        public Dictionary<string, ScratchType> Arguments = new();
        public ScratchType ReturnType = ScratchType.Unknown;
        public ScratchType CallerType = ScratchType.Unknown;
        public string Code;
        public bool Warp;
        public Dictionary<string, object> Attributes = new();

        public ScratchIrProcedure(string name, IEnumerable<string> arguments)
        {
            Name = name;
            foreach (var argument in arguments)
                Arguments[argument] = ScratchType.Unknown;
        }

        public override string ToString()
        {
            var arguments = Arguments.Aggregate("",
                (current, pair) => current + $"{pair.Key}:{(pair.Value == ScratchType.Boolean ? "bool" : "sn")} ");
            return $"\nproc{(Warp ? ":w" : "")} {Name} {arguments}\n{Code}\n";
        }
    }

    private ScratchIrProcedure InitProcedure = new("__Init", Array.Empty<string>());
    public List<ScratchIrProcedure> Procedures = new();
    public List<ScratchFunction> Functions = new();

    public override TypedValue? VisitProcedureDeclarationStatement(
        ScratchScriptParser.ProcedureDeclarationStatementContext context)
    {
        var name = context.Identifier().GetText();
        if (Scope.IdentifierUsed(name))
        {
            DiagnosticReporter.Error(ScratchScriptError.IdentifierAlreadyUsed, context, context.Identifier().Symbol,
                name);
            return null;
        }

        var argumentNames = context.typedIdentifier().Select(x => x.Identifier().GetText()).ToList();
        var procedure = new ScratchIrProcedure(name, argumentNames);
        for (var i = 0; i < argumentNames.Count; i++)
        {
            if (Scope.IdentifierUsed(argumentNames[i]))
            {
                DiagnosticReporter.Error(ScratchScriptError.IdentifierAlreadyUsed, context,
                    context.typedIdentifier(i).Identifier().Symbol, argumentNames[i]);
                return null;
            }

            procedure.Arguments[argumentNames[i]] = CreateType(context.typedIdentifier(i).type());
        }

        foreach (var attributeStatementContext in context.attributeStatement())
            HandleProcedureAttribute(attributeStatementContext, ref procedure);
        Procedures.Add(procedure);

        var scope = CreateScope(context.block().line(), reporters: argumentNames);
        procedure.Code = scope.ToString();
        DefineFunction(procedure);
        return new(procedure.ToString());
    }

    public override TypedValue? VisitReturnStatement(ScratchScriptParser.ReturnStatementContext context)
    {
        var expression = Visit(context.expression());
        if (Procedures.Last().ReturnType == ScratchType.Unknown)
            Procedures.Last().ReturnType = TypeHelper.GetType(expression);
        return new(
            $"{Stack.PopFunctionArguments()}\npush {FunctionStackName} {expression}\n{Scope.Append}\n{Stack.PopArguments()}\nraw control_stop f:STOP_OPTION:\"this script\"\n");
    }

    public override TypedValue? VisitProcedureCallStatement(ScratchScriptParser.ProcedureCallStatementContext context)
    {
        var name = context.Identifier().GetText();
        var function = GetFunction(name);
        if (function == null)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureNotDefined, context, context.Identifier().Symbol,
                name);
            return null;
        }

        return HandleProcedureCall(context, function, true, context.procedureArgument());
    }

    private TypedValue? HandleProcedureCall(ParserRuleContext context, ScratchFunction function, bool isStatement,
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
                var arguments = new object[function.Arguments.Count];

                if (caller != null)
                    arguments[0] = caller.Format();

                for (var index = 0; index < procedureArguments.Length; index++)
                {
                    var argument = procedureArguments[index];
                    var argumentName = argument.Identifier() == null
                        ? function.Arguments.ElementAt(index + isMember).Name
                        : argument.Identifier().GetText();

                    var expression = Visit(argument.expression());
                    var argumentType = function.Arguments.First(x => x.Name == argumentName).Type;
                    if (TypeHelper.GetType(argumentType) != ScratchType.Unknown &&
                        AssertType(context, expression, argumentType, argument.expression()))
                        return null;
                    if (argumentType.Kind == ScratchTypeKind.List)
                        expression = PackListAsArgument(expression.Value);

                    var argumentIndex = function.Arguments.FindIndex(x => x.Name == argumentName);
                    arguments[argumentIndex] = expression;
                }

                return Scope.CallFunction(function.BlockInformation.Name, arguments,
                    isStatement
                        ? ScratchType.Unknown
                        : function.BlockInformation.ReturnType);
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
                    if (AssertNotNull(context, expression, argument.expression())) return null;
                    var argumentIndex = nativeFunction.Arguments.FindIndex(x => x.Name == argumentName);
                    var argumentType = function.Arguments.First(x => x.Name == argumentName).Type;

                    var allowedValues = function.Arguments[argumentIndex].AllowedValues;
                    if (allowedValues.Length != 0 && !allowedValues.Contains(expression.Format().RemoveQuotes()))
                    {
                        var values = string.Join(", ",
                            allowedValues.Select(x => x.Format(escapeStrings: true)));
                        DiagnosticReporter.Error(ScratchScriptError.ValueNotAllowed, context, argument.expression(),
                            values);
                        return null;
                    }

                    if (TypeHelper.GetType(argumentType) != ScratchType.Unknown &&
                        AssertType(context, expression, argumentType, argument.expression()))
                        return null;

                    arguments[argumentIndex] = expression.Format();
                }

                var result = nativeFunction.NativeMethod.Invoke(null, arguments);
                if (result != null)
                {
                    var resultString = result as string + "\n";
                    return new(resultString, nativeFunction.BlockInformation.ReturnType);
                }

                return null;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override TypedValue? VisitMemberProcedureCallExpression(
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

        return HandleProcedureCall(context, function, false, context.procedureCallStatement().procedureArgument(),
            member);
    }

    private ScratchFunction FindMemberFunction(object member, string functionName)
    {
        ScratchFunction function;
        if (member.IsVariable() && GetFunction(functionName, false, new(ScratchTypeKind.Variable)) != null)
            function = GetFunction(functionName, false, new(ScratchTypeKind.Variable));
        else if (member.IsList() && GetFunction(functionName, false, new(ScratchTypeKind.List)) != null)
            function = GetFunction(functionName, false, new(ScratchTypeKind.List));
        else
            function = GetFunction(functionName, false, TypeHelper.GetType(member));
        return function;
    }

    public override TypedValue? VisitMemberProcedureCallStatement(
        ScratchScriptParser.MemberProcedureCallStatementContext context)
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

        return HandleProcedureCall(context, function, true, context.procedureCallStatement().procedureArgument(),
            member);
    }

    public override TypedValue? VisitProcedureCallExpression(ScratchScriptParser.ProcedureCallExpressionContext context)
    {
        var name = context.procedureCallStatement().Identifier().GetText();
        var function = GetFunction(name);

        var disableCheck = function is DefinedScratchFunction definedFunction &&
                           definedFunction.Attributes.ContainsKey("DISABLE_TYPE_CHECK");
        if (function.BlockInformation.ReturnType == ScratchType.Unknown && !disableCheck)
        {
            DiagnosticReporter.Error(ScratchScriptError.ProcedureExpressionDoesNotReturn, context,
                ParserRuleContext.EmptyContext, name);
            return null;
        }

        var callResult =
            HandleProcedureCall(context, function, false, context.procedureCallStatement().procedureArgument());
        var isNative = function is NativeScratchFunction;
        var result = isNative ? callResult.Format() : $"{FunctionStackName}#(+ :pi: {Scope.ProcedureIndex})";
        return new(result, function.BlockInformation.ReturnType);
    }

    private void DefineFunction(ScratchIrProcedure procedure)
    {
        var function = new DefinedScratchFunction
        {
            BlockInformation = new ScratchBlockAttribute(string.IsNullOrEmpty(Namespace) ? "global" : Namespace,
                procedure.Name, procedure.ReturnType != ScratchType.Unknown,
                procedure.CallerType == ScratchType.Unknown, procedure.CallerType, procedure.ReturnType),
            Arguments = procedure.Arguments.Select(arg =>
                    new ScratchArgumentAttribute(arg.Key, arg.Value))
                .ToList(),
            Code = procedure.ToString(),
            Imported = false,
            Attributes = procedure.Attributes
        };
        Functions.Add(function);
    }

    private ScratchFunction
        GetFunction(string name, bool isStatic = true, ScratchType callerType = null) =>
        Functions.FirstOrDefault(x =>
            x.BlockInformation.Name == name && x.BlockInformation.IsStatic == isStatic &&
            x.BlockInformation.CallerType == (callerType ?? ScratchType.Unknown));
}