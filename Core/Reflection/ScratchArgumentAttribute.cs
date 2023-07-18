using ScratchScript.Helpers;

namespace ScratchScript.Core.Reflection;

[AttributeUsage(AttributeTargets.Parameter)]
public class ScratchArgumentAttribute: Attribute
{
    public string Name;
    public ScratchType Type;
    public object[] AllowedValues;

    public ScratchArgumentAttribute(string name, ScratchType type, object[] allowedValues = null)
    {
        Name = name;
        Type = type;
        AllowedValues = allowedValues ?? Array.Empty<object>();
    }
}