using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks.Extensions;

public class Pen
{
    [ScratchBlock("scratch/pen", "clear", false, true)]
    public static string Clear() => "raw pen_clear";

    [ScratchBlock("scratch/pen", "stamp", false, true)]
    public static string Stamp() => "raw pen_stamp";

    [ScratchBlock("scratch/pen", "penDown", false, true)]
    public static string PenDown() => "raw pen_penDown";
    
    [ScratchBlock("scratch/pen", "penUp", false, true)]
    public static string PenUp() => "raw pen_penUp";

    [ScratchBlock("scratch/pen", "setPenColor", false, true)]
    public static string SetPenColor([ScratchArgument("color", ScratchTypeKind.Color)] string color) => $"raw pen_setPenColorToColor i:COLOR:{color}";

    [ScratchBlock("scratch/pen", "changePenProperty", false, true)]
    public static string ChangePenProperty(
        [ScratchArgument("what", ScratchTypeKind.String,
            new object[] { "color", "saturation", "brightness", "transparency" })]
        string what, [ScratchArgument("amount", ScratchTypeKind.Number)] string amount)
        => $"raw pen_changePenColorParamBy i:COLOR_PARAM:(rawshadow pen_menu_colorParam f:colorParam:\"{what.RemoveQuotes()}\" endshadow) i:VALUE:{amount}";

    [ScratchBlock("scratch/pen", "setPenProperty", false, true)]
    public static string SetPenProperty(
        [ScratchArgument("what", ScratchTypeKind.String,
            new object[] { "color", "saturation", "brightness", "transparency" })]
        string what, [ScratchArgument("amount", ScratchTypeKind.Number)] string amount)
        => $"raw pen_setPenColorParamTo i:COLOR_PARAM:(rawshadow pen_menu_colorParam f:colorParam:\"{what.RemoveQuotes()}\" endshadow) i:VALUE:{amount}";

    [ScratchBlock("scratch/pen", "changePenSize", false, true)]
    public static string ChangePenSize([ScratchArgument("amount", ScratchTypeKind.Number)] string amount) => $"raw pen_changePenSizeBy i:SIZE:{amount}";
    
    [ScratchBlock("scratch/pen", "setPenSize", false, true)]
    public static string SetPenSize([ScratchArgument("amount", ScratchTypeKind.Number)] string amount) => $"raw pen_setPenSizeTo i:SIZE:{amount}";
}