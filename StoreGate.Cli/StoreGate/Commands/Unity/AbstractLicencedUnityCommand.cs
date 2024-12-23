using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using StoreGate.Commands.Common;
using StoreGate.Common.Environments;
using StoreGate.Services.Unity;

namespace StoreGate.Commands.Unity;

public abstract class AbstractLicencedUnityCommand : AbstractCommand
{
    protected AbstractLicencedUnityCommand(
        UnityService unityService,
        ILogger logger)
        : base(logger)
    {
        UnityService = unityService;
    }

    protected UnityService UnityService { get; }

    protected abstract Task RunLicencedAsync();

    public override async Task RunAsync()
    {
#if RELEASE
        await ActivateLicenceAsync();
#endif
        await RunLicencedAsync();
#if RELEASE
        await ReturnLicenceAsync();
#endif
    }

    private async Task ActivateLicenceAsync()
    {
        Guard.IsNotNullOrEmpty(UnityEnvironment.Username, "Could not activate unity. Missing username");
        Guard.IsNotNullOrEmpty(UnityEnvironment.Password, "Could not activate unity. Missing password");

        string serial = string.IsNullOrEmpty(UnityEnvironment.Licence)
            ? UnityEnvironment.Serial
            : UnityService.GetSerial(UnityEnvironment.Licence);

        await UnityService.ActivateLicenceAsync(UnityEnvironment.Username, UnityEnvironment.Password, serial);
    }

    private async Task ReturnLicenceAsync()
    {
        await UnityService.ReturnLicenceAsync();
    }
}