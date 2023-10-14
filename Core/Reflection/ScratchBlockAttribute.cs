using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Core.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class ScratchBlockAttribute: Attribute
{
    public string Namespace;
    public string Name;
    public bool IsShadow;
    public bool IsStatic;
    public ScratchType ReturnType;
    public ScratchType CallerType;

    public ScratchBlockAttribute(string ns, string name, bool isShadow, bool isStatic, ScratchTypeKind callerType = ScratchTypeKind.Unknown, ScratchTypeKind returnType = ScratchTypeKind.Unknown)
    {
        Namespace = ns;
        Name = name;
        IsShadow = isShadow;
        IsStatic = isStatic;
        ReturnType = new(returnType);
        CallerType = new(callerType);
        
        if(callerType == ScratchTypeKind.Unknown && !isStatic)
            Log.Error("Cannot declare a non-static block on an object with unknown type!");
    }
    
    public ScratchBlockAttribute(string ns, string name, bool isShadow, bool isStatic, ScratchType callerType, ScratchType returnType)
    {
        Namespace = ns;
        Name = name;
        IsShadow = isShadow;
        IsStatic = isStatic;
        ReturnType = returnType;
        CallerType = callerType;
        
        if(callerType.Kind == ScratchTypeKind.Unknown && !isStatic)
            Log.Error("Cannot declare a non-static block on an object with unknown type!");
    }
}