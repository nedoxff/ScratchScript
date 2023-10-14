using System.Globalization;
using ScratchScript.Helpers;

namespace ScratchScript.Extensions;

public static class StringExtensions
{
    public static string RemoveEmptyLines(this string s) => string.Join('\n', s.Split("\n").Where(x => !string.IsNullOrEmpty(x)));
    public static string RemoveQuotes(this string s) => s.Replace("\"", "");
    public static string Format(this object o, bool rawColor = true, bool escapeStrings = false, bool processString = false)
    {
        return o switch
        {
            null => "",
            bool b => $"\"{b.ToString().ToLower()}\"",
            decimal d => d.ToString(CultureInfo.InvariantCulture),
            string s => ProcessString(escapeStrings ? $"\"{s}\"": s, processString),
            ScratchColor c => rawColor ? $"#{c.Value.ToLower()}": $"\"0x{c.Value.ToLower()}\"",
            ScratchVariable var => $"var:{var.Id}",
            TypedValue v => v.Value.Format(rawColor),
            _ => o.ToString()
        };
    }

    private static string ProcessString(string str, bool enabled) => enabled ? str.Replace("\\\\", "\\").Replace("\\\"", "\""): str;

    public static bool IsVariable(this object o) => o.Format().StartsWith("var:");
    public static bool IsList(this object o) => o.Format().StartsWith("arr:");
}