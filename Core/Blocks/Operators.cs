using ScratchScript.Core.Models;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Operators
{
    public static Block Join() => new("operator_join", "oj");
    public static Block Add() => new("operator_add", "o+", ScratchType.Number, shadow: true);
    public static Block Subtract() => new("operator_subtract", "o-", ScratchType.Number, shadow: true);
    public static Block Multiply() => new("operator_multiply", "o*", ScratchType.Number, shadow: true);
    public static Block Divide() => new("operator_divide", "o/", ScratchType.Number, shadow: true);
    public static Block Modulus() => new("operator_mod", "o%", ScratchType.Number, shadow: true);

    public static Block LessThan() => new("operator_lt", "o<", ScratchType.Boolean, shadow: true);
    public static Block Equals() => new("operator_equals", "o=", ScratchType.Boolean, shadow: true);
    public static Block GreaterThan() => new("operator_gt", "o>", ScratchType.Boolean, shadow: true);

    public static Block And() => new("operator_and", "o&", ScratchType.Boolean, shadow: true);
    public static Block Or() => new("operator_or", "o|", ScratchType.Boolean, shadow: true);

    public static Block Not() => new("operator_not", "o!", ScratchType.Boolean, shadow: true);
}