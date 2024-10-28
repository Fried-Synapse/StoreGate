using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.Unity.Services;

namespace StoreGate.Unity.Commands;

[Command("unityActivate", "Activates Unity's licence")]
public class ActivateCommand : AbstractLicencedUnityCommand
{
    public ActivateCommand(
        UnityService unityService,
        UnityEnvironment environment,
        ILogger<ActivateCommand> logger)
        : base(unityService, environment, logger)
    {
    }

    protected override async Task RunLicencedAsync()
    {
        await Task.CompletedTask;
    }
}