using ScratchScript.Extensions;

namespace ScratchScript.Helpers;

public struct TypedValue
{
    public ScratchType Type;
    public object Value;
    public Dictionary<string, string> Data;
    public string Before;
    public string After;

    public TypedValue(object value, ScratchType type = null, Dictionary<string, string> data = null, string before = "", string after = "")
    {
        Type = type ?? ScratchType.Unknown;
        Value = value;
        Data = data ?? new();
        Before = before;
        After = after;
    }

    public override string ToString() => Value.Format();
}