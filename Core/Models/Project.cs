using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Project
{
    [JsonProperty("extensions")] public List<string> Extensions = new();

    [NonSerialized] public int LayerOrder;


    [JsonProperty("meta")] public Metadata Metadata = new();


    [JsonProperty("monitors")] public List<Monitor> Monitors = new();


    [JsonProperty("targets")] public List<Target> Targets = new();
}