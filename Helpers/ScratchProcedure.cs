using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScratchScript.Core.Blocks;
using ScratchScript.Core.Frontend.Implementation;
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
    public Dictionary<string, ProcedureArgumentType> Arguments;
    public bool Warp;
    public Block Definition;
    public Block Prototype;
    public Block Call => _call.Clone();
    
    private Block _call;
    private Mutation _mutation;
    public readonly Block StackIndexReporter;

    public ScratchProcedure(string name, bool warp, Dictionary<string, ProcedureArgumentType> arguments)
    {
        Name = name;
        Warp = warp;
        Prototype = Procedures.Prototype();
        Definition = Procedures.Definition();
        Arguments = arguments;
        if (arguments.Count != 0)
        {
            StackIndexReporter = Procedures.ReporterStringNumber(ScratchScriptVisitor.StackIndexArgumentName);
            StackIndexReporter.SetParent(Prototype, false);
        }
    }

    public void Build()
    {
        var proccode = $"{Name} {(StackIndexReporter == null ? "": "%s")}";
        proccode = proccode.Trim();

        var idsArray = new JArray();
        var namesArray = new JArray();
        var defaultsArray = new JArray();

        if (StackIndexReporter != null)
        {
            idsArray.Add(StackIndexReporter.Id);
            namesArray.Add(ScratchScriptVisitor.StackIndexArgumentName);
            defaultsArray.Add("");
        }

        _mutation = new Mutation
        {
            ProcCode = proccode,
            ArgumentIds = idsArray.ToString(Formatting.None),
            ArgumentNames = namesArray.ToString(Formatting.None),
            ArgumentDefaults = defaultsArray.ToString(Formatting.None),
            Warp = Warp
        };

        Prototype.Mutation = _mutation;
        
        Definition.SetInput("custom_block", ScratchInput.New(Prototype, Definition, false));
        
        _call = Procedures.Call();
        _call.Mutation = _mutation;
    }
}