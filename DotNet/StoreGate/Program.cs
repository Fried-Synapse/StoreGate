using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoreGate.Common;
using StoreGate.Common.Commands;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub;
using StoreGate.Unity;
using Environment = StoreGate.Common.Models.Environment;
using Version = StoreGate.Common.Models.Version;

if (args.Length == 0)
{
    ShowHelp();
    return;
}

InitBinder();
ServiceCollection serviceCollection = new();
ConfigureServices(serviceCollection, GetTypes(args[0]));
ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

AppDomain.CurrentDomain.UnhandledException += (_, e)
    => serviceProvider.GetRequiredService<ILogger<Program>>().LogCritical((Exception)e.ExceptionObject, "StoreGate encountered an error.");

await serviceCollection.BuildServiceProvider().GetRequiredService<CommandRunner>().RunAsync(args);


#region Config

static void InitBinder()
{
    OptionBinder.AddRule<Version>(Version.Parse);
}

static void ConfigureServices(IServiceCollection serviceCollection, (Type Command, Type Environment) types)
{
    serviceCollection.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddProvider(new GitHubLoggerProvider());
    });
    serviceCollection.AddSingleton(types.Environment);
    serviceCollection.AddAllTransient<IService>();
    serviceCollection.AddTransient<CommandRunner>();
    serviceCollection.AddTransient(typeof(AbstractCommand), types.Command);
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

static (Type Command, Type Environment) GetTypes(string command)
{
    //HACK we need to force load the assemblies and this is the only way
    _ = new GitHubEnvironment();
    _ = new UnityEnvironment();

    Type? commandType = Utils.FindAllTypes<ICommand>().FirstOrDefault(t => t.GetCustomAttribute<CommandAttribute>()?.Name == command);
    if (commandType == null)
    {
        throw new Exception($"Command not found: \"{command}\".");
    }

    Type? environmentType = Utils.FindAllTypes<Environment>().FirstOrDefault(t => t.Assembly == commandType.Assembly);
    if (environmentType == null)
    {
        throw new Exception($"Environment not found for command: \"{command}\".");
    }

    return (commandType, environmentType);
}

#endregion