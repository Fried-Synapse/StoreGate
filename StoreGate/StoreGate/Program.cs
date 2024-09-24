using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StoreGate.Commands;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;

ServiceCollection serviceCollection = new();
ConfigureServices(serviceCollection);
ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
await serviceProvider.GetRequiredService<CommandRunner>().RunAsync(args);

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new GitHubData()
    {
        Owner = "Fried-Synapse",
        Repo = "StoreGate",
    });
    services.AddAllTransient<IService>();
    services.AddTransient<CommandRunner>();
    services.AddTransient(typeof(AbstractCommand), GetCommandType(args[0]));
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