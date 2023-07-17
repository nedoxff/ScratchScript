using System.Text.RegularExpressions;
using Antlr4.Runtime.Tree.Xpath;
using ScratchScript.Core.Frontend.Scope;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Implementation;

public partial class ScratchScriptVisitor: ScratchScriptBaseVisitor<object>
{
    public static ScratchScriptVisitor Instance { get; private set; }

    private string _output = "";
    private string _proceduresSection = "";
    private string _loadSection = "";

    public string Output => RemoveEmptyLines($"{_loadSection}\n{_proceduresSection}\n{_output}");
    private ScratchScriptParser _parser;
    private Dictionary<string, ScratchType> _typeLookup = new();
    private ScopeInfo _currentScope;

    private List<string> _reservedNames = new()
    {
        "__FunctionReturnValues"
    };

    private ScratchType GetType(object o)
    {
        return o switch
        {
            string search => _typeLookup.TryGetValue(search, out var value) ? value : ScratchType.String,
            _ => TypeHelper.GetType(o)
        };
    }

    private void SaveType(string s, ScratchType type) => _typeLookup[s] = type;

    public ScratchScriptVisitor(ScratchScriptParser parser)
    {
        _parser = parser;
        Instance = this;
    }

    public override object VisitTopLevelStatement(ScratchScriptParser.TopLevelStatementContext context)
    {
        object result = null;
        if (context.attributeStatement() != null)
            result = Visit(context.attributeStatement());
        if (context.procedureDeclarationStatement() != null)
            result = Visit(context.procedureDeclarationStatement());
        if (context.eventStatement() != null)
            result = Visit(context.eventStatement());
        return result;
    }

    public override object VisitEventStatement(ScratchScriptParser.EventStatementContext context)
    {
        //TODO: temporary implementation
        var startingLine = $"on {context.Identifier()}";
        var scope = CreateScope(context.block().line(), startingLine);
        return scope.ToString();
    }

    public override object VisitLine(ScratchScriptParser.LineContext context)
    {
        object result = null;
        if (context.statement() != null)
            result = Visit(context.statement());
        if (context.ifStatement() != null)
            result = Visit(context.ifStatement());
        if (context.whileStatement() != null)
            result = Visit(context.whileStatement());
       
        if (context.repeatStatement() != null)
            result = Visit(context.repeatStatement());
        if (context.comment() != null)
            result = Visit(context.comment());
        
        return result;
    }

    public override object VisitProgram(ScratchScriptParser.ProgramContext context)
    {
        _currentScope = new("", "");
        InitProcedure.Code += "popall __FunctionReturnValues";
        
        foreach (var statement in context.topLevelStatement())
        {
            var result = Visit(statement);
            if (result is string str)
            {
                if(statement.procedureDeclarationStatement() == null)
                    _currentScope.Content.Add(str);
                else
                {
                    _proceduresSection += str;
                    _currentScope.Variables.Add(new ScratchVariable
                    {
                        Type = _procedures.Last().ReturnType,
                        Name = _procedures.Last().Name,
                    });
                }
            }
        }

        _currentScope.Content.Insert(0, InitProcedure + "end\n");
        if (!_currentScope.Content.Any(x => x.StartsWith("on start")))
            _currentScope.Content.Add("on start\ncall __Init\nend");
        else
        {
            var index = _currentScope.Content.FindIndex(x => x.StartsWith("on start"));
            var split = _currentScope.Content[index].Split("\n").ToList();
            split.Insert(1, "call __Init");
            _currentScope.Content[index] = string.Join('\n', split);
        }

        _output += _currentScope.ToString();
        return null;
    }

    public override object VisitStatement(ScratchScriptParser.StatementContext context)
    {
        if (context.assignmentStatement() != null)
            return Visit(context.assignmentStatement());
        if (context.procedureCallStatement() != null)
            return Visit(context.procedureCallStatement());
        if (context.variableDeclarationStatement() != null)
            return Visit(context.variableDeclarationStatement());
        if (context.importStatement() != null)
            return Visit(context.importStatement());
        if (context.returnStatement() != null)
            return Visit(context.returnStatement());
        if (context.breakStatement() != null)
            return Visit(context.breakStatement());

        return null;
    }
    
    public override object VisitConstant(ScratchScriptParser.ConstantContext context)
    {
        if (context.Number() is { } n)
            return decimal.Parse(n.GetText());
        if (context.String() is { } s)
            return s.GetText();
        if (context.boolean() is { } b)
            return b.GetText() == "true";
        return null;
    }

    public override string VisitParenthesizedExpression(ScratchScriptParser.ParenthesizedExpressionContext context)
    {
        var expression = Visit(context.expression());
        var expressionType = GetType(expression);
        
        var result = $"({FormatString(expression)})";
        SaveType(result, expressionType);
        return result;
    }

    public override object VisitIdentifierExpression(ScratchScriptParser.IdentifierExpressionContext context)
    {
        return VisitIdentifierInternal(context.Identifier().GetText());
    }

    private object VisitIdentifierInternal(string identifier)
    {
        if (!_currentScope.IdentifierUsed(identifier))
            return null;
        var variable = _currentScope.GetVariable(identifier);
        var ir = $"v:{(variable.IsReporter ? "argr:" : "")}{identifier}";
        SaveType(ir, variable.Type);
        return ir;
    }

    public override object VisitBlock(ScratchScriptParser.BlockContext context)
    {
        var scope = CreateScope(context.line());
        return scope.ToString();
    }
    
    private ScopeInfo CreateScope(IEnumerable<ScratchScriptParser.LineContext> lines, string startingLine = "", IEnumerable<string> reporters = null)
    {
        var scope = new ScopeInfo(startingLine);
        if (reporters != null)
        {
            foreach(var reporter in reporters)
                scope.Variables.Add(new ScratchVariable(reporter, ScratchType.Unknown, true));
        }
        scope.ParentScope = _currentScope;
        scope.ProcedureIndex = _currentScope.ProcedureIndex;
        scope.Variables.AddRange(_currentScope.Variables);
        _currentScope = scope;

        foreach (var line in lines)
        {
            var result = Visit(line);
            if (result is not string resultString) continue;
            if (line.statement()?.returnStatement() != null || line.statement()?.breakStatement() != null) // No code should be after the return statement
                scope.Content.Add(_currentScope.Prepend + resultString); 
            else
                scope.Content.Add(_currentScope.Prepend + resultString + _currentScope.Append);
            _currentScope.ProcedureIndex -= Regex.Matches(_currentScope.Append, PopFunctionStackCommand).Count;
            _currentScope.Append = "";
            _currentScope.Prepend = "";
        }

        _currentScope = scope.ParentScope;
        return scope;
    }

    // Annoying workaround
    private string FormatString(object o) => o is bool b ? $"\"{b.ToString().ToLower()}\"" : o.ToString();

    private string RemoveEmptyLines(string s) => string.Join('\n', s.Split("\n").Where(x => !string.IsNullOrEmpty(x)));
}