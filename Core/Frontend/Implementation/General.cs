using System.Globalization;
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
    public string ProceduresSection = "";
    public string ImportedProceduresSection = "";

    public string Output => $"{ImportedProceduresSection}\n{ProceduresSection}\n{_output}".RemoveEmptyLines();
    public string Namespace = "global";
    public bool Success = true;
    public string InputFile { get; }
    
    private readonly ScratchScriptParser _parser;
    public ScopeInfo Scope { get; set; }
    public int CurrentStackLength => Scope.PendingItemsCount;

    public string GetCleanupCode(int prev, bool modify = true)
    {
        var count = Scope.PendingItemsCount - prev;
        if(modify) Scope.PendingItemsCount -= count;
        return count switch
        {
            0 => "",
            1 => PopStackCommand,
            _ => $"repeat {count}\n{PopStackCommand}\nend"
        };
    }

    private ScopeInfo GlobalScope
    {
        get
        {
            var cur = Scope;
            while (cur.ParentScope != null) cur = cur.ParentScope;
            return cur;
        }
    }

    public ScratchScriptVisitor(ScratchScriptParser parser, string file)
    {
        _parser = parser;
        InputFile = file;
        Scope = new("", ""); // global scope
        
        Functions.AddRange(ReflectionBlockLoader.Functions.Values.SelectMany(f => f)); // imports all native scratch blocks
        if (!InputFile.EndsWith("std/global.scrs") && !Imports.ContainsKey("std/global"))
            ImportInternal("std/global");
        
        var list = new ScratchVariable("__Unicode", new ScratchType(ScratchTypeKind.List, ScratchType.String), false, "__Unicode");
        var symbols = new ScratchVariable("__Symbols", new ScratchType(ScratchTypeKind.String), false, "__Symbols");
        Scope.Variables.Add(list);
        Scope.Variables.Add(symbols);
        
        Instance = this;
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
        if (context.debuggerStatement() != null)
            return Visit(context.debuggerStatement());
        if (context.forStatement() != null)
            return Visit(context.forStatement());
        if (context.throwStatement() != null)
            return Visit(context.throwStatement());

        return null;
    }

    public override TypedValue? VisitProgram(ScratchScriptParser.ProgramContext context)
    {
        foreach (var statement in context.topLevelStatement())
        {
            var result = Visit(statement);
            if (result is { Value: string str })
            {
                if (statement.procedureDeclarationStatement() == null)
                    Scope.Content.Add(str);
                else
                {
                    ProceduresSection += str;
                    Scope.Variables.Add(new ScratchVariable
                    {
                        Type = Procedures.Last().ReturnType,
                        Name = Procedures.Last().Name,
                    });
                }
            }
        }

        //TODO: why was there an Init procedure?
        //Scope.Content.Insert(0, InitProcedure + "end\n");
        if (!Scope.Content.Any(x => x.StartsWith("on start")))
            Scope.Content.Add($"on start\npopall {StackName}\nend");
        else
        {
            var index = Scope.Content.FindIndex(x => x.StartsWith("on start"));
            var split = Scope.Content[index].Split("\n").ToList();
            split.Insert(1, $"popall {StackName}");
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
        if (context.type() is { } t)
            return new(CreateType(t), ScratchType.Type);
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
        if (FunctionNamespaces.ContainsKey(identifier))
            return new(identifier, ScratchType.Identifier);
        
        if (!Scope.IdentifierUsed(identifier))
            return null;

        var variable = Scope.GetVariable(identifier);
        
        if (variable.IsReporter)
            return Stack.GetArgument(variable);
        
        var ir = variable.Type.Kind == ScratchTypeKind.List ? $"arr:{variable.Id}" : $"var:{(variable.IsReporter ? "argr:" : "")}{variable.Id}";
        return new(ir, variable.Type);
    }

    public override TypedValue? VisitTernaryExpression(ScratchScriptParser.TernaryExpressionContext context)
    {
        var condition = Visit(context.expression(0));
        if (AssertType(context, condition, ScratchType.Boolean, context.expression(0))) return null;
        var first = Visit(context.expression(1));
        var second = Visit(context.expression(2));
        if (AssertType(context, first, second, context.expression(2))) return null;

        RequireFunction("__Ternary", context);
        return Scope.CallFunction("__Ternary",
            new object[] { condition, first, second }, TypeHelper.GetType(first));
    }

    public override TypedValue? VisitBlock(ScratchScriptParser.BlockContext context)
    {
        var scope = CreateScope(context.line());
        return new(scope.ToString());
    }

    private ScopeInfo CreateScope(IEnumerable<ScratchScriptParser.LineContext> lines, string startingLine = "",
        IEnumerable<string> reporters = null, bool isFunctionScope = false)
    {
        var scope = new ScopeInfo(startingLine, isFunctionScope: isFunctionScope);
        if (reporters != null)
        {
            foreach (var reporter in reporters)
            {
                if (!Procedures.Last().Arguments.TryGetValue(reporter, out var type)) continue;
                scope.Variables.Add(new ScratchVariable(reporter,
                     type, true));
            }
        }

        scope.ParentScope = Scope;
        scope.Variables.AddRange(Scope.Variables);
        Scope = scope;

        foreach (var line in lines)
        {
            var result = Visit(line);
            if (result is not { Value: string resultString }) continue;
            scope.Content.Add(resultString);
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

    //TODO: no try/catch (for now) because its complicated
    public override TypedValue? VisitThrowStatement(ScratchScriptParser.ThrowStatementContext context)
    {
        RequireFunction("__Throw", context);
        return Scope.CallFunction("__Throw", new object[] { context.String().GetText() }, ScratchType.Unknown);
    }

    public ScratchType CreateType(ScratchScriptParser.TypeContext context)
    {
        var type = new ScratchType(context.Type() == null ? "List": context.Type().GetText());
        if (context.type() != null)
        {
            var childType = CreateType(context.type());
            childType.ParentType = type;
            type.ChildType = childType;
        }

        return type;
    }
}