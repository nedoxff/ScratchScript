using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Mutation
{
    [JsonProperty("argumentdefaults")] public string ArgumentDefaults = "[]";


    [JsonProperty("argumentids")] public string ArgumentIds = "[]";


    [JsonProperty("argumentnames")] public string ArgumentNames = "[]";


    [JsonProperty("children")] public List<object> Children = new();


    [JsonProperty("hasnext")] public bool HasNext;


    [JsonProperty("proccode")] public string ProcCode;


    [JsonProperty("tagName")] public string TagName = "mutation";


    [JsonProperty("warp")] public bool Warp;
}