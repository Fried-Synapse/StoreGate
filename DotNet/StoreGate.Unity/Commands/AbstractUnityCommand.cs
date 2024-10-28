using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.Unity.Services;

namespace StoreGate.Unity.Commands;

public abstract class AbstractUnityCommand : AbstractCommand
{
    protected AbstractUnityCommand(
        UnityService unityService,
        UnityEnvironment environment,
        ILogger logger)
        : base(logger)
    {
        Environment = environment;
        UnityService = unityService;
    }

    protected UnityEnvironment Environment { get; }
    protected UnityService UnityService { get; }

    protected async Task<bool> TryActivate()
    {
        if (string.IsNullOrEmpty(Environment.Username) || string.IsNullOrEmpty(Environment.Password))
        {
            return false;
        }

        string serial = string.IsNullOrEmpty(Environment.Licence) ? Environment.Serial : UnityService.GetSerial(Environment.Licence);

        await UnityService.ActivateAsync(Environment.Username, Environment.Password, serial);

        return true;
    }
}