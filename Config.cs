using Newtonsoft.Json;
using Serilog;

namespace ScratchScript;

public class Config
{
    private static readonly Config Default = new()
    {
        RunnerPath = "",
        DeveloperEditorPath = ""
    };

    public static Config Instance = Default;

    public string RunnerPath;
    public string DeveloperEditorPath;

    private static void SaveDefault()
    {
        try
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(Default, Formatting.Indented));
        }
        catch
        {
            // ignored
        }
    }

    public static void Load()
    {
        if (!File.Exists("config.json"))
        {
            Log.Information("config.json does not exist, creating a new file");
            SaveDefault();
            return;
        }

        try
        {
            var deserialized = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            Instance = deserialized ?? throw new Exception();
        }
        catch
        {
            Log.Fatal(
                "Failed to deserialize config.json! Please ensure that there are no errors in the JSON and/or delete the file entirely.");
        }
    }
}