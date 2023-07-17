using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Sound : Asset
{
    [JsonProperty("rate")] public int Rate;


    [JsonProperty("sampleCount")] public int SampleCount;
}