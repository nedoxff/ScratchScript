using ScratchScript.Core.Models;
using ScratchScript.Core.Reflection;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public static class Data
{
    public static Block SetVariableTo() =>
        new("data_setvariableto", "dsvt");

    public static Block Variable(ScratchVariable variable) => new("data_variable", "dv", shadow: true)
    {
        CustomData = new Dictionary<string, string>
        {
            { "VARIABLE_NAME", variable.Name },
            { "VARIABLE_ID", variable.Id }
        },
        ExpectedType = variable.Type
    };

    public static Block List(ScratchVariable list) => new("data_listcontents", "dlc", shadow: true)
    {
        CustomData = new Dictionary<string, string>
        {
            { "VARIABLE_NAME", list.Name },
            { "VARIABLE_ID", list.Id }
        },
    };

    public static Block DeleteAllOfList() => new("data_deletealloflist", "ddalol");
    public static Block DeleteFromList() => new("data_deleteoflist", "ddol"); //input INDEX, field LIST
    public static Block InsertIntoList() => new("data_insertatlist", "dial"); //input ITEM INDEX, field LIST
    public static Block ReplaceInList() => new("data_replaceitemoflist", "driol"); //input ITEM INDEX, field LIST
    public static Block LengthOfList() => new("data_lengthoflist", "dlol", shadow: true); //field LIST
    public static Block AddToList() => new("data_addtolist", "datl"); //input ITEM, field LIST
    public static Block ItemOfList() => new("data_itemoflist", "diol", shadow: true); //input INDEX, field LIST

    [ScratchBlock("scratch/data", "show", false, false, ScratchTypeKind.Variable)]
    public static string ShowVariable([ScratchArgument("variable", ScratchTypeKind.Variable)] string variable) =>
        $"raw data_showvariable f:VARIABLE:{variable}";

    [ScratchBlock("scratch/data", "hide", false, false, ScratchTypeKind.Variable)]
    public static string HideVariable([ScratchArgument("variable", ScratchTypeKind.Variable)] string variable) =>
        $"raw data_hidevariable f:VARIABLE:{variable}";

    [ScratchBlock("scratch/data", "add", false, false, ScratchTypeKind.List)]
    public static string AddToList([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("value", ScratchTypeKind.Unknown)]
        string value) => $"raw data_addtolist f:LIST:{list} i:ITEM:{value}";

    [ScratchBlock("scratch/data", "delete", false, false, ScratchTypeKind.List)]
    public static string DeleteOfList([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("index", ScratchTypeKind.Number)]
        string index) =>
        $"raw data_deleteoflist f:LIST:{list} i:INDEX:{index}";

    [ScratchBlock("scratch/data", "deleteAll", false, false, ScratchTypeKind.List)]
    public static string DeleteAllOfList([ScratchArgument("list", ScratchTypeKind.List)] string list) =>
        $"raw data_deletealloflist f:LIST:{list}";

    [ScratchBlock("scratch/data", "insert", false, false, ScratchTypeKind.List)]
    public static string InsertAtList([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("index", ScratchTypeKind.Number)]
        string index,
        [ScratchArgument("value", ScratchTypeKind.Unknown)]
        string value) =>
        $"raw data_insertatlist f:LIST:{list} i:INDEX:{index} i:ITEM:{value}";

    [ScratchBlock("scratch/data", "replace", false, false, ScratchTypeKind.List)]
    public static string ReplaceItemOfList([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("index", ScratchTypeKind.Number)]
        string index,
        [ScratchArgument("value", ScratchTypeKind.Unknown)]
        string value) =>
        $"raw data_replaceitemoflist f:LIST:{list} i:INDEX:{index} i:ITEM:{value}";

    [ScratchBlock("scratch/data", "length", true, false, ScratchTypeKind.List, ScratchTypeKind.Number)]
    public static string LengthOfList([ScratchArgument("list", ScratchTypeKind.List)] string list) =>
        $"rawshadow data_lengthoflist f:LIST:{list} endshadow";

    [ScratchBlock("scratch/data", "contains", true, false, ScratchTypeKind.List, ScratchTypeKind.Boolean)]
    public static string ListContainsItem([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("item", ScratchTypeKind.Unknown)]
        string item) => $"rawshadow data_listcontainsitem f:LIST:{list} i:ITEM:{item} endshadow";

    [ScratchBlock("scratch/data", "show", false, false, ScratchTypeKind.List)]
    public static string ShowList([ScratchArgument("list", ScratchTypeKind.List)] string list) =>
        $"raw data_showlist f:LIST:{list}";

    [ScratchBlock("scratch/data", "hide", false, false, ScratchTypeKind.List)]
    public static string HideList([ScratchArgument("list", ScratchTypeKind.List)] string list) =>
        $"raw data_hidelist f:LIST:{list}";

    [ScratchBlock("scratch/data", "findIndex", true, false, ScratchTypeKind.List, ScratchTypeKind.Number)]
    public static string ItemNumberOfList([ScratchArgument("list", ScratchTypeKind.List)] string list,
        [ScratchArgument("value", ScratchTypeKind.Any)] string obj) =>
        $"rawshadow data_itemnumoflist f:LIST:{list} i:ITEM:{obj} endshadow";
}