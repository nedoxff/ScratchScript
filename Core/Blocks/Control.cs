using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Control
{
    public static Block If() => new("control_if", "if");
    public static Block IfElse() => new("control_if_else", "ife");
    public static Block Repeat() => new("control_repeat", "rp");
    public static Block While() => new("control_while", "wh");
}