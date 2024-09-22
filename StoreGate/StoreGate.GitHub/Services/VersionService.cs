using System.Net;
using StoreGate.Common;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;
using StoreGate.GitHub.Services.Web;

namespace StoreGate.GitHub.Services;

public class VersionService : AbstractService
{
    public VersionService(VariableService variableService)
    {
        VariableService = variableService;
    }

    private VariableService VariableService { get; }

    public async Task<StoreGateVersion> GetOrDefaultAsync(string variable)
    {
        string? response;
        try
        {
            response = await VariableService.GetAsync(variable);
        }
        catch (ApiException ex) when (ex.ResponseStatusCode == HttpStatusCode.NotFound)
        {
            return new StoreGateVersion();
        }

        if (!StoreGateVersion.TryParse(response, out StoreGateVersion version))
        {
            throw new Exception($"Wrong version found: \"{response}\".");
        }

        return version;
    }

    public async Task Set(string variable, StoreGateVersion version)
    {
        await VariableService.CreateOrUpdateAsync(variable, version.ToString());
    }
}