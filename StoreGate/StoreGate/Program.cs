using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StoreGate;
using StoreGate.Commands.Common;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;

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
    string[] repositoryInfo = Environment.GetEnvironmentVariable(Constants.Environment.GitHub.Repo)?.Split('/') ??
                        throw new KeyNotFoundException(Constants.Environment.GitHub.Repo);
    services.AddSingleton(new GitHubConfig()
    {
        Owner = repositoryInfo[0],
        Repo = repositoryInfo[1],
        Token = Environment.GetEnvironmentVariable(Constants.Environment.GitHub.Token) ??
                throw new KeyNotFoundException(Constants.Environment.GitHub.Token),
    });
}

static void ShowHelp()
{
    IEnumerable<string> commandsHelp = Utils.FindAllTypes<ICommand>()
        .Select(t => t.GetCustomAttribute<CommandAttribute>())
        .Where(c => c != null)
        .Select(c => $"{c.Name,-20} | {c.Description}");
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