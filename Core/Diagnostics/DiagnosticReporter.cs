using Antlr4.Runtime;

namespace ScratchScript.Core.Diagnostics;

public class DiagnosticReporter
{
    public static void Error(ScratchScriptError error, ParserRuleContext start, ParserRuleContext conflicting, params object[] data)
    {
        Console.WriteLine("ERROR");
    }

    public static void Error(ScratchScriptError error, ParserRuleContext start, IToken conflicting,
        params object[] data)
    {
        Console.WriteLine("ERROR");
    }

    public static void Warning(ScratchScriptWarning warning, IToken start, IToken conflicting)
    {
        Console.WriteLine("WARNING");
    }
}