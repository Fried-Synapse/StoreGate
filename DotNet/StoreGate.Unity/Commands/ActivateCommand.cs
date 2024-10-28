using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.Unity.Services;

namespace StoreGate.Unity.Commands;

[Command("unityActivate", "Activates Unity's licence")]
public class ActivateCommand : AbstractUnityCommand
{
    public ActivateCommand(
        UnityService unityService,
        UnityEnvironment environment,
        ILogger<ActivateCommand> logger)
        : base(unityService, environment, logger)
    {
    }

    public override async Task RunAsync()
        => await TryActivate();
}