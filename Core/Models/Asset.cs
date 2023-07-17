using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Asset
{
    [JsonProperty("assetId")] public string AssetId;


    [JsonProperty("dataFormat")] public string DataFormat;


    [JsonProperty("md5ext")] public string Md5Extension;


    [JsonProperty("name")] public string Name;


    [NonSerialized] public byte[] Data;
}