using Newtonsoft.Json;

namespace ScratchScript.Core.Models;

public class Stage : Target
{
    [JsonProperty("tempo")] public int Tempo = 60;


    [JsonProperty("textToSpeechLanguage")] public string TextToSpeechLanguage;


    [JsonProperty("videoState")] public string VideoState = "on";


    [JsonProperty("videoTransparency")] public int VideoTransparency = 50;
}