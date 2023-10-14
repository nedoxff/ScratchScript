using Newtonsoft.Json;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Models;

public class Block
{
    #region JSON Structure

    [JsonProperty("opcode")] public string Opcode;


    [JsonProperty("next")] public string Next;


    [JsonProperty("parent")] public string Parent;


    [JsonProperty("inputs")] public Dictionary<string, List<object>> Inputs = new();


    [JsonProperty("fields")] public Dictionary<string, List<object>> Fields = new();


    [JsonProperty("shadow")] public bool Shadow;


    [JsonProperty("topLevel")] public bool TopLevel;


    [JsonProperty("x")] public float X;


    [JsonProperty("y")] public float Y;

    
    [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)] public string Comment;


    [JsonProperty("mutation", NullValueHandling = NullValueHandling.Ignore)] public Mutation Mutation;

    #endregion

    #region Custom

    [NonSerialized] public string Id;
    [NonSerialized] public string ShortName;
    [NonSerialized] public ScratchType ExpectedType;
    [NonSerialized] public Dictionary<string, string> CustomData = new();

    public Block(string opcode, string shortName, ScratchType type = null, bool shadow = false, bool topLevel = false)
    {
        Opcode = opcode;
        ShortName = shortName;
        Id = NameHelper.New(shortName ?? "unset");
        Shadow = shadow;
        TopLevel = topLevel;
        ExpectedType = type ?? ScratchType.Unknown;
    }
    
    public override string ToString()
    {
        return $"{Opcode} ({Id.Replace(Opcode + "_", "")})";
    }

    #endregion
}