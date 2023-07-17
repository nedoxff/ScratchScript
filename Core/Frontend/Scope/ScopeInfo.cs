using ScratchScript.Helpers;

namespace ScratchScript.Core.Frontend.Scope;

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
}