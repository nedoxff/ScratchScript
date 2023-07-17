using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Metadata
{
    [JsonProperty("agent")] public string UserAgent = "";
    [JsonProperty("semver")] public string Version = "3.0.0";
    [JsonProperty("vm")] public string Vm = "0.2.0";
}