using ScratchScript.Core.Models;

namespace ScratchScript.Helpers;

public class TypeHelper
{
    public static ScratchType GetType(object o)
    {
        if (o is null) return ScratchType.Unknown;
        if (o is ScratchType type) return type;
        if (o is TypedValue value) return value.Type;
        if (o is not Block b) return _typeConverter[o.GetType()];
        return b.Opcode switch
        {
            "data_variable" => ScratchType.Variable,
            "data_list" => ScratchType.List,
            "argument_reporter_string_number" => b.ExpectedType,
            "argument_reporter_boolean" => b.ExpectedType,
            _ => b.ExpectedType == ScratchType.Unknown ? _typeConverter[o.GetType()]: b.ExpectedType
        };
    }

    public static ScratchShadowType GetShadowType(object o)
    {
        var type = GetType(o);
        return type is ScratchType.List or ScratchType.Variable ? ScratchShadowType.ObscuredShadow : ScratchShadowType.Shadow;
    }

    private static Dictionary<Type, ScratchType> _typeConverter = new()
    {
        {typeof(int), ScratchType.Number},
        {typeof(decimal), ScratchType.Number},
        {typeof(string), ScratchType.String},
        {typeof(bool), ScratchType.Boolean},
        {typeof(ScratchColor), ScratchType.Color}
    };

    public static readonly string[] PossibleTypes = new[] {"num", "str", "bool", "color"};

    public static ScratchType StringToScratchType(string str)
    {
        return str switch
        {
            "num" => ScratchType.Number,
            "str" => ScratchType.String,
            "bool" => ScratchType.Boolean,
            "color" => ScratchType.Color,
            _ => ScratchType.Unknown
        };
    }

    public static string ScratchTypeToString(ScratchType type)
    {
        return type switch
        {
            ScratchType.String => "s",
            ScratchType.Boolean => "s",
            ScratchType.Color => "s",
            ScratchType.List => "l",
            ScratchType.Number => "n",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static ScratchType ScratchTypeFromString(string str)
    {
        return str switch
        {
            "s" => ScratchType.String,
            "l" => ScratchType.List,
            "n" => ScratchType.Number,
        };
    }
}

public enum ScratchShadowType
{
    Shadow = 1,
    NoShadow = 2,
    ObscuredShadow = 3
}

public enum ScratchType
{
    Unknown = 0,
    Number = 4,
    Color = 9,
    String = 10,
    Variable = 12,
    List = 13,
    Boolean = 15
}

public struct ScratchColor //TODO: workaround??? idk
{
    public string Value;
    public ScratchColor(string value)
    {
        Value = value;
    }
}