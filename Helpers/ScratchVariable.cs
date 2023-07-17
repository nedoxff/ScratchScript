namespace ScratchScript.Helpers;

public struct ScratchVariable
{
    public string Name;
    public string Id;
    public ScratchType Type;
    public bool IsReporter;

    public ScratchVariable(string name, ScratchType type = ScratchType.Unknown, bool isReporter = false)
    {
        Name = name;
        Id = Guid.NewGuid().ToString("N");
        Type = type;
        IsReporter = isReporter;
    }
}