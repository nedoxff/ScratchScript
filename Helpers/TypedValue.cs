using ScratchScript.Extensions;

namespace ScratchScript.Helpers;

public struct TypedValue
{
    public ScratchType Type;
    public object Value;
    public Dictionary<string, string> Data;

    public TypedValue(object value, ScratchType type = null, Dictionary<string, string> data = null)
    {
        Type = type ?? ScratchType.Unknown;
        Value = value;
        Data = data ?? new();
    }

    public override string ToString() => Value.Format();
}