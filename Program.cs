using System.Reflection;
using ScratchScript;
using ScratchScript.Commands;
using Serilog;
using Spectre.Console.Cli;

InitializeLogger();
Config.Load();
Run();

void Run()
{
    var app = new CommandApp();
    app.Configure(c =>
    {
        c.SetApplicationName("ScratchScript");
        var assembly = Assembly.GetExecutingAssembly();
        c.SetApplicationVersion(assembly.GetName().Version?.ToString() ?? "development");

        c.AddCommand<RunCommand>("run");
    });
    app.Run(args);
}

void InitializeLogger()
{
    File.WriteAllText("log.txt", "");
    Log.Logger = new LoggerConfiguration()
#if DEBUG
        .MinimumLevel.Verbose() //TODO: add verbose property to config
#else
        .MinimumLevel.Information()
#endif
        .WriteTo.File("log.txt")
        .WriteTo.Conditional(_ => Static.LogToConsole, cs => cs.Console())
        .CreateLogger();
}