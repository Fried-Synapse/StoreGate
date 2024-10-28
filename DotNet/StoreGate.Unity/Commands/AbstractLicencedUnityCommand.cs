using Microsoft.Extensions.Logging;
using StoreGate.Common.Commands;
using StoreGate.Unity.Services;

namespace StoreGate.Unity.Commands;

public abstract class AbstractLicencedUnityCommand : AbstractCommand
{
    protected AbstractLicencedUnityCommand(
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

    protected abstract Task RunLicencedAsync();

    public override async Task RunAsync()
    {
        await ActivateLicenceAsync();
        await RunLicencedAsync();
        await ReturnLicenceAsync();
    }

    protected async Task ActivateLicenceAsync()
    {
        if (string.IsNullOrEmpty(Environment.Username) || string.IsNullOrEmpty(Environment.Password))
        {
            throw new KeyNotFoundException("Could not activate unity. Missing username or password");
        }

        string serial = string.IsNullOrEmpty(Environment.Licence) ? Environment.Serial : UnityService.GetSerial(Environment.Licence);

        await UnityService.ActivateLicenceAsync(Environment.Username, Environment.Password, serial);
    }

    protected async Task ReturnLicenceAsync()
    {
        await UnityService.ReturnLicenceAsync();
    }
}