using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Target
{
    [JsonProperty("blocks")] public Dictionary<string, Block> Blocks = new();


    [JsonProperty("broadcasts")] public Dictionary<string, string> Broadcasts = new();


    [JsonProperty("comments")] public Dictionary<string, Comment> Comments = new();


    [JsonProperty("costumes")] public List<Costume> Costumes = new();


    [JsonProperty("currentCostume")] public int CurrentCostume;


    [JsonProperty("isStage")] public bool IsStage;


    [JsonProperty("layerOrder")] public int LayerOrder;


    [JsonProperty("lists")] public Dictionary<string, List<object>> Lists = new();


    [JsonProperty("name")] public string Name;


    [JsonProperty("sounds")] public List<Sound> Sounds = new();


    [JsonProperty("variables")] public Dictionary<string, List<object>> Variables = new();


    [JsonProperty("volume")] public int Volume = 100;
}