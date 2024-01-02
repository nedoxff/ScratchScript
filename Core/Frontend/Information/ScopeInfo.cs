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
    
    public int PendingItemsCount = 0;
    public bool IsFunctionScope;

    public ScopeInfo(string startingLine = "", string endingLine = "end\n", bool isFunctionScope = false)
    {
        StartingLine = startingLine;
        EndingLine = endingLine;
        IsFunctionScope = isFunctionScope;
    }

    public ScopeInfo ParentScope = null;
    public List<string> Content = new();

    public ScratchVariable GetVariable(string name) => Variables.FirstOrDefault(x => x.Name == name);
    public bool IsInsideFunction()
    {
        var scope = this;
        while (scope.ParentScope != null)
        {
            if (scope.IsFunctionScope) return true;
            scope = scope.ParentScope;
        }

        return false;
    }

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
        var before = arguments.Aggregate("", (current, argument) => current + Stack.PushArgument(argument));
        before += $"call {name}\n";
        if (returnType != ScratchType.Unknown) PendingItemsCount++;
        return returnType != ScratchType.Unknown
            ? new($"{ScratchScriptVisitor.StackName}#{(IsInsideFunction() ? $"(+ :si: {PendingItemsCount})": PendingItemsCount)}", returnType, before: before)
            : new(before);
    }

    public TypedValue? PackList(IEnumerable<object> values, ScratchType expectedType)
    {
        var before = "";
        before += "set var:__CopyList \"\"\n";
        foreach (var value in values)
        {
            var stackCapture = ScratchScriptVisitor.Instance.CurrentStackLength;
            var newList = CallFunction("__WriteListValue", new[] { "var:__CopyList", value }, ScratchType.String);
            before += $"set var:__CopyList {newList.Format()}\n{ScratchScriptVisitor.Instance.GetCleanupCode(stackCapture)}";
        }

        return new("var:__CopyList", ScratchType.List(expectedType), before: before);
    }
}