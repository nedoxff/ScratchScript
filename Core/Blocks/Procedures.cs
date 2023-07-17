using ScratchScript.Core.Models;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Procedures
{
    public static Block Definition() => new("procedures_definition", "pd", topLevel: true);
    public static Block Prototype() => new("procedures_prototype", "pp", shadow: true);
    public static Block Call() => new("procedures_call", "pc");

    public static Block ReporterStringNumber(string name)
    {
        var block = new Block("argument_reporter_string_number", "arsn", shadow: true);
        block.SetField("VALUE", ScratchField.FromArgumentReporter(name));
        return block;
    }
    
    public static Block ReporterBoolean(string name)
    {
        var block = new Block("argument_reporter_boolean", "arb", shadow: true);
        block.SetField("VALUE", ScratchField.FromArgumentReporter(name));
        return block;
    }
}