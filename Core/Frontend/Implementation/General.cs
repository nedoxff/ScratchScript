using System.Globalization;
using System.Text.RegularExpressions;
using ScratchScript.Core.Diagnostics;
using ScratchScript.Core.Frontend.Information;
using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor : ScratchScriptBaseVisitor<TypedValue?>
{
    public static ScratchScriptVisitor Instance { get; private set; }

    private string _output = "";
    private string _proceduresSection = "";
    private string _loadSection = "";

    public string Output => $"{_loadSection}\n{_proceduresSection}\n{_output}".RemoveEmptyLines();
    public string Namespace = "global";
    public bool Success = true;
    public string InputFile { get; }

    public List<DefinedScratchFunction> DefinedFunctions =>
        Functions.Where(x => x is DefinedScratchFunction { Imported: false }).Select(x => x as DefinedScratchFunction)
            .ToList();

    private readonly List<string> _imports = new();
    private readonly ScratchScriptParser _parser;
    private ScopeInfo Scope { get; set; }

    public ScratchScriptVisitor(ScratchScriptParser parser, string file)
    {
        _parser = parser;
        Instance = this;
        InputFile = file;
        Scope = new("", "");
    }

    public override TypedValue? VisitTopLevelStatement(ScratchScriptParser.TopLevelStatementContext context)
    {
        if (context.attributeStatement() != null)
            return Visit(context.attributeStatement());
        if (context.procedureDeclarationStatement() != null)
            return Visit(context.procedureDeclarationStatement());
        if (context.eventStatement() != null)
            return Visit(context.eventStatement());
        if (context.importStatement() != null)
            return Visit(context.importStatement());
        if (context.namespaceStatement() != null)
            return Visit(context.namespaceStatement());
        return null;
    }

    public override TypedValue? VisitEventStatement(ScratchScriptParser.EventStatementContext context)
    {
        //TODO: temporary implementation
        var startingLine = $"on {context.Identifier()}";
        var scope = CreateScope(context.block().line(), startingLine);
        return new(scope.ToString());
    }

    public override TypedValue? VisitLine(ScratchScriptParser.LineContext context)
    {
        if (context.statement() != null)
            return Visit(context.statement());
        if (context.ifStatement() != null)
            return Visit(context.ifStatement());
        if (context.whileStatement() != null)
            return Visit(context.whileStatement());
        if (context.switchStatement() != null)
            return Visit(context.switchStatement());
        if (context.repeatStatement() != null)
            return Visit(context.repeatStatement());
        if (context.comment() != null)
            return Visit(context.comment());
        if (context.returnStatement() != null)
            return Visit(context.returnStatement());
        if (context.breakStatement() != null)
            return Visit(context.breakStatement());
        if (context.forStatement() != null)
            return Visit(context.forStatement());

        return null;
    }

    public override TypedValue? VisitProgram(ScratchScriptParser.ProgramContext context)
    {
        // Add all global functions from the STD (if they're not used they can be optimised later)
        if (StdLoader.Functions.TryGetValue("global", out var functions))
        {
            Functions.AddRange(functions);
            _proceduresSection += functions.Select(x => x.Code).Aggregate("", (current, next) => current + "\n" + next);
        }

        InitProcedure.Code += $"popall {FunctionStackName}\npopall {StackName}";

        foreach (var statement in context.topLevelStatement())
        {
            var result = Visit(statement);
            if (result is { Value: string str })
            {
                if (statement.procedureDeclarationStatement() == null)
                    Scope.Content.Add(str);
                else
                {
                    _proceduresSection += str;
                    Scope.Variables.Add(new ScratchVariable
                    {
                        Type = Procedures.Last().ReturnType,
                        Name = Procedures.Last().Name,
                    });
                }
            }
        }

        Scope.Content.Insert(0, InitProcedure + "end\n");
        if (!Scope.Content.Any(x => x.StartsWith("on start")))
            Scope.Content.Add("on start\ncall __Init\nend");
        else
        {
            var index = Scope.Content.FindIndex(x => x.StartsWith("on start"));
            var split = Scope.Content[index].Split("\n").ToList();
            split.Insert(1, "call __Init");
            Scope.Content[index] = string.Join('\n', split);
        }

        _output += Scope.ToString();
        return null;
    }

    public override TypedValue? VisitStatement(ScratchScriptParser.StatementContext context)
    {
        if (context.assignmentStatement() != null)
            return Visit(context.assignmentStatement());
        if (context.procedureCallStatement() != null)
            return Visit(context.procedureCallStatement());
        if (context.memberProcedureCallStatement() != null)
            return Visit(context.memberProcedureCallStatement());
        if (context.variableDeclarationStatement() != null)
            return Visit(context.variableDeclarationStatement());
        if (context.postIncrementStatement() != null)
            return Visit(context.postIncrementStatement());

        return null;
    }

    public override TypedValue? VisitConstant(ScratchScriptParser.ConstantContext context)
    {
        if (context.Number() is { } n)
            return new(decimal.Parse(n.GetText(), CultureInfo.InvariantCulture), ScratchType.Number);
        if (context.String() is { } s)
            return new(s.GetText(), ScratchType.String);
        if (context.boolean() is { } b)
            return new(b.GetText() == "true", ScratchType.Boolean);
        if (context.Color() is { } c)
            return new(new ScratchColor(c.GetText()[1..]), ScratchType.Color);
        return null;
    }

    public override TypedValue? VisitParenthesizedExpression(ScratchScriptParser.ParenthesizedExpressionContext context)
    {
        var expression = Visit(context.expression());
        if (AssertNotNull(context, expression, context.expression())) return null;

        var result = $"({expression.Format()})";
        return new(result, expression.Value.Type);
    }

    public override TypedValue? VisitIdentifierExpression(ScratchScriptParser.IdentifierExpressionContext context)
    {
        var result = VisitIdentifierInternal(context.Identifier().GetText());
        if (result == null)
        {
            DiagnosticReporter.Error(ScratchScriptError.UnknownIdentifier, context, context.Identifier().Symbol, context.Identifier().GetText());
        }
        return result;
    }

    private TypedValue? VisitIdentifierInternal(string identifier)
    {
        if (!Scope.IdentifierUsed(identifier))
            return null;

        var variable = Scope.GetVariable(identifier);
        
        if (variable.IsReporter)
            return Stack.GetArgument(variable);
        
        var ir = variable.IsList ? $"arr:{variable.Id}" : $"var:{(variable.IsReporter ? "argr:" : "")}{variable.Id}";
        return new(ir, variable.Type);
    }

    public override TypedValue? VisitTernaryExpression(ScratchScriptParser.TernaryExpressionContext context)
    {
        var condition = Visit(context.expression(0));
        if (AssertType(context, condition, ScratchType.Boolean, context.expression(0))) return null;
        var first = Visit(context.expression(1));
        var second = Visit(context.expression(2));
        if (AssertType(context, first, second, context.expression(2))) return null;

        return Scope.CallFunction($"__Ternary{Enum.GetName(first.Value.Type)}",
            new object[] { condition, first, second }, TypeHelper.GetType(first));
    }

    public override TypedValue? VisitBlock(ScratchScriptParser.BlockContext context)
    {
        var scope = CreateScope(context.line());
        return new(scope.ToString());
    }

    private ScopeInfo CreateScope(IEnumerable<ScratchScriptParser.LineContext> lines, string startingLine = "",
        IEnumerable<string> reporters = null)
    {
        var scope = new ScopeInfo(startingLine);
        if (reporters != null)
        {
            foreach (var reporter in reporters)
                scope.Variables.Add(new ScratchVariable(reporter, Procedures.Last().Arguments.TryGetValue(reporter, out var type) ? type: ScratchType.Unknown, true));
        }

        scope.ParentScope = Scope;
        scope.ProcedureIndex = Scope.ProcedureIndex;
        scope.Variables.AddRange(Scope.Variables);
        Scope = scope;

        foreach (var line in lines)
        {
            var result = Visit(line);
            if (result is not { Value: string resultString }) continue;
            if (line.returnStatement() != null ||
                line.breakStatement() != null) // No code should be after the return statement
                scope.Content.Add(Scope.Prepend + resultString);
            else
                scope.Content.Add(Scope.Prepend + resultString + Scope.Append);
            Scope.ProcedureIndex -= Regex.Matches(Scope.Append, PopFunctionStackCommand).Count;
            Scope.Append = "";
            Scope.Prepend = "";
        }

        Scope = scope.ParentScope;
        return scope;
    }

    public override TypedValue? VisitNamespaceStatement(ScratchScriptParser.NamespaceStatementContext context)
    {
        var name = context.String().GetText()[1..^1];
        if (Procedures.Count != 0 && Namespace == "global")
        {
            DiagnosticReporter.Error(ScratchScriptError.NamespacePlacedIncorrectly, context, context);
            return null;
        }

        Namespace = name;
        return null;
    }

    public override TypedValue? VisitImportStatement(ScratchScriptParser.ImportStatementContext context)
    {
        var name = context.String().GetText()[1..^1];
        var names = context.Identifier().Select(x => x.GetText()).ToList();
        //TODO: also add searching in other files
        if (ReflectionBlockLoader.Functions.TryGetValue(name, out var nativeFunctions))
        {
            _imports.Add(name);
            if (names.Count == 0)
                Functions.AddRange(nativeFunctions);
            else
            {
                foreach (var func in names)
                {
                    if (nativeFunctions.All(x => x.BlockInformation.Name != func))
                    {
                        //TODO: ERROR
                    }
                    else Functions.Add(nativeFunctions.First(x => x.BlockInformation.Name == func));
                }
            }

            return null;
        }

        if (StdLoader.Functions.TryGetValue(name, out var definedFunctions))
        {
            _imports.Add(name);
            if (names.Count == 0)
            {
                Functions.AddRange(definedFunctions);
                _proceduresSection += definedFunctions.Select(x => x.Code)
                    .Aggregate("", (current, next) => current + "\n" + next);
            }
            else
            {
                foreach (var functionName in names)
                {
                    if (definedFunctions.All(x => x.BlockInformation.Name != functionName))
                    {
                        //TODO: ERROR
                    }
                    else
                    {
                        var func = definedFunctions.First(x => x.BlockInformation.Name == functionName);
                        Functions.Add(func);
                        _proceduresSection += func.Code + "\n";
                    }
                }
            }
        }

        return null;
    }
}