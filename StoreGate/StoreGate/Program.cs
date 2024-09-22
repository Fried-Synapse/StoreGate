using Microsoft.Extensions.DependencyInjection;
using StoreGate.Common.Extensions;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services;

ServiceCollection serviceCollection = new();
ConfigureServices(serviceCollection);
ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new GitHubData()
    {
        Owner = "Fried-Synapse",
        Repo = "StoreGate",
    });
    services.AddAllTransient<IService>();
}

VersionService versionService = serviceProvider.GetRequiredService<VersionService>();
StoreGateVersion version = await versionService.GetOrDefaultAsync("Version");
version.Patch++;
Console.WriteLine(version);
await versionService.Set("Version", version);