using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Comment
{
    [JsonProperty("blockId")] public string BlockId;


    [JsonProperty("height")] public float Height;


    [JsonProperty("minimized")] public bool Minimized;


    [JsonProperty("text")] public string Text;


    [JsonProperty("width")] public float Width;


    [JsonProperty("x")] public float X;


    [JsonProperty("y")] public float Y;
}