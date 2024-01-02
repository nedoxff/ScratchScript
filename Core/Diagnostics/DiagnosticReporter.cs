using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using ScratchScript.Core.Frontend.Implementation;
using ScratchScript.Extensions;
using Spectre.Console;

namespace ScratchScript.Core.Diagnostics;

public class DiagnosticReporter
{
    public static void Note(ScratchScriptNote note, ParserRuleContext start, ParserRuleContext conflicting,
        params object[] data)
    {
        NoteInternal(note, start, conflicting.Start.StartIndex, conflicting.Stop.StopIndex, conflicting.Start.Column,
            conflicting.Start.Line, data);
    }
    
    public static void Error(ScratchScriptError error, ParserRuleContext start, ParserRuleContext conflicting,
        params object[] data)
    {
        ErrorInternal(error, start, conflicting.Start.StartIndex, conflicting.Stop.StopIndex, conflicting.Start.Column,
            conflicting.Start.Line, data);
    }

    public static void Error(ScratchScriptError error, ParserRuleContext start, IToken conflicting,
        params object[] data)
    {
        ErrorInternal(error, start, conflicting.StartIndex, conflicting.StopIndex, conflicting.Column,
            conflicting.Line, data);
    }

    private static void ErrorInternal(ScratchScriptError error, ParserRuleContext start, int conflictingStart,
        int conflictingStop, int conflictingColumn, int conflictingLine,
        params object[] data)
    {
        ScratchScriptVisitor.Instance.Success = false;
        var inputStream = start.Start.InputStream;
        ParserRuleContext lineContext = start.GetParent<ScratchScriptParser.TopLevelStatementContext>();
        var text = inputStream.GetText(new Interval(lineContext.Start.StartIndex, lineContext.Stop.StopIndex)).Replace("[", "[[").Replace("]", "]]");
        var conflictingText = inputStream.GetText(new Interval(conflictingStart, conflictingStop));

        var startLine = lineContext.Start.Line;
        var endingLine = lineContext.Stop.Line;
        var linePadding = Math.Max(startLine, endingLine).ToString().Length;
        var underline = new string(' ', LimitAtZero(conflictingColumn)) +
                        new string('^', conflictingText.Length) + " here";

        var message = string.Format(DiagnosticMessages.Errors[(int)error], data);
        AnsiConsole.MarkupLine($"[red]error[[E{(int)error}]][/]: {message}");
        AnsiConsole.MarkupLine(
            $"{new string(' ', linePadding)}[grey]-->[/] {ScratchScriptVisitor.Instance.InputFile}:{conflictingLine}:{conflictingColumn + 1}");

        for (var i = startLine; i <= endingLine; i++)
        {
            if (Math.Abs(conflictingLine - i) > 1) continue;
            AnsiConsole.MarkupLine(
                $"[grey]{i}{new string(' ', LimitAtZero(linePadding - i.ToString().Length + 1))}|[/] {text.Split("\n")[i - startLine]}");
            if (i == conflictingLine)
                AnsiConsole.MarkupLine($"[grey]{new string(' ', linePadding + 1)}|[/] [red]{underline}[/]");
        }

        AnsiConsole.MarkupLine($"For more information about this error, try [yellow]`scrs explain E{(int)error}`[/].");
    }

    private static int LimitAtZero(int x) => x < 0 ? 0 : x;

    private static void WarningInternal(ScratchScriptWarning warning, ParserRuleContext start, int conflictingStart,
        int conflictingStop, int conflictingColumn, int conflictingLine,
        params object[] data)
    {
        var inputStream = start.Start.InputStream;
        ParserRuleContext lineContext = start.GetParent<ScratchScriptParser.LineContext>();
        lineContext ??= start.GetParent<ScratchScriptParser.TopLevelStatementContext>();
        var text = inputStream.GetText(new Interval(lineContext.Start.StartIndex, lineContext.Stop.StopIndex)).Replace("[", "[[").Replace("]", "]]");
        var conflictingText = inputStream.GetText(new Interval(conflictingStart, conflictingStop));

        var startLine = lineContext.Start.Line;
        var endingLine = lineContext.Stop.Line;
        var linePadding = Math.Max(startLine, endingLine).ToString().Length;
        var underline = new string(' ', LimitAtZero(conflictingColumn - linePadding - 3)) + new string('^', conflictingText.Length) +
                        " here";

        var message = string.Format(DiagnosticMessages.Warnings[(int)warning], data);
        AnsiConsole.MarkupLine($"[yellow]warning[[W{(int)warning}]][/]: {message}");
        AnsiConsole.MarkupLine(
            $"{new string(' ', linePadding)}[grey]-->[/] {ScratchScriptVisitor.Instance.InputFile}:{conflictingLine}:{conflictingColumn + 1}");

        for (var i = startLine; i <= endingLine; i++)
        {
            if (Math.Abs(conflictingLine - i) > 1) continue;
            AnsiConsole.MarkupLine(
                $"[grey]{i}{new string(' ', LimitAtZero(linePadding - i.ToString().Length + 1))}|[/] {text.Split("\n")[i - startLine]}");
            if (i == conflictingLine)
                AnsiConsole.MarkupLine($"[grey]{new string(' ', linePadding + 1)}|[/] [yellow]{underline}[/]");
        }

        AnsiConsole.MarkupLine(
            $"For more information about this warning, try [yellow]`scrs explain W{(int)warning}`[/].");
    }
    
    private static void NoteInternal(ScratchScriptNote note, ParserRuleContext start, int conflictingStart,
        int conflictingStop, int conflictingColumn, int conflictingLine,
        params object[] data)
    {
        var inputStream = start.Start.InputStream;
        ParserRuleContext lineContext = start.GetParent<ScratchScriptParser.LineContext>();
        lineContext ??= start.GetParent<ScratchScriptParser.TopLevelStatementContext>();
        var text = inputStream.GetText(new Interval(lineContext.Start.StartIndex, lineContext.Stop.StopIndex)).Replace("[", "[[").Replace("]", "]]");
        var conflictingText = inputStream.GetText(new Interval(conflictingStart, conflictingStop));

        var startLine = lineContext.Start.Line;
        var endingLine = lineContext.Stop.Line;
        var linePadding = Math.Max(startLine, endingLine).ToString().Length;
        var underline = new string(' ', LimitAtZero(conflictingColumn - linePadding - 3)) + new string('^', conflictingText.Length) +
                        " here";

        var message = string.Format(DiagnosticMessages.Notes[(int)note], data);
        AnsiConsole.MarkupLine($"[grey]note[[N{(int)note}]][/]: {message}");
        AnsiConsole.MarkupLine(
            $"{new string(' ', linePadding)}[grey]-->[/] {ScratchScriptVisitor.Instance.InputFile}:{conflictingLine}:{conflictingColumn + 1}");

        for (var i = startLine; i <= endingLine; i++)
        {
            if (Math.Abs(conflictingLine - i) > 1) continue;
            AnsiConsole.MarkupLine(
                $"[grey]{i}{new string(' ', LimitAtZero(linePadding - i.ToString().Length + 1))}|[/] {text.Split("\n")[i - startLine]}");
            if (i == conflictingLine)
                AnsiConsole.MarkupLine($"[grey]{new string(' ', linePadding + 1)}|[/] [grey]{underline}[/]");
        }
    }

    public static void Warning(ScratchScriptWarning warning, ParserRuleContext start, ParserRuleContext conflicting,
        params object[] data)
    {
        WarningInternal(warning, start, conflicting.Start.StartIndex, conflicting.Stop.StopIndex,
            conflicting.Start.Column, conflicting.Start.Line, data);
    }

    public static void Warning(ScratchScriptWarning warning, ParserRuleContext start, IToken conflicting,
        params object[] data)
    {
        WarningInternal(warning, start, conflicting.StartIndex, conflicting.StopIndex, conflicting.Column,
            conflicting.Line, data);
    }
}