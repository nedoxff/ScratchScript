using ScratchScript.Core.Models;
using ScratchScript.Core.Reflection;
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

    [ScratchBlock("scratch/operators", "toString", true, false, ScratchType.Number, ScratchType.String)]
    public static string ToString([ScratchArgument("num", ScratchType.Number)] string num) => $"rawshadow operator_join i:STRING1:{num} i:STRING2:\"\" endshadow";

    [ScratchBlock("scratch/operators", "random", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Random([ScratchArgument("min", ScratchType.Number)] string min,
        [ScratchArgument("max", ScratchType.Number)]
        string max) =>
        $"rawshadow operator_random i:FROM:{min} i:TO:{max} endshadow";

    [ScratchBlock("scratch/operators", "round", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Round([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_round i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "length", true, false, ScratchType.String, ScratchType.Number)]
    public static string Length([ScratchArgument("str", ScratchType.String)] string str) =>
        $"rawshadow operator_length i:STRING:{str} endshadow";

    [ScratchBlock("scratch/operators", "contains", true, false, ScratchType.String, ScratchType.Boolean)]
    public static string Contains([ScratchArgument("str", ScratchType.String)] string str,
        [ScratchArgument("str2", ScratchType.String)] string str2) =>
        $"rawshadow operator_contains i:STRING1:{str} i:STRING2:{str2} endshadow";

    [ScratchBlock("scratch/operators", "abs", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Abs([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"abs\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "floor", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Floor([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"floor\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "ceil", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Ceiling([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"ceiling\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "sqrt", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Sqrt([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"sqrt\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "sin", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Sin([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"sin\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "cos", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Cos([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"cos\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "tan", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Tan([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"tan\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "asin", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Asin([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"asin\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "acos", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Acos([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"acos\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "atan", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Atan([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"atan\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "ln", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Ln([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"ln\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "log", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Log([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"log\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "exp", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string Exp([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"e ^\" i:NUM:{num} endshadow";

    [ScratchBlock("scratch/operators", "powerOf10", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string PowerOf10([ScratchArgument("num", ScratchType.Number)] string num) =>
        $"rawshadow operator_mathop f:OPERATOR:\"10 ^\" i:NUM:{num} endshadow";
}