using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StoreGate;
using StoreGate.Common.Commands;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;
using Version = StoreGate.GitHub.Models.Version;

InitBinder();
ServiceCollection serviceCollection = new();
ConfigureServices(serviceCollection, args);
if (args.Length == 0)
{
    ShowHelp();
}
else
{
    ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
    await serviceProvider.GetRequiredService<CommandRunner>().RunAsync(args);
}


static void InitBinder()
{
    OptionBinder.AddRule<Version>(Version.Parse);
}

static void ConfigureServices(IServiceCollection services, string[] args)
{
    ConfigureConfigs(services);
    services.AddAllTransient<IService>();
    services.AddTransient<CommandRunner>();
    if (args.Length != 0)
    {
        services.AddTransient(typeof(AbstractCommand), GetCommandType(args[0]));
    }
}

static void ConfigureConfigs(IServiceCollection services)
{
    services.AddSingleton(new Config()
    {
        // Repo = Environment.GetEnvironmentVariable(Constants.GitHub.Environment.Repo) ??
        //        throw new KeyNotFoundException(Constants.GitHub.Environment.Repo),
        // Token = Environment.GetEnvironmentVariable(Constants.GitHub.Environment.Token) ??
        //         throw new KeyNotFoundException(Constants.GitHub.Environment.Token),
    });
}

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