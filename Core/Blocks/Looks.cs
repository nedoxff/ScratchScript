using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Looks
{
    [ScratchBlock("scratch/looks", "sayTimed", false, true)]
    public static string SayForSeconds([ScratchArgument("message", ScratchTypeKind.String)] string message,
        [ScratchArgument("secs", ScratchTypeKind.Number)]
        string seconds) =>
        $"raw looks_sayforsecs i:MESSAGE:{message} i:SECS:{seconds}";

    [ScratchBlock("scratch/looks", "say", false, true)]
    public static string Say([ScratchArgument("message", ScratchTypeKind.String)] string message) =>
        $"raw looks_say i:MESSAGE:{message}";

    [ScratchBlock("scratch/looks", "thinkTimed", false, true)]
    public static string ThinkForSeconds([ScratchArgument("message", ScratchTypeKind.String)] string message,
        [ScratchArgument("secs", ScratchTypeKind.Number)]
        string seconds) =>
        $"raw looks_thinkforsecs i:MESSAGE:{message} i:SECS:{seconds}";

    [ScratchBlock("scratch/looks", "think", false, true)]
    public static string Think([ScratchArgument("message", ScratchTypeKind.String)] string message) =>
        $"raw think i:MESSAGE:{message}";

    [ScratchBlock("scratch/looks", "show", false, true)]
    public static string Show() => "raw looks_show";

    [ScratchBlock("scratch/looks", "hide", false, true)]
    public static string Hide() => "raw looks_hide";

    [ScratchBlock("scratch/looks", "changeLookEffect", false, true)]
    public static string ChangeEffectBy(
        [ScratchArgument("effect", ScratchTypeKind.String,
            new object[] { "color", "fisheye", "whirl", "pixelate", "mosaic", "brightness", "ghost" })]
        string effect, [ScratchArgument("change", ScratchTypeKind.Number)] string change
    ) => $"raw looks_changeeffectby f:EFFECT:{effect} i:CHANGE:{change}";
    
    [ScratchBlock("scratch/looks", "setLookEffect", false, true)]
    public static string SetEffectTo(
        [ScratchArgument("effect", ScratchTypeKind.String,
            new object[] { "color", "fisheye", "whirl", "pixelate", "mosaic", "brightness", "ghost" })]
        string effect, [ScratchArgument("value", ScratchTypeKind.Number)] string value
    ) => $"raw looks_seteffectto f:EFFECT:{effect} i:VALUE:{value}";

    [ScratchBlock("scratch/looks", "clearLookEffects", false, true)]
    public static string ClearEffects() => "raw looks_cleargraphiceffects";

    [ScratchBlock("scratch/looks", "changeSize", false, true)]
    public static string ChangeSizeBy([ScratchArgument("change", ScratchTypeKind.Number)] string change) =>
        $"raw looks_changesizeby i:CHANGE:{change}";

    [ScratchBlock("scratch/looks", "setSize", false, true)]
    public static string SetSizeTo([ScratchArgument("size", ScratchTypeKind.Number)] string size) =>
        $"raw looks_setsizeto i:SIZE:{size}";

    [ScratchBlock("scratch/looks", "getSize", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetSize() => "rawshadow looks_size endshadow";

    [ScratchBlock("scratch/looks", "switchCostume", false, true)]
    public static string SwitchCostumeTo([ScratchArgument("name", ScratchTypeKind.String)] string name) =>
        $"raw looks_switchcostumeto i:COSTUME:{(name.IsVariable() ? name.Format(): $"(rawshadow looks_costume f:COSTUME:{name} endshadow)")}";
    [ScratchBlock("scratch/looks", "switchCostumeIndex", false, true)]
    public static string SwitchCostumeToIndex([ScratchArgument("idx", ScratchTypeKind.Number)] string idx) =>
        $"raw looks_switchcostumeto i:COSTUME:(* {idx} 1)";

    [ScratchBlock("scratch/looks", "nextCostume", false, true)]
    public static string NextCostume() => "raw looks_nextcostume";

    [ScratchBlock("scratch/looks", "switchBackdrop", false, true)]
    public static string SwitchBackdropTo([ScratchArgument("name", ScratchTypeKind.String)] string name) =>
        $"raw looks_switchbackdropto i:BACKDROP:(rawshadow looks_backdrops f:BACKDROP:{name} endshadow)";

    [ScratchBlock("scratch/looks", "switchBackdropAndWait", false, true)]
    public static string SwitchBackdropToAndWait([ScratchArgument("name", ScratchTypeKind.String)] string name) =>
        $"raw looks_switchbackdroptoandwait i:BACKDROP:(rawshadow looks_backdrops f:BACKDROP:{name} endshadow)";
    
    [ScratchBlock("scratch/looks", "nextBackdrop", false, true)]
    public static string NextBackdrop() => "raw looks_nextbackdrop";

    [ScratchBlock("scratch/looks", "goToLayer", false, true)]
    public static string GoToFrontBack(
        [ScratchArgument("layer", ScratchTypeKind.String, new object[] { "front", "back" })] string layer) =>
        $"raw looks_gotofrontback f:FRONT_BACK:\"{layer}\"";

    [ScratchBlock("scratch/looks", "switchLayers", false, true)]
    public static string GoForwardBackwardLayers(
        [ScratchArgument("layer", ScratchTypeKind.String, new object[] { "front", "back" })] string layer,
        [ScratchArgument("amount", ScratchTypeKind.Number)] string amount) =>
        $"raw looks_goforwardbackwardlayers f:FORWARD_BACKWARD:\"{layer}\" i:NUM:{amount}";

    [ScratchBlock("scratch/looks", "getBackdropNumber", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetBackdropNumber() => "rawshadow looks_backdropnumbername f:NUMBER_NAME:\"number\" endshadow";
    
    [ScratchBlock("scratch/looks", "getBackdropName", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.String)]
    public static string GetBackdropName() => "rawshadow looks_backdropnumbername f:NUMBER_NAME:\"name\" endshadow";

    [ScratchBlock("scratch/looks", "getCostumeNumber", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetCostumeNumber() => "rawshadow looks_costumenumbername f:NUMBER_NAME:\"number\" endshadow";
    
    [ScratchBlock("scratch/looks", "getCostumeName", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.String)]
    public static string GetCostumeName() => "rawshadow looks_costumenumbername f:NUMBER_NAME:\"name\" endshadow";
}