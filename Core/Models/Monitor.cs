using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Monitor
{
    [JsonProperty("height")] public float Height;


    [JsonProperty("id")] public string Id;


    [JsonProperty("isDiscrete")] public bool IsDiscrete;


    [JsonProperty("mode")] public string Mode;


    [JsonProperty("opcode")] public string Opcode;


    [JsonProperty("params")] public Dictionary<string, string> Params;


    [JsonProperty("sliderMax")] public float SliderMaximum;


    [JsonProperty("sliderMin")] public float SliderMinimum;


    [JsonProperty("spriteName")] public string SpriteName;


    [JsonProperty("value")] public object Value;


    [JsonProperty("visible")] public bool Visible;


    [JsonProperty("width")] public float Width;


    [JsonProperty("x")] public float X;


    [JsonProperty("y")] public float Y;
}