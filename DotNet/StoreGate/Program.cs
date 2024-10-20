using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoreGate;
using StoreGate.Common;
using StoreGate.Common.Commands;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;
using Version = StoreGate.GitHub.Models.Version;

if (args.Length == 0)
{
    ShowHelp();
    return;
}

InitBinder();
ServiceCollection serviceCollection = new();
ConfigureServices(serviceCollection, GetCommandType(args[0]));
ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

AppDomain.CurrentDomain.UnhandledException += (_, e)
    => serviceProvider.GetRequiredService<ILogger<Program>>().LogCritical((Exception)e.ExceptionObject, "StoreGate encountered an error.");

await serviceCollection.BuildServiceProvider().GetRequiredService<CommandRunner>().RunAsync(args);


#region Config

static void InitBinder()
{
    OptionBinder.AddRule<Version>(Version.Parse);
}

static void ConfigureServices(IServiceCollection serviceCollection, Type commandType)
{
    serviceCollection.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddProvider(new GitHubLoggerProvider());
    });
    ConfigureConfigs(serviceCollection);
    serviceCollection.AddAllTransient<IService>();
    serviceCollection.AddTransient<CommandRunner>();
    serviceCollection.AddTransient(typeof(AbstractCommand), commandType);
}

static void ConfigureConfigs(IServiceCollection serviceCollection)
{
    serviceCollection.AddSingleton(new Config()
    {
        Repo = EnvironmentHelper.GetEnvironmentVariable(Constants.GitHub.Environment.Repo),
        Token = EnvironmentHelper.GetEnvironmentVariable(Constants.GitHub.Environment.Token),
    });
}

#endregion

#region Init Command

static void ShowHelp()
{
    IEnumerable<string> commandsHelp = Utils.FindAllTypes<ICommand>()
        .Select(t => t.GetCustomAttribute<CommandAttribute>())
        .Where(c => c != null)
        .OrderBy(c => c?.Name)
        .Select(c => $"{c?.Name,-20} | {c?.Description}");
    Console.WriteLine(string.Join("\n", commandsHelp));
}

static Type GetCommandType(string command)
{
    Type? commandType = Utils.FindAllTypes<ICommand>().FirstOrDefault(t => t.GetCustomAttribute<CommandAttribute>()?.Name == command);
    if (commandType == null)
    {
        throw new Exception($"Command not found: \"{command}\".");
    }

    return commandType;
}

#endregion