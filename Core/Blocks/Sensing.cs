using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Sensing
{
    [ScratchBlock("scratch/sensing", "isTouching", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string IsTouchingObject(
        [ScratchArgument("object", ScratchType.String, new object[] { "mouse", "edge" })]
        string obj) =>
        $"rawshadow sensing_touchingobject i:TOUCHINGOBJECTMENU:(rawshadow sensing_touchingobjectmenu f:TOUCHINGOBJECTMENU:\"_{obj.RemoveQuotes()}_\" endshadow) endshadow";

    [ScratchBlock("scratch/sensing", "isTouchingColor", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string IsTouchingColor([ScratchArgument("color", ScratchType.Color)] string color) =>
        $"rawshadow sensing_touchingcolor i:COLOR:{color} endshadow";

    [ScratchBlock("scratch/sensing", "isColorTouchingColor", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string IsColorTouchingColor([ScratchArgument("first", ScratchType.Color)] string first,
        [ScratchArgument("second", ScratchType.Color)]
        string second) =>
        $"rawshadow sensing_touchingcolor i:COLOR:{first} i:COLOR2:{second} endshadow";

    //TODO: this requires checking the sprites
    [ScratchBlock("scratch/sensing", "distanceTo", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string DistanceTo([ScratchArgument("object", ScratchType.String)] string obj) =>
        $"rawshadow sensing_distanceto i:DISTANCETOMENU:(rawshadow f:DISTANCETOMENU:{(obj == "mouse" ? "\"_mouse_\"" : obj)} endshadow) endshadow";

    [ScratchBlock("scratch/sensing", "ask", false, true)]
    public static string AskAndWait([ScratchArgument("question", ScratchType.String)] string question) =>
        $"raw sensing_askandwait i:QUESTION:{question}";

    [ScratchBlock("scratch/sensing", "getAnswer", true, true, ScratchType.Unknown, ScratchType.String)]
    public static string GetAnswer() => "rawshadow sensing_answer endshadow";

    // why c#
    [ScratchBlock("scratch/sensing", "isKeyPressed", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string IsKeyPressed([ScratchArgument("key", ScratchType.String, new object[]
        {
            "space", "up arrow", "down arrow", "right arrow", "left arrow", "any", "a", "b", "c", "d", "e", "f", "g",
            "h",
            "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2",
            "3",
            "4", "5", "6", "7", "8", "9"
        })]
        string key) =>
        $"rawshadow sensing_keypressed i:KEY_OPTION:(rawshadow sensing_keyoptions f:KEY_OPTION:{key} endshadow) endshadow";

    [ScratchBlock("scratch/sensing", "isMouseDown", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string IsMouseDown() => "rawshadow sensing_mousedown endshadow";

    [ScratchBlock("scratch/sensing", "getMouseX", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string GetMouseX() => "rawshadow sensing_mousex endshadow";

    [ScratchBlock("scratch/sensing", "getMouseY", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string GetMouseY() => "rawshadow sensing_mousey endshadow";

    [ScratchBlock("scratch/sensing", "setDragMode", true, true, ScratchType.Unknown, ScratchType.Boolean)]
    public static string SetDragMode(
        [ScratchArgument("mode", ScratchType.String, new object[] { "draggable", "not draggable" })]
        string mode) => $"rawshadow sensing_setdragmode f:DRAG_MODE:{mode} endshadow";

    [ScratchBlock("scratch/sensing", "getLoudness", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetLoudness() => "rawshadow sensing_loudness endshadow";

    [ScratchBlock("scratch/sensing", "getTimer", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetTimer() => "rawshadow sensing_timer endshadow";

    [ScratchBlock("scratch/sensing", "resetTimer", false, true)]
    public static string ResetTimer() => "raw sensing_resettimer";
    
    //TODO: IMPLEMENT THE SENSING_OF BLOCK (it requires non-static fields!!!!!!!!!!!!!!!!!!!!!)

    [ScratchBlock("scratch/sensing", "getYear", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetYear() => "rawshadow sensing_current f:CURRENTMENU:\"YEAR\" endshadow";
    [ScratchBlock("scratch/sensing", "getMonth", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetMonth() => "rawshadow sensing_current f:CURRENTMENU:\"MONTH\" endshadow";
    [ScratchBlock("scratch/sensing", "getDay", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetDay() => "rawshadow sensing_current f:CURRENTMENU:\"DATE\" endshadow";
    [ScratchBlock("scratch/sensing", "getDayOfWeek", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetDayOfWeek() => "rawshadow sensing_current f:CURRENTMENU:\"DAYOFWEEK\" endshadow";
    [ScratchBlock("scratch/sensing", "getHour", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetHour() => "rawshadow sensing_current f:CURRENTMENU:\"HOUR\" endshadow";
    [ScratchBlock("scratch/sensing", "getMinute", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetMinute() => "rawshadow sensing_current f:CURRENTMENU:\"MINUTE\" endshadow";
    [ScratchBlock("scratch/sensing", "getSecond", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetSecond() => "rawshadow sensing_current f:CURRENTMENU:\"SECOND\" endshadow";
    
    // this is such a silly function. why scratch
    [ScratchBlock("scratch/sensing", "getDaysSince2000", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetDaysSince2000() => "rawshadow sensing_dayssince2000 endshadow";

    [ScratchBlock("scratch/sensing", "getUsername", true, true, ScratchType.Unknown, ScratchType.String)]
    public static string GetUsername() => "rawshadow sensing_username endshadow";

}