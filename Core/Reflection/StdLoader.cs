using Antlr4.Runtime;
using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Core.Reflection;

public class StdLoader
{
    public static Dictionary<string, List<DefinedScratchFunction>> Functions = new();

    public static void Load(string stdPath)
    {
        Log.Verbose("--- StdLoader ---");
        foreach (var file in Directory.EnumerateFiles(stdPath, "*.scrs", SearchOption.AllDirectories))
        {
            Log.Verbose("Analyzing file {File}", Path.GetFullPath(file).Replace(stdPath, ""));
            var inputStream = new AntlrFileStream(file);
            var lexer = new ScratchScriptLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ScratchScriptParser(tokenStream);
            var visitor = new ScratchScriptVisitor(parser);
            visitor.Visit(parser.program());
            var newFunctions = visitor.DefinedFunctions.Select(x =>
            {
                x.Imported = true;
                return x;
            }).ToList();
            if(Functions.TryGetValue(visitor.Namespace, out var functions))
                functions.AddRange(newFunctions);
            Functions.TryAdd(visitor.Namespace, newFunctions);
            Log.Verbose("Loaded {Count} functions to {Namespace}", newFunctions.Count, visitor.Namespace);
        }
    }
}