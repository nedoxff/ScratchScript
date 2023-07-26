using CliWrap;
using CliWrap.Exceptions;
using ScratchScript.Core;
using ScratchScript.Core.Reflection;
using Serilog;
using Spectre.Console.Cli;

namespace ScratchScript.Commands;

public class RunCommand : AsyncCommand<RunCommand.RunCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, RunCommandSettings settings)
    {
        if (!File.Exists(settings.File)) Console.WriteLine($"The specified script ({settings.File}) was not found!");

        if (string.IsNullOrEmpty(Config.Instance.RunnerPath))
        {
            Console.WriteLine("Cannot run a script without the runner path specified!");
            return 1;
        }

        if (settings.Verbose) Static.LogToConsole = true;

        try
        {

            ReflectionBlockLoader.Load();
            StdLoader.Load(Path.Join(Environment.CurrentDirectory, "std"));

            var output = Path.Join(Path.GetTempPath(), $"temp_{Guid.NewGuid():N}.sb3");
            var manager = new ProjectManager(settings.File, settings.IrPath, output);
            var success = manager.Build();

            if (success)
            {
                try
                {
                    Log.Information("--- Launching the project ---");
                    await Task.Delay(500);
                    Cli.Wrap(Config.Instance.RunnerPath)
                        .WithArguments(output)
                        .ExecuteAsync();
                    Log.Information("Done!");
                }
                catch (Exception e)
                {
                    Log.Fatal("Failed to run the project file! Reason: {Message}", e);
                }
            }

            return success ? 0 : 1;
        }
        catch (Exception e)
        {
            return 1;
        }
    }

    public class RunCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[file]")] public string File { get; init; }
        [CommandOption("--ir-path")] public string IrPath { get; init; } 
        [CommandOption("-v|--verbose")] public bool Verbose { get; init; }
    }
}