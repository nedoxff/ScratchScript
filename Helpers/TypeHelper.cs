using ScratchScript.Core.Models;

namespace ScratchScript.Helpers;

public class ScratchType
{
    private static readonly Dictionary<ScratchTypeKind, string> _kindConverter = new()
    {
        { ScratchTypeKind.Any, "any" },
        { ScratchTypeKind.Boolean, "bool" },
        { ScratchTypeKind.Color, "color" },
        { ScratchTypeKind.List, "list" },
        { ScratchTypeKind.Number, "number" },
        { ScratchTypeKind.String, "string" },
        
        { ScratchTypeKind.Unknown, "unknown" },
        { ScratchTypeKind.Type, "type" },
        { ScratchTypeKind.Variable , "var" }
    };
    
    public static readonly ScratchType Unknown = new(ScratchTypeKind.Unknown);
    public static readonly ScratchType Number = new(ScratchTypeKind.Number);
    public static readonly ScratchType String = new(ScratchTypeKind.String);
    public static readonly ScratchType Boolean = new(ScratchTypeKind.Boolean);
    public static readonly ScratchType Color = new(ScratchTypeKind.Color);
    // Custom types for the compiler
    public static readonly ScratchType Any = new(ScratchTypeKind.Any);
    public static readonly ScratchType Type = new(ScratchTypeKind.Type);

    public static ScratchType List(ScratchType innerType)
    {
        var type = new ScratchType(ScratchTypeKind.List);
        innerType.ParentType = type;
        type.ChildType = innerType;
        return type;
    }

    public ScratchTypeKind Kind { get; set; }
    public string Name { get; set; }
    public ScratchType ParentType { get; set; }
    public ScratchType ChildType { get; set; }

    public ScratchType(ScratchTypeKind kind, ScratchType childType = null, ScratchType parentType = null)
    {
        Kind = kind;
        Name = _kindConverter[kind];
        ParentType = parentType;
        ChildType = childType;
    }
    
    public ScratchType(string name)
    {
        Kind = _kindConverter.First(x => x.Value == name).Key;
        Name = name;
    }

    public override string ToString() => $"{Name}{(ChildType is not null ? $"<{ChildType}>" : "")}";

    public override bool Equals(object obj)
    {
        if (obj is not ScratchType other) return false;
        return ToString() == other.ToString(); // note: not sure if this is a great method but it should work
    }

    public static bool operator ==(ScratchType first, ScratchType second) => first?.Equals(second) ?? false;
    public static bool operator !=(ScratchType first, ScratchType second) => !(first?.Equals(second) ?? false);

    public override int GetHashCode() => ToString().GetHashCode();
}

public enum ScratchTypeKind
{
    Unknown = 0,
    Number = 4,
    Color = 9,
    String = 10,
    Variable = 12,
    List = 13,
    Boolean = 15,
    // These should NEVER we encoded to scratch blocks, they're only used in the compiler
    Any,
    Type
}

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
            "data_variable" => new(ScratchTypeKind.Variable),
            "data_list" => new(ScratchTypeKind.List),
            "argument_reporter_string_number" => b.ExpectedType,
            "argument_reporter_boolean" => b.ExpectedType,
            _ => b.ExpectedType == ScratchType.Unknown ? _typeConverter[o.GetType()] : b.ExpectedType
        };
    }

    private static Dictionary<Type, ScratchType> _typeConverter = new()
    {
        { typeof(int), ScratchType.Number },
        { typeof(decimal), ScratchType.Number },
        { typeof(string), ScratchType.String },
        { typeof(bool), ScratchType.Boolean },
        { typeof(ScratchColor), ScratchType.Color }
    };
}

public enum ScratchShadowType
{
    Shadow = 1,
    NoShadow = 2,
    ObscuredShadow = 3
}

public struct ScratchColor //TODO: workaround??? idk
{
    public string Value;

    public ScratchColor(string value)
    {
        Value = value;
    }
}