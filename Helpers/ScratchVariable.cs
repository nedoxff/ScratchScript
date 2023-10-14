namespace ScratchScript.Helpers;

public struct ScratchVariable
{
    public string Name;
    public string Id;
    public ScratchType Type;
    public bool IsReporter;

    public ScratchVariable(string name, ScratchType type = null, bool isReporter = false, string id = "")
    {
        Name = name;
        Id = string.IsNullOrEmpty(id) ? NameHelper.New(name): id;
        Type = type ?? ScratchType.Unknown;
        IsReporter = isReporter;
    }
}