﻿using CliWrap;
using CliWrap.Exceptions;
using ScratchScript.Core;
using ScratchScript.Core.Reflection;
using Serilog;
using Spectre.Console.Cli;

namespace ScratchScript.Commands;

public class CompileCommand : AsyncCommand<CompileCommand.CompileCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, CompileCommandSettings settings)
    {
        if (!File.Exists(settings.File)) Console.WriteLine($"The specified script ({settings.File}) was not found!");

        if (settings.Verbose) Static.LogToConsole = true;
        if(settings.DevelopmentMode) Static.DeveloperMode = true;

        try
        {
            ReflectionBlockLoader.Load();

            var output = Path.Join(Path.GetTempPath(), $"temp_{Guid.NewGuid():N}.sb3");
            var manager = new ProjectManager(settings.File, settings.IrPath, output);
            var success = manager.Build();

            if (success && !string.IsNullOrEmpty(Config.Instance.RunnerPath) && settings.Run)
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

    public class CompileCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[file]")] public string File { get; init; }
        [CommandOption("--ir-path")] public string IrPath { get; init; } 
        [CommandOption("-b|--verbose")] public bool Verbose { get; init; } //TODO: temporary change, -v is reserved by accident and displays the version (https://github.com/spectreconsole/spectre.console/issues/1400)
        [CommandOption("-r|--run")] public bool Run { get; init; }
        [CommandOption("-d|--developer")] public bool DevelopmentMode { get; init; }
    }
}