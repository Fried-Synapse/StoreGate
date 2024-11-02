using Microsoft.Extensions.Logging;
using StoreGate.Commands.Common;
using StoreGate.Services.Unity;

namespace StoreGate.Commands.Unity;

[Command("unityActivate", "Activates Unity's licence")]
public class ActivateCommand : AbstractLicencedUnityCommand
{
    public ActivateCommand(
        UnityService unityService,
        ILogger<ActivateCommand> logger)
        : base(unityService, logger)
    {
    }

    protected override async Task RunLicencedAsync()
    {
        await Task.CompletedTask;
    }
}