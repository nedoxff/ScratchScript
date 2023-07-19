using System.Reflection;
using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Core.Reflection;

public class ReflectionBlockLoader
{
    public static Dictionary<string, List<NativeScratchFunction>> Functions = new();

    public static void Load()
    {
        Log.Verbose("--- ReflectionBlockLoader ---");
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes()
            .Where(x => x.GetMethods().Any(y => y.GetCustomAttribute<ScratchBlockAttribute>() != null)).ToList();

        foreach (var type in types)
        {
            Log.Verbose("Analyzing type {Type}", type.Name);
            var methods = type.GetMethods().Where(x => x.GetCustomAttribute<ScratchBlockAttribute>() != null);
            foreach (var methodInfo in methods)
            {
                var function = new NativeScratchFunction();
                var blockInformation = methodInfo.GetCustomAttribute<ScratchBlockAttribute>()!;

                if (!Functions.ContainsKey(blockInformation.Namespace))
                    Functions[blockInformation.Namespace] = new();

                if (methodInfo.ReturnType != typeof(string))
                    throw new Exception("All native scratch blocks must return an IR string!");
                
                function.BlockInformation = blockInformation;
                function.Arguments = methodInfo.GetParameters()
                    .Select(x => x.GetCustomAttribute<ScratchArgumentAttribute>()).ToList();
                function.NativeMethod = methodInfo;
                Functions[blockInformation.Namespace].Add(function);
            }
        }
        
        foreach (var (ns, functions) in Functions)
            Log.Verbose("Loaded {FunctionCount} functions to {Namespace}", functions.Count, ns);
    }
}