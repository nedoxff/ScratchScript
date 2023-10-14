using System.Formats.Asn1;
using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Information;

public class ScopeInfo
{
    public List<ScratchVariable> Variables = new();

    public string StartingLine;
    public string EndingLine;

    public string Append = "";
    public string Prepend = "";
    public int ProcedureIndex;

    public ScopeInfo(string startingLine = "", string endingLine = "end\n")
    {
        StartingLine = startingLine;
        EndingLine = endingLine;
    }

    public ScopeInfo ParentScope = null;
    public List<string> Content = new();

    public ScratchVariable GetVariable(string name) => Variables.FirstOrDefault(x => x.Name == name);

    public override string ToString()
    {
        var content = string.Join('\n', Content);
        return $"{StartingLine}\n{content}\n{EndingLine}";
    }

    public bool IdentifierUsed(string identifier)
    {
        var scope = this;
        while (scope != null)
        {
            if (scope.Variables.Any(x => x.Name == identifier)) return true;
            scope = scope.ParentScope;
        }

        return false;
    }

    public TypedValue CallFunction(string name, object[] arguments, ScratchType returnType)
    {
        foreach (var argument in arguments)
            Prepend += Stack.PushArgument(argument);
        Prepend += $"call {name}\n";
        if (returnType != ScratchType.Unknown)
        {
            ProcedureIndex++;
            Append += ScratchScriptVisitor.PopFunctionStackCommand;
        }
        return returnType != ScratchType.Unknown
            ? new($"{ScratchScriptVisitor.FunctionStackName}#(+ :pi: {ProcedureIndex})", returnType)
            : new("");
    }

    public TypedValue? PackList(IEnumerable<object> values, ScratchType expectedType)
    {
        Prepend += "set var:__CopyList \"\"";
        foreach (var value in values)
        {
            var newList = CallFunction("__WriteListValue", new[] { "var:__CopyList", value }, ScratchType.String);
            Prepend += $"set var:__CopyList {newList.Format()}\n";
        }

        return new("var:__CopyList", ScratchType.List(expectedType));
    }
}