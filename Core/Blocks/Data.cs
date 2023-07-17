using ScratchScript.Core.Models;
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
            {"VARIABLE_NAME", variable.Name},
            {"VARIABLE_ID", variable.Id}
        },
        ExpectedType = variable.Type
    };

    public static Block DeleteAllOfList() => new("data_deletealloflist", "ddalol");
    public static Block DeleteFromList() => new("data_deleteoflist", "ddol"); //input INDEX, field LIST
    public static Block InsertIntoList() => new("data_insertatlist", "dial"); //input ITEM INDEX, field LIST
    public static Block ReplaceInList() => new("data_replaceitemoflist", "driol"); //input ITEM INDEX, field LIST
    public static Block LengthOfList() => new("data_lengthoflist", "dlol", shadow: true); //field LIST
    public static Block AddToList() => new("data_addtolist", "datl"); //input ITEM, field LIST
    public static Block ItemOfList() => new("data_itemoflist", "diol", shadow: true); //input INDEX, field LIST
}