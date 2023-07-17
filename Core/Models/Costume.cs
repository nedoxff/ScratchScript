using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Costume : Asset
{
    [JsonProperty("bitmapResolution")] public float? BitmapResolution = 2;


    [JsonProperty("rotationCenterX")] public float RotationCenterX;


    [JsonProperty("rotationCenterY")] public float RotationCenterY;
}