using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Sprite : Target
{
    [JsonProperty("direction")] public float Direction = 90;


    [JsonProperty("draggable")] public bool Draggable;


    [JsonProperty("rotationStyle")] public string RotationStyle = "all around";


    [JsonProperty("size")] public float Size = 100;


    [JsonProperty("visible")] public bool Visible = true;


    [JsonProperty("x")] public float X;


    [JsonProperty("y")] public float Y;
}