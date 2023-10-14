using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Motion
{
    [ScratchBlock("scratch/motion", "move", false, true)]
    public static string MoveSteps([ScratchArgument("steps", ScratchTypeKind.Number)] string steps) =>
        $"raw motion_movesteps i:STEPS:{steps}";

    [ScratchBlock("scratch/motion", "turnClockwise", false, true)]
    public static string TurnClockwise([ScratchArgument("degrees", ScratchTypeKind.Number)] string degrees) =>
        $"raw motion_turnright i:DEGREES:{degrees}";

    [ScratchBlock("scratch/motion", "turnCounterClockwise", false, true)]
    public static string TurnCounterClockwise([ScratchArgument("degrees", ScratchTypeKind.Number)] string degrees) =>
        $"raw motion_turnleft i:DEGREES:{degrees}";

    [ScratchBlock("scratch/motion", "pointInDirection", false, true)]
    public static string PointInDirection([ScratchArgument("direction", ScratchTypeKind.Number)] string direction) =>
        $"raw motion_pointindirection i:DIRECTION:{direction}";

    [ScratchBlock("scratch/motion", "pointTowards", false, true)]
    public static string PointTowards(
        [ScratchArgument("towards", ScratchTypeKind.String, new object[] { "mouse", "random" })]
        string towards) =>
        $"raw motion_pointtowards i:TOWARDS:(rawshadow motion_pointtowards_menu i:TOWARDS:\"_{towards.RemoveQuotes()}_\" endshadow)";

    [ScratchBlock("scratch/motion", "goTo", false, true)]
    public static string GoTo(
        [ScratchArgument("to", ScratchTypeKind.String, new object[] { "mouse", "random" })]
        string to) =>
        $"raw motion_goto i:TO:(rawshadow motion_goto_menu f:TO:\"_{to.RemoveQuotes()}_\" endshadow)";

    [ScratchBlock("scratch/motion", "goToXY", false, true)]
    public static string GoToXy([ScratchArgument("x", ScratchTypeKind.Number)] string x,
        [ScratchArgument("y", ScratchTypeKind.Number)]
        string y) =>
        $"raw motion_gotoxy i:X:{x} i:Y:{y}";

    [ScratchBlock("scratch/motion", "glideToXY", false, true)]
    public static string GlideToXy([ScratchArgument("x", ScratchTypeKind.Number)] string x,
        [ScratchArgument("y", ScratchTypeKind.Number)]
        string y, [ScratchArgument("secs", ScratchTypeKind.Number)] string secs) =>
        $"raw motion_glidesecstoxy i:X:{x} i:Y:{y} i:SECS:{secs}";

    [ScratchBlock("scratch/motion", "glideTo", false, true)]
    public static string GlideTo(
        [ScratchArgument("to", ScratchTypeKind.String, new object[] { "mouse", "random" })]
        string to, [ScratchArgument("secs", ScratchTypeKind.Number)] string secs) =>
        $"raw motion_glideto i:TO:(rawshadow motion_glideto_menu f:TO:\"_{to.RemoveQuotes()}_\" endshadow) i:SECS:{secs}";

    [ScratchBlock("scratch/motion", "changeX", false, true)]
    public static string ChangeX([ScratchArgument("dx", ScratchTypeKind.Number)] string dx) =>
        $"raw motion_changexby i:DX:{dx}";

    [ScratchBlock("scratch/motion", "setX", false, true)]
    public static string SetX([ScratchArgument("x", ScratchTypeKind.Number)] string x) =>
        $"raw motion_setx i:X:{x}";
    
    [ScratchBlock("scratch/motion", "changeY", false, true)]
    public static string ChangeY([ScratchArgument("dy", ScratchTypeKind.Number)] string dy) =>
        $"raw motion_changeyby i:DY:{dy}";

    [ScratchBlock("scratch/motion", "setY", false, true)]
    public static string SetY([ScratchArgument("y", ScratchTypeKind.Number)] string y) =>
        $"raw motion_sety i:Y:{y}";

    [ScratchBlock("scratch/motion", "bounceOnEdge", false, true)]
    public static string BounceOnEdge() => "raw motion_ifonedgebounce";

    [ScratchBlock("scratch/motion", "setRotationStyle", false, true)]
    public static string SetRotationStyle(
        [ScratchArgument("style", ScratchTypeKind.String,
            new object[] { "left-right", "don't rotate", "all around" })]
        string style) =>
        $"raw motion_setrotationstyle i:STYLE:\"{style.RemoveQuotes()}\"";

    [ScratchBlock("scratch/motion", "getX", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetX() => "rawshadow motion_xposition endshadow";
    
    [ScratchBlock("scratch/motion", "getY", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetY() => "rawshadow motion_yposition endshadow";
    
    [ScratchBlock("scratch/motion", "getDirection", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetDirection() => "rawshadow motion_direction endshadow";
}