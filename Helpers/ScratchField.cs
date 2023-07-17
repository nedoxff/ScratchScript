using ScratchScript.Core.Models;

namespace ScratchScript.Helpers;

public class ScratchField
{
    public static List<object> New(object o)
    {
        return o switch
        {
            ScratchVariable variable => FromVariable(variable),
            Block block => FromVariableBlock(block),
            _ => FromValue(o)
        };
    }
    
    public static List<object> FromVariable(ScratchVariable variable) => new() { variable.Name, variable.Id };
    public static List<object> FromVariableBlock(Block b) => b.CustomData.ContainsKey("VARIABLE_NAME") ? new() { b.CustomData["VARIABLE_NAME"], b.CustomData["VARIABLE_ID"] }: FromArgumentReporter(b.CustomData["ARGUMENT_NAME"]);
    public static List<object> FromValue(object o) => new() { o, null };
    public static List<object> FromArgumentReporter(string name) =>
        new() { name, "0" }; //TODO: defaultValue seems to be always 0, but needs to be tested.
}