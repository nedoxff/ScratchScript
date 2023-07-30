using ScratchScript.Extensions;

namespace ScratchScript.Helpers;

public struct TypedValue
{
    public ScratchType Type;
    public object Value;
    public Dictionary<string, string> Data;

    public TypedValue(object value, ScratchType type = ScratchType.Unknown, Dictionary<string, string> data = null)
    {
        Type = type;
        Value = value;
        Data = data ?? new();
    }

    public override string ToString() => Value.Format();
}