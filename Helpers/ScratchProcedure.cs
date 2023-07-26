using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Models;
using ScratchScript.Extensions;

namespace ScratchScript.Helpers;


public class ScratchProcedure
{
    public enum ProcedureArgumentType
    {
        StringNumber,
        Boolean
    }
    public string Name;
    public Dictionary<string, Block> Arguments = new();
    public bool Warp;
    public Block Definition;
    public Block Prototype;
    public Block Call => _call.Clone();
    
    private Block _call;
    private Mutation _mutation;

    public ScratchProcedure(string name, bool warp, Dictionary<string, ProcedureArgumentType> arguments)
    {
        Name = name;
        Warp = warp;
        Prototype = Procedures.Prototype();
        Definition = Procedures.Definition();
        foreach (var (arg, type) in arguments)
        {
            Arguments[arg] = type == ProcedureArgumentType.StringNumber ? Procedures.ReporterStringNumber(arg): Procedures.ReporterBoolean(arg);
            Arguments[arg].SetParent(Prototype, false);
        }
    }

    public void Build()
    {
        var proccode = $"{Name} ";
        proccode = Arguments.Aggregate(proccode, (current, argument) => current + (argument.Value.Opcode == "argument_reporter_string_number" ? "%s " : "%b "));
        proccode = proccode.Trim();

        var idsArray = new JArray();
        var namesArray = new JArray();
        var defaultsArray = new JArray();
        foreach (var (argumentName, block) in Arguments)
        {
            var id = block.Id;
            //function.ArgumentIds[argumentName] = id;
            idsArray.Add(id);
            namesArray.Add(argumentName);
            defaultsArray.Add(block.Opcode == "argument_reporter_string_number" ? "": "false");
        }
        var ids = idsArray.ToString(Formatting.None);
        var names = namesArray.ToString(Formatting.None);
        var defaults = defaultsArray.ToString(Formatting.None);
        
        _mutation = new Mutation
        {
            ProcCode = proccode,
            ArgumentIds = ids,
            ArgumentNames = names,
            ArgumentDefaults = defaults,
            Warp = Warp
        };
        
        
        Prototype.Mutation = _mutation;
        
        Definition.SetInput("custom_block", ScratchInput.New(Prototype, Definition, false));
        
        _call = Procedures.Call();
        _call.Mutation = _mutation;
    }
}