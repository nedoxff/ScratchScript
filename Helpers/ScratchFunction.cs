using System.Reflection;
using ScratchScript.Core.Reflection;

namespace ScratchScript.Helpers;

public abstract class ScratchFunction
{
    public ScratchBlockAttribute BlockInformation { get; set; }
    public List<ScratchArgumentAttribute> Arguments { get; set; }
}

public class NativeScratchFunction : ScratchFunction
{
    public MethodInfo NativeMethod { get; set; }
}

public class DefinedScratchFunction : ScratchFunction
{
    public string Code { get; set; }
    public List<string> Dependencies = new();
    public Dictionary<string, object> Attributes = new();
}