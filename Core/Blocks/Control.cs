using ScratchScript.Core.Models;
using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Control
{
    public static Block If() => new("control_if", "if");
    public static Block IfElse() => new("control_if_else", "ife");
    public static Block Repeat() => new("control_repeat", "rp");
    public static Block While() => new("control_while", "wh");

    [ScratchBlock("scratch/control", "wait", false, true)]
    public static string Wait([ScratchArgument("seconds", ScratchType.Number)] string seconds) =>
        $"raw control_wait i:DURATION:{seconds}";

    [ScratchBlock("scratch/control", "waitUntil", false, true)]
    public static string WaitUntil([ScratchArgument("condition", ScratchType.Boolean)] string condition) =>
        $"raw control_wait_until i:CONDITION:{condition}";

    [ScratchBlock("scratch/control", "stop", false, true)]
    public static string Stop(
        [ScratchArgument("what", ScratchType.String, new object[] { "all", "this script", "other scripts" })]
        string what) => $"raw control_stop f:STOP_OPTION:{what}";
}